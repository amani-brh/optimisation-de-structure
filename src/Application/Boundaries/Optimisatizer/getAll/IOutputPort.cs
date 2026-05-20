namespace AmaniRobot.Application.Boundaries.Optimisatizer.getAll;

public interface IOutputPort
{
    void Standard(GetAllOptimizationsOutput output);
    void NotFound(string message);
    void Error(string message);
}