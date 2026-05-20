using AmaniRobot.Domain.Reports;

namespace AmaniRobot.Application.Repositories;

public interface IReportRepository
{
    Task Add(IReport report);
    Task<IReport?> Get(Guid id);
    Task<IReport?> GetByRunId(int runId);
    Task<IEnumerable<IReport>> GetAll();
    Task Update(IReport report);
}
