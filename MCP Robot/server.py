"""
Minimal MCP server for structural weight optimization.

Run:
  pip install "mcp[cli]" httpx
  python server.py             # MCP stdio mode (for Claude Desktop)
  python server.py --selftest  # Standalone: hit the API + POST a sample optimization
"""

import logging
import os
import sys
from typing import Any

import httpx
from mcp.server.fastmcp import FastMCP

# ---------------------------------------------------------------------------
# Logging
# ---------------------------------------------------------------------------
# CRITICAL: log to STDERR. STDOUT is the MCP JSON-RPC channel; printing there
# corrupts the stream and the host drops the server.
logging.basicConfig(
    level=logging.INFO,
    stream=sys.stderr,
    format="[%(asctime)s] %(levelname)s %(name)s: %(message)s",
    datefmt="%H:%M:%S",
)
log = logging.getLogger("structure-optimizer")

# ---------------------------------------------------------------------------
# Config
# ---------------------------------------------------------------------------
API_BASE = os.environ.get("REPORTS_API_BASE", "https://localhost:5001/api/v1")
VERIFY_TLS = os.environ.get("VERIFY_TLS", "false").lower() == "true"

log.info("Booting MCP server")
log.info("  API_BASE   = %s", API_BASE)
log.info("  VERIFY_TLS = %s", VERIFY_TLS)

mcp = FastMCP("structure-optimizer")
_client = httpx.AsyncClient(verify=VERIFY_TLS, timeout=30.0)


# ---------------------------------------------------------------------------
# API tools
# ---------------------------------------------------------------------------
@mcp.tool()
async def get_reports() -> list[dict[str, Any]]:
    """Fetch all structural reports from the Reports API."""
    url = f"{API_BASE}/Reports"
    log.info("GET %s", url)
    try:
        r = await _client.get(url)
        log.info("  <- %s (%d bytes)", r.status_code, len(r.content))
        r.raise_for_status()
        return r.json()
    except Exception as e:
        log.exception("get_reports failed: %s", e)
        raise


@mcp.tool()
async def get_report(report_id: str) -> dict[str, Any]:
    """Fetch a single report by reportId (GUID)."""
    url = f"{API_BASE}/Reports/{report_id}"
    log.info("GET %s", url)
    try:
        r = await _client.get(url)
        log.info("  <- %s (%d bytes)", r.status_code, len(r.content))
        r.raise_for_status()
        return r.json()
    except Exception as e:
        log.exception("get_report failed: %s", e)
        raise


@mcp.tool()
async def submit_optimization(
    report_id: str,
    ai_recommendation: str,
    optimization_strategy: str,
    current_weight_kg: float,
    projected_weight_kg: float,
    design_modifications: list[str],
    parameter_suggestions: list[str],
    suggested_modifications: list[str],
    results: list[dict[str, str]],
) -> dict[str, Any]:
    """
    POST an optimization payload to /Reports/{reportId}/optimizations.
    `results` is a list of {"parameter": str, "before": str, "after": str}.
    """
    payload = {
        "AIRecommendation": ai_recommendation,
        "OptimizationStrategy": optimization_strategy,
        "CurrentWeightKg": current_weight_kg,
        "ProjectedWeightKg": projected_weight_kg,
        "DesignModifications": design_modifications,
        "ParameterSuggestions": parameter_suggestions,
        "SuggestedModifications": suggested_modifications,
        "Results": [
            {
                "Parameter": r.get("parameter", ""),
                "Before": r.get("before", ""),
                "After": r.get("after", ""),
            }
            for r in results
        ],
    }
    url = f"{API_BASE}/Reports/{report_id}/optimizations"
    log.info("POST %s", url)
    log.info("  payload: current=%.1fkg projected=%.1fkg results=%d",
             current_weight_kg, projected_weight_kg, len(results))
    try:
        r = await _client.post(url, json=payload)
        log.info("  <- %s", r.status_code)
        r.raise_for_status()
        return {"status": r.status_code, "response": r.json()}
    except Exception as e:
        log.exception("submit_optimization failed: %s", e)
        raise


