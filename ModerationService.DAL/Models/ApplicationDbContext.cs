using Npgsql;

namespace ModerationService.DAL.Models
{
	public class ApplicationDbContext(string connectionString)
	{
		private readonly string _connectionString = connectionString;

		public NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);
	}
}
