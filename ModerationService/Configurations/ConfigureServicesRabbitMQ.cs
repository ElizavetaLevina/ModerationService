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
		public static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
		{
			services.AddMassTransit(c =>
			{
				c.AddConsumer<PostSubmittedForModerationConsumer, PostSubmittedForModerationConsumerDefinition>();

				c.UsingRabbitMq((context, cfg) =>
				{
					cfg.Host($"rabbitmq://{configuration["RabbitMQ:Host"]}:{configuration["RabbitMQ:Port"]}", x =>
					{
						x.Username(configuration["RabbitMQ:UserName"]);
						x.Password(configuration["RabbitMQ:Password"]);
					});
                    
					cfg.ConfigureEndpoints(context);
				});
			})
			.AddMassTransitHostedService();
		}
	}
}
