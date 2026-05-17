namespace AmaniRobot.Application.Boundaries.ImportReport.GetAllReports;

public interface IOutputPort
{
    void Standard(GetAllReportsOutput output);
    void Error(string message);
}