# ---------------------------------------------------------------------------
# Heuristic 2D optimizer on L, H, alpha
# ---------------------------------------------------------------------------
LINEAR_MASS = {
    "IPE 200": 22.35, "IPE 220": 26.20, "IPE 240": 30.70, "IPE 270": 36.10,
    "IPE 300": 42.23, "IPE 330": 49.10, "IPE 360": 57.10, "IPE 400": 66.30,
    "IPE 450": 77.60, "IPE 500": 90.70, "IPE 550": 105.48, "IPE 600": 122.40,
    "CAE 50x5": 3.77, "CAE 60x6": 5.42, "CAE 70x7": 7.38,
}

DOWNSIZE = {
    "IPE 550": "IPE 500", "IPE 500": "IPE 450", "IPE 450": "IPE 400",
    "IPE 400": "IPE 360", "IPE 360": "IPE 330", "IPE 330": "IPE 300",
    "IPE 300": "IPE 270",
}


@mcp.tool()
def analyze_report(report: dict[str, Any]) -> dict[str, Any]:
    """
    Heuristic 2D weight optimization. Takes a report object from
    get_reports / get_report; returns a payload shaped for submit_optimization.
    Does NOT call the API.
    """
    log.info("analyze_report: reportId=%s",
             _get_field(report, "reportId", "ReportId"))

    raw_rows = _get_field(report, "rows", "Rows") or []
    rows = [r for r in raw_rows if not _get_field(r, "isHeader", "IsHeader")]
    current_kg = float(_get_field(report, "grandTotalKg", "GrandTotalKg") or 0)

    by_section: dict[str, dict[str, float]] = {}
    for row in rows:
        section = _get_field(row, "type", "Type") or ""
        n = float(_get_field(row, "nombre", "Nombre") or 0)
        length = float(_get_field(row, "lengthM", "LengthM") or 0)
        if section not in by_section:
            by_section[section] = {"total_len_m": 0.0, "count": 0.0}
        by_section[section]["total_len_m"] += n * length
        by_section[section]["count"] += n

    contributions = []
    for section, agg in by_section.items():
        lm = LINEAR_MASS.get(section, 0)
        contributions.append((section, agg["total_len_m"], lm * agg["total_len_m"]))
    contributions.sort(key=lambda x: x[2], reverse=True)

    log.info("  sections: %d, top: %s",
             len(contributions),
             contributions[0][0] if contributions else "none")

    results: list[dict[str, str]] = []
    design_mods: list[str] = []
    projected_kg = current_kg

    for section, total_len, _ in contributions:
        new_section = DOWNSIZE.get(section)
        if not new_section or total_len <= 0:
            continue
        new_lm = LINEAR_MASS.get(new_section, LINEAR_MASS.get(section, 0))
        old_lm = LINEAR_MASS.get(section, 0)
        saved = (old_lm - new_lm) * total_len
        if saved <= 0:
            continue
        projected_kg -= saved
        results.append({
            "parameter": f"Section {section}",
            "before": f"{section} ({old_lm} kg/m, {total_len:.2f} m total)",
            "after": f"{new_section} ({new_lm} kg/m) -> ~{saved:.0f} kg saved",
        })
        design_mods.append(
            f"Replace {section} with {new_section} on non-critical spans "
            f"(verify deflection and buckling)."
        )

    param_suggestions = [
        "Reduce span L by ~5-10% where layout allows; bending demand scales with L^2 "
        "for distributed loads, so small L reductions yield disproportionate weight savings.",
        "Increase column height H only as required for clearance; taller H raises "
        "second-order (P-delta) demand and column section size.",
        "Increase roof slope alpha to 8-12 deg to lower bending moments at the ridge "
        "and at the eaves, enabling smaller rafter sections.",
        "Consider haunched rafters at the eaves: local depth increase lets the mid-span "
        "section drop one IPE size.",
    ]

    suggested_mods = [
        "Re-run the 2D portal-frame analysis with the proposed sections.",
        "Check serviceability (deflection L/200 or L/250) before committing to downsizes.",
        "Validate lateral-torsional buckling for the new rafter sections.",
    ]

    strategy = (
        "Two-step approach: (1) Section optimization - downsize the largest "
        "contributors by one IPE size where capacity allows. (2) Geometry tuning "
        "in the 2D model on L, H, alpha to reduce internal forces."
    )

    saving_pct = (current_kg - projected_kg) / max(current_kg, 1) * 100
    ai_reco = (
        f"Current weight: {current_kg:.0f} kg. Projected: {projected_kg:.0f} kg "
        f"(~{saving_pct:.1f}% saving). Largest contributor: "
        f"{contributions[0][0] if contributions else 'n/a'}. "
        "Combine with geometry tuning on L, H, alpha for additional gains."
    )

    log.info("  proposal: %.0f kg -> %.0f kg (%.1f%% saving)",
             current_kg, projected_kg, saving_pct)

    return {
        "report_id": _get_field(report, "reportId", "ReportId"),
        "ai_recommendation": ai_reco,
        "optimization_strategy": strategy,
        "current_weight_kg": current_kg,
        "projected_weight_kg": round(projected_kg, 1),
        "design_modifications": design_mods,
        "parameter_suggestions": param_suggestions,
        "suggested_modifications": suggested_mods,
        "results": results,
    }


