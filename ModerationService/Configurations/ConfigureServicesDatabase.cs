using Microsoft.EntityFrameworkCore;
using ModerationService.DAL.Models;

namespace ModerationService.Configurations
{
	/// <summary>
	/// Конфигурация подключения к базе данных
	/// </summary>
	public static class ConfigureServicesDatabase
	{
		/// <summary>
		/// Регистрирует DbContext с PostgreSQL
		/// </summary>
		/// <param name="services">Коллекция сервисов</param>
		/// <param name="configuration">Конфигурация приложения</param>
		/// <param name="isInDocker">Флаг запуска в контейнере</param>
		public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, bool isInDocker)
		{
			var connectionStringName = isInDocker ? nameof(ApplicationDbContext) : $"{nameof(ApplicationDbContext)}_Local";

			services.AddDbContext<ApplicationDbContext>(option =>
			{
				option.UseNpgsql(configuration.GetConnectionString(connectionStringName));
				option.UseSnakeCaseNamingConvention();
			});
		}
	}
}
