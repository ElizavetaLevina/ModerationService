using ModerationService.DAL.Migrations;
using ModerationService.DAL.Data;

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
		public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			var connectionStringName = nameof(ConnectionProvider);
			var connectionString = configuration.GetConnectionString(connectionStringName)
				?? throw new InvalidOperationException($"Строка подключение к бд '{connectionStringName}' не найдена в  appsettings.json");

			services.AddScoped<ConnectionProvider>(_ => new ConnectionProvider(connectionString));
			services.AddTransient<DbMigrator>(option => new DbMigrator(connectionString, option.GetRequiredService<ILogger<DbMigrator>>()));

			Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
		}

		/// <summary>
		/// Применяет миграции БД при старте приложения
		/// </summary>
		public static void Configure(WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var migrator = scope.ServiceProvider.GetRequiredService<DbMigrator>();
			migrator.ApplyMigrations();
		}
	}
}
