using DbUp;
using Microsoft.Extensions.Logging;

namespace ModerationService.DAL.Migrations
{
	public class DbMigrator(string connectionString, ILogger<DbMigrator> logger)
	{
		private readonly string _connectionString = connectionString;
		private readonly ILogger<DbMigrator> _logger = logger;

		public void ApplyMigrations()
		{
			var upgrader = DeployChanges.To
			.PostgresqlDatabase(_connectionString)
			.WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts"))
			.WithTransaction()
			.LogTo(_logger)
			.Build();

			var result = upgrader.PerformUpgrade();

			if (result.Successful)
			{
				_logger.LogInformation("Миграции успешно применены");
			}
			else
			{
				_logger.LogError(result.Error, "Ошибка миграции");
				throw new Exception($"Ошибка миграции: {result.Error.Message}", result.Error);
			}
		}
	}
}
