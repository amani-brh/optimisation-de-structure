namespace AmaniRobot.Application.Boundaries.Optimisatizer;

public interface IOutputPort
{
    void Standard(CreateOptimizationOutput output);
    void NotFound(string message);
    void Error(string message);
}