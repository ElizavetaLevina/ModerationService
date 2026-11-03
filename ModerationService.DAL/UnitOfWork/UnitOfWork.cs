using ModerationService.BLL.Interfaces;
using ModerationService.DAL.Data;
using Npgsql;
using System.Data;
namespace ModerationService.DAL.UnitOfWork
{
	/// <summary>
	/// Реализация Unit of Work для управления транзакциями
	/// </summary>
	/// <param name="dbContext">Контекст базы данных</param>
	public sealed class UnitOfWork(ConnectionProvider dbContext) : IUnitOfWork
	{
		private readonly ConnectionProvider _dbContext = dbContext;
		private NpgsqlConnection? _connection;
		private NpgsqlTransaction? _transaction;

		public IDbTransaction? Transaction => _transaction;

		public IDbConnection? Connection => _connection;

		public async Task BeginTransactionAsync(CancellationToken token = default)
		{
			_connection = _dbContext.CreateConnection();
			await _connection.OpenAsync(token);
			_transaction = await _connection.BeginTransactionAsync(token);
		}

		public async Task SaveChangesAsync(CancellationToken token = default)
		{
			if (_transaction == null)
				throw new InvalidOperationException("Transaction is not started");

			await _transaction.CommitAsync(token);
		}

		public async Task RollbackAsync(CancellationToken token = default)
		{
			if (_transaction == null)
				throw new InvalidOperationException("Transaction is not started");

			await _transaction.RollbackAsync(token);
		}

		public void Dispose()
		{
			_connection?.Dispose();
			_transaction?.Dispose();
		}
	}
}
