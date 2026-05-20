using AmaniRobot.Domain.Reports;

namespace AmaniRobot.Application.Repositories;

public interface IOptimizationRepository
{
    Task Add(Optimization optimization);
    Task<Optimization?> Get(Guid id);
    Task<List<Optimization>> GetAll();
    Task<List<Optimization>> GetByReportId(Guid reportId);
    Task Update(Optimization optimization);
}