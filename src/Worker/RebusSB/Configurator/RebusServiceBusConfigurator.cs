using AmaniRobot.Infrastructure.RebusSB;
using AmaniRobot.Worker.RebusSB.HostedServices;

namespace AmaniRobot.Worker.Configurator;

public class RebusServiceBusConfigurator
{
    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<RebusBusSettings>(context.Configuration.GetSection(RebusBusSettings.Position));
        services.AddHostedService<RebusService>();
    }
}
