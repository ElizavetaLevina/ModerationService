using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using ModerationService.DAL.Mappings;

namespace ModerationService.Configurations
{
	/// <summary>
	/// Конфигурация AutoMapper
	/// </summary>
	public static class ConfigureServicesAutoMapper
	{
		/// <summary>
		/// Регистрирует маппер с профилем AppMappingProfile
		/// </summary>
		/// <param name="services"></param>
		public static void ConfigureServices(IServiceCollection services)
		{
			var mapperConfig = new MapperConfiguration(c =>
			{
				c.AddProfile<AppMappingProfile>();
			}, NullLoggerFactory.Instance);

			mapperConfig.AssertConfigurationIsValid(); // Проверяем корректность маппингов

			services.AddSingleton(mapperConfig.CreateMapper());
		}
	}
}
