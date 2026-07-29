using ModerationService.Common.DTO;
using ModerationService.DAL.Migrations;
using ModerationService.DAL.Models;
using System.Reflection;

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
			var connectionString = configuration.GetConnectionString(connectionStringName)
				?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found in appsettings.json");

			services.AddScoped<ApplicationDbContext>(option => new ApplicationDbContext(connectionString));
			services.AddTransient<DbMigrator>(option => new DbMigrator(connectionString, option.GetRequiredService<ILogger<DbMigrator>>()));

			Dapper.SqlMapper.SetTypeMap(typeof(ModerationResultDTO), new Dapper.CustomPropertyTypeMap(typeof(ModerationResultDTO), (type, columnName) =>
			{
				var prop = type.GetProperties().FirstOrDefault(p =>
					p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>()?.Name == columnName);

				return prop ?? throw new InvalidOperationException($"Не найдено свойство для колонки {columnName}");
			}));
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
