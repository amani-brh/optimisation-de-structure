namespace AmaniRobot.Application.Boundaries.ImportReport.GetReport;

public interface IOutputPort
{
    void Standard(GetReportOutput output);
    void NotFound(Guid id);
    void Error(string message);
}

