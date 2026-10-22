using ModerationService.BLL.Interfaces;
using ModerationService.BLL.Logics;
using ModerationService.DAL.Repositories;
using ModerationService.DAL.UnitOfWork;
using ModerationService.HostedServices;
using ModerationService.Options;

namespace ModerationService.Configurations
{
    /// <summary>
    /// Конфигурация бизнес-логики, репозиториев и внешних сервисов
    /// </summary>
	public static class ConfigureServicesLogic
	{
        /// <summary>
        /// Регистрирует репозитории, логику и внешние сервисы
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="configuration">Конфигурация приложения</param>
		public static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
		{
			services.AddHostedService<ModerationResultPublisherService>();

			// Репозитории
			services.AddScoped<IModerationResultsRepository, ModerationResultsRepository>();
			services.AddScoped<IProcessedMessageRepository, ProcessedMessageRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			// Бизнес-логика
			services.AddScoped<IModerationResultPublisherLogic, ModerationResultPublisherLogic>();
			services.AddScoped<IModerationLogic, ModerationLogic>();
			services.AddScoped<IProcessedMessageLogic, ProcessedMessageLogic>();
            
            // Options
            services.Configure<ModerationResultPublisherOptions>(configuration.GetSection(ModerationResultPublisherOptions.SectionName));
		}
	}
}