@mcp.tool()
def _get_field(d: dict[str, Any], *names: str) -> Any:
    """Case-insensitive multi-key getter for ASP.NET PascalCase vs camelCase."""
    lower = {k.lower(): v for k, v in d.items()}
    for n in names:
        if n.lower() in lower:
            return lower[n.lower()]
    return None


@mcp.tool()
async def optimize_report(report_id: str | None = None) -> dict[str, Any]:
    """
    Fetch reports, pick one (by report_id if given, else the first),
    analyze, and POST the optimization. Only calls GET once.
    """
    log.info("optimize_report: requested=%s", report_id)
    reports = await get_reports()
    if not reports:
        return {"error": "no reports returned by API"}

    if report_id:
        report = next(
            (r for r in reports
             if str(_get_field(r, "reportId", "ReportId")) == report_id),
            None,
        )
        if report is None:
            log.warning("  reportId %s not found, falling back to first", report_id)
            report = reports[0]
    else:
        report = reports[0]

    actual_id = str(_get_field(report, "reportId", "ReportId"))
    log.info("  using reportId=%s", actual_id)

    proposal = analyze_report(report)
    return await submit_optimization(
        report_id=actual_id,
        ai_recommendation=proposal["ai_recommendation"],
        optimization_strategy=proposal["optimization_strategy"],
        current_weight_kg=proposal["current_weight_kg"],
        projected_weight_kg=proposal["projected_weight_kg"],
        design_modifications=proposal["design_modifications"],
        parameter_suggestions=proposal["parameter_suggestions"],
        suggested_modifications=proposal["suggested_modifications"],
        results=proposal["results"],
    )


# ---------------------------------------------------------------------------
# Self-test (no MCP host needed - run from a terminal)
# ---------------------------------------------------------------------------
async def _selftest() -> None:
    log.info("=== SELFTEST START ===")
    result = await optimize_report()
    log.info("POST result: %s", result)
    log.info("=== SELFTEST DONE ===")


if __name__ == "__main__":
    if "--selftest" in sys.argv:
        import asyncio
        try:
            asyncio.run(_selftest())
        except Exception as e:
            log.exception("selftest crashed: %s", e)
            sys.exit(1)
    else:
        log.info("Starting MCP stdio loop. Waiting for host to connect...")
        log.info("(In a plain terminal nothing will happen - that's normal.")
        log.info(" Launch via Claude Desktop, or run: python server.py --selftest)")
        mcp.run()