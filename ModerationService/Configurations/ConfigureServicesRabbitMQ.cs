using MassTransit;
using ModerationService.BLL;

namespace ModerationService.Configurations
{
    public static class ConfigureServicesRabbitMQ
    {
        public static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration, bool isInDocker)
        {
            services.AddMassTransit(c =>
            {
                c.AddConsumer<PostSubmittedForModerationConsumer>();

                c.UsingRabbitMq((context, cfg) =>
                {
                    var host = isInDocker ? configuration["RabbitMQ:HostDocker"] : configuration["RabbitMQ:HostLocal"];
                    cfg.Host($"rabbitmq://{host}:{configuration["RabbitMQ:Port"]}", x =>
                    {
                        x.Username(configuration["RabbitMQ:UserName"]);
                        x.Password(configuration["RabbitMQ:Password"]);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
