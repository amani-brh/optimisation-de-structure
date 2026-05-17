// Infrastructure/PersistenceLayer/MongoDb/Repositories/ReportRepository.cs
using AmaniRobot.Application.Repositories;
using AmaniRobot.Domain;
using AmaniRobot.Domain.Reports;
using AmaniRobot.Domain.ValueObjects;
using AmaniRobot.Infrastructure.PersistenceLayer.MongoDb.Documents;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb.Repositories;

public sealed class ReportRepository : IReportRepository
{
    private readonly IMongoContext _context;
    private readonly IMongoCollection<ReportDocument> _collection;
    private readonly IEntityFactory _entityFactory;

    public ReportRepository(IMongoContext context, IEntityFactory entityFactory)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
        _collection = _context.GetDocumentCollection<ReportDocument>("Reports");
    }

    public Task Add(IReport report)
    {
        var doc = ToDocument(report);
        _context.AddCommand(async () => await _collection.InsertOneAsync(doc));
        return Task.CompletedTask;
    }

    public async Task<IReport?> Get(Guid id)
    {
        var filter = Builders<ReportDocument>.Filter.Eq(r => r.Id, GuidToObjectIdString(id));
        var cursor = await _collection.FindAsync(filter);
        var doc = await cursor.FirstOrDefaultAsync();
        return doc is null ? null : ToDomain(doc);
    }

    public async Task<IReport?> GetByRunId(int runId)
    {
        var cursor = await _collection.FindAsync(r => r.RunId == runId);
        var doc = await cursor.FirstOrDefaultAsync();
        return doc is null ? null : ToDomain(doc);
    }

    public async Task<IEnumerable<IReport>> GetAll()
    {
        var cursor = await _collection.FindAsync(_ => true);
        var docs = await cursor.ToListAsync();
        return docs.Select(ToDomain);
    }

    public async Task Update(IReport report)
    {
        var doc = ToDocument(report);
        var filter = Builders<ReportDocument>.Filter.Eq(r => r.Id, GuidToObjectIdString(report.Id));
        await _collection.FindOneAndReplaceAsync(filter, doc);
    }

    // ── Mapping ───────────────────────────────────────────────────

    private static ReportDocument ToDocument(IReport report)
    {
        return new ReportDocument
        {
            Id = ObjectId.GenerateNewId().ToString(),
            RunId = report.RunId,
            Timestamp = report.Timestamp,
            ProjectFile = report.ProjectFile.Value,
            GrandTotalKg = report.GrandTotalKg.ToDecimal(),
            Totaux = report.Totaux.Select(t => new TotauxEntryDocument
            {
                Material = t.Material.Value,
                TotalKg = t.TotalKg.ToDecimal()
            }).ToList(),
            Rows = report.Rows.Select(r => new ReportRowDocument
            {
                Type = r.Type.Value,
                IsHeader = r.IsHeader,
                Nombre = r.Nombre,
                LengthM = r.LengthM,
                PoidsUnitaire = r.PoidsUnitaire,
                PoidsPiece = r.PoidsPiece,
                PoidsTotal = r.PoidsTotal?.ToDecimal()
            }).ToList()
        };
    }

    private IReport ToDomain(ReportDocument doc)
    {
        var totaux = new TotauxCollection();
        foreach (var t in doc.Totaux)
            totaux.Add(_entityFactory.NewTotauxEntry(
                new SectionType(t.Material),
                new Weight(t.TotalKg)));

        var rows = new RowsCollection();
        foreach (var r in doc.Rows)
            rows.Add(_entityFactory.NewReportRow(
                new SectionType(r.Type),
                r.IsHeader,
                r.Nombre,
                r.LengthM,
                r.PoidsUnitaire,
                r.PoidsPiece,
                r.PoidsTotal.HasValue ? new Weight(r.PoidsTotal.Value) : null));

        return _entityFactory.NewReport(
            doc.RunId,
            doc.Timestamp,
            new FilePath(doc.ProjectFile),
            new Weight(doc.GrandTotalKg),
            totaux,
            rows);
    }

    // ── Helpers ───────────────────────────────────────────────────

    private static Guid ConvertObjectIdToGuid(ObjectId objectId)
    {
        var bytes = new byte[16];
        Array.Copy(objectId.ToByteArray(), 0, bytes, 0, 12);
        return new Guid(bytes);
    }

    private static string GuidToObjectIdString(Guid guid)
    {
        var bytes = new byte[12];
        Array.Copy(guid.ToByteArray(), 0, bytes, 0, 12);
        return new ObjectId(bytes).ToString();
    }
}