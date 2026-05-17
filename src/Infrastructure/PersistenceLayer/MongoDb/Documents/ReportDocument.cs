using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb.Documents;


public sealed class ReportDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("run_id")]
    public int RunId { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }

    [BsonElement("project_file")]
    public string ProjectFile { get; set; } = string.Empty;

    [BsonElement("grand_total_kg")]
    public decimal GrandTotalKg { get; set; }

    [BsonElement("totaux")]
    public List<TotauxEntryDocument> Totaux { get; set; } = [];

    [BsonElement("rows")]
    public List<ReportRowDocument> Rows { get; set; } = [];
}

public sealed class TotauxEntryDocument
{
    [BsonElement("material")]
    public string Material { get; set; } = string.Empty;

    [BsonElement("total_kg")]
    public decimal TotalKg { get; set; }
}

public sealed class ReportRowDocument
{
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("is_header")]
    public bool IsHeader { get; set; }

    [BsonElement("nombre")]
    public int? Nombre { get; set; }

    [BsonElement("length_m")]
    public double? LengthM { get; set; }

    [BsonElement("poids_unitaire")]
    public double? PoidsUnitaire { get; set; }

    [BsonElement("poids_piece")]
    public double? PoidsPiece { get; set; }

    [BsonElement("poids_total")]
    public decimal? PoidsTotal { get; set; }
}