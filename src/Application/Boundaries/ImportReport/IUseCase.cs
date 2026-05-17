namespace AmaniRobot.Application.Boundaries.ImportReport;

public interface IUseCase
{
    Task ExecuteAsync(ImportReportInput input);
}