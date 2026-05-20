using AmaniRobot.Application.Repositories;
using AmaniRobot.Application.Boundaries.Optimisatizer.getAll; 
namespace AmaniRobot.Application.UseCases;

public sealed class GetAllOptimizations(
    IOutputPort outputPort,
    IOptimizationRepository optimizationRepository)
{
    public async Task ExecuteAsync()
    {
        try
        {
            var optimizations = await optimizationRepository.GetAll();

            if (optimizations is null || optimizations.Count == 0)
            {
                outputPort.NotFound("No optimizations found");
                return;
            }

            outputPort.Standard(new GetAllOptimizationsOutput(optimizations));
        }
        catch (Exception ex)
        {
            outputPort.Error($"An error occurred while retrieving optimizations: {ex.Message}");
        }
    }
}