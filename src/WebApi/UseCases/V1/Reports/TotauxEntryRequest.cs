namespace AmaniRobot.WebApi.UseCases.V1.Reports;

public sealed record TotauxEntryRequest(
    string Material,
    decimal TotalKg);
