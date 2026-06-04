using ModerationService.BLL.Interfaces;
using ModerationService.BLL.Logics;
using ModerationService.DAL.Repositories;
using ModerationService.DAL.UnitOfWork;
using ModerationService.HostedServices;

namespace ModerationService.Configurations
{
	public static class ConfigureServicesLogic
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddHostedService<ModerationResultPublisherHostedService>();

			// Репозитории
			services.AddScoped<IModerationResultsRepository, ModerationResultsRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			// Бизнес-логика
			services.AddScoped<IModerationResultPublisherLogic, ModerationResultPublisherLogic>();
		}
	}
}
