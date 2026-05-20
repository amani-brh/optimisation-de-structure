using System.ComponentModel.DataAnnotations;
using AmaniRobot.Application.Boundaries.ImportReport;
using AmaniRobot.Application.Boundaries.ImportReport.GetAllReports;
using AmaniRobot.Application.Boundaries.ImportReport.GetReport;
using AmaniRobot.Application.Boundaries.Optimisatizer;
using AmaniRobot.Application.UseCases;
using AmaniRobot.Domain.Reports;
using AmaniRobot.WebApi.UseCases.V1.Optimisation;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static AmaniRobot.Application.UseCases.CloseAccount;

namespace AmaniRobot.WebApi.UseCases.V1.Reports;

[ApiVersion("1.0")]
[Route("api/v1/[controller]")]
[ApiController]
public sealed class ReportsController(
    ImportReport importReportUseCase,
    ImportReportPresenter importPresenter,
    GetReport getReportUseCase,
    GetReportPresenter getPresenter,
    GetAllReports getAllReportsUseCase,
    GetAllReportsPresenter getAllReportsPresenter,
    CreateOptimization createOptimizationUseCase,
    GetAllOptimizations getAllOptimizationsUseCase,
    GetAllOptimizationsPresenter getAllOptimizationsPresenter,
    CreateOptimizationPresenter createOptimizationPresenter)
    : ControllerBase
{
    private readonly ImportReport _importReportUseCase = importReportUseCase ?? throw new ArgumentNullException(nameof(importReportUseCase));
    private readonly ImportReportPresenter _importPresenter = importPresenter ?? throw new ArgumentNullException(nameof(importPresenter));
    private readonly GetReport _getReportUseCase = getReportUseCase ?? throw new ArgumentNullException(nameof(getReportUseCase));
    private readonly GetReportPresenter _getPresenter = getPresenter ?? throw new ArgumentNullException(nameof(getPresenter));
    private readonly GetAllReports _getAllReportsUseCase = getAllReportsUseCase ?? throw new ArgumentNullException(nameof(getAllReportsUseCase));
    private readonly GetAllReportsPresenter _getAllReportsPresenter = getAllReportsPresenter ?? throw new ArgumentNullException(nameof(getAllReportsPresenter));
    private readonly GetAllOptimizations _getAllOptimizationsUseCase = getAllOptimizationsUseCase ?? throw new ArgumentNullException(nameof(getAllOptimizationsUseCase));
    private readonly GetAllOptimizationsPresenter _getAllOptimizationsPresenter = getAllOptimizationsPresenter ?? throw new ArgumentNullException(nameof(getAllOptimizationsPresenter));
    // ADD these two
    private readonly CreateOptimization _createOptimizationUseCase = createOptimizationUseCase ?? throw new ArgumentNullException(nameof(createOptimizationUseCase));
    private readonly AmaniRobot.WebApi.UseCases.V1.Optimisation.CreateOptimizationPresenter _createOptimizationPresenter = createOptimizationPresenter ?? throw new ArgumentNullException(nameof(createOptimizationPresenter));

    /// <summary>Import a robot structural analysis report.</summary>
    /// <response code="201">Report imported successfully.</response>
    /// <response code="400">Bad request or duplicate run_id.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImportReportResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> Post([FromBody][Required] ImportReportRequest request)
    {
        var input = new ImportReportInput(
            request.RunId,
            request.Timestamp,
            request.ProjectFile,
            request.GrandTotalKg,
            request.Totaux
                .Select(t => new TotauxEntryInput(t.Material, t.TotalKg))
                .ToList(),
            request.Rows
                .Select(r => new ReportRowInput(
                    r.Type,
                    r.IsHeader,
                    r.Nombre,
                    r.LengthM,
                    r.PoidsUnitaire,
                    r.PoidsPiece,
                    r.PoidsTotal))
                .ToList());

        await _importReportUseCase.ExecuteAsync(input);
        return _importPresenter.ViewModel;
    }

    /// <summary>Get all reports.</summary>
    /// <response code="200">Reports retrieved successfully.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet(Name = "GetAllReports")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ImportReportResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> GetAll()
    {
        await _getAllReportsUseCase.ExecuteAsync();
        return _getAllReportsPresenter.ViewModel;
    }

    /// <summary>Get a report by its id.</summary>
    /// <response code="200">Report found.</response>
    /// <response code="404">Report not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("{reportId:guid}", Name = "GetReport")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImportReportResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> Get([FromRoute] Guid reportId)
    {
        await _getReportUseCase.ExecuteAsync(new GetReportInput(reportId));
        return _getPresenter.ViewModel;
    }

    [HttpPost("{reportId:guid}/optimizations")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateOptimization(
        [FromRoute] Guid reportId,
        [FromBody, Required] CreateOptimizationRequest request)
    {
        // Translate flat MCP payload -> rich domain objects
        var parameterDict = request.ParameterSuggestions
            .Select((s, i) => new { Key = $"suggestion_{i + 1}", Value = s })
            .ToDictionary(x => x.Key, x => x.Value);

        var analysis = new OptimizationAnalysis(
            request.AIRecommendation,
            request.OptimizationStrategy,
            request.CurrentWeightKg,
            request.ProjectedWeightKg,
            request.DesignModifications,
            parameterDict);

        var sectionMods = request.SuggestedModifications
            .Select(s => new SectionModification
            {
                SectionType = s,
                CurrentProfile = "",
                RecommendedProfile = "",
                Quantity = 0,
                CurrentWeightPerPiece = 0,
                RecommendedWeightPerPiece = 0,
                TotalWeightSavings = 0,
                Justification = s,
            })
            .ToList();

        var results = new OptimizationResults
        {
            TotalWeightSavings = request.CurrentWeightKg - request.ProjectedWeightKg,
            EstimatedCostImpact = 0,
            NumberOfSectionsModified = request.Results?.Count ?? 0,
            FeasibilityAssessment = "Pending engineering review",
            ImplementationNotes = request.Results?
                .Select(r => $"{r.Parameter}: {r.Before} -> {r.After}")
                .ToList() ?? new List<string>(),
        };

        var input = new CreateOptimizationInput(reportId, analysis, sectionMods, results);

        await _createOptimizationUseCase.ExecuteAsync(input);
        return _createOptimizationPresenter.ViewModel;
    }


    [HttpGet("optimizations", Name = "GetAllOptimizations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult?> GetAllOptimizations()
    {
        await _getAllOptimizationsUseCase.ExecuteAsync();
        return _getAllOptimizationsPresenter.ViewModel;
    }

    //[HttpGet("optimizations/{optimizationId:guid}", Name = "GetOptimization")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //public async Task<IActionResult?> GetOptimization([FromRoute] Guid optimizationId)
    //{
    //    await _getOptimizationUseCase.ExecuteAsync(
    //        new Application.Boundaries.Optimisatizer.GetById.GetOptimizationInput(optimizationId));
    //    return _getOptimizationPresenter.ViewModel;
    //}
}
