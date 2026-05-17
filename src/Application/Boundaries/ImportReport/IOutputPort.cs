namespace AmaniRobot.Application.Boundaries.ImportReport;

public interface IOutputPort
{
    void Standard(ImportReportOutput output);
    void Error(string message);
}