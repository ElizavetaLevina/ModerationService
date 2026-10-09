using ModerationService.BLL.Interfaces;
using ModerationService.DAL.Models;
using Npgsql;
using System.Data;
namespace ModerationService.DAL.UnitOfWork
{
	/// <summary>
	/// Реализация Unit of Work для управления транзакциями
	/// </summary>
	/// <param name="dbContext">Контекст базы данных</param>
	/// <param name="moderationResultsRepository">Репозиторий результатов модерации</param>
	public class UnitOfWork(ApplicationDbContext dbContext, IModerationResultsRepository moderationResultsRepository) : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext = dbContext;
		private readonly IModerationResultsRepository _moderationResultsRepository = moderationResultsRepository;
		private NpgsqlConnection? _connection;
		private NpgsqlTransaction? _transaction;

		public IModerationResultsRepository ModerationResultsRepository => _moderationResultsRepository;

		public IDbTransaction? Transaction => _transaction;

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
