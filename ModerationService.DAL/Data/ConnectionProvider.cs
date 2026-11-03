using Npgsql;

namespace ModerationService.DAL.Data
{
	public class ConnectionProvider(string connectionString)
	{
		private readonly string _connectionString = connectionString;

		public NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);
	}
}
