using MassTransit;
using ModerationService.BLL.Consumers;

namespace ModerationService.Configurations
{
    /// <summary>
    /// Конфигурация подключения к RabbitMQ
    /// </summary>
    public static class ConfigureServicesRabbitMQ
    {
		/// <summary>
		/// Регистрирует клиента RabbitMQ для обмена сообщениями
		/// </summary>
		/// <param name="services">Коллекция сервисов</param>
		/// <param name="configuration">Конфигурация приложения</param>
		/// <param name="isInDocker">Флаг запуска в контейнере</param>
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
