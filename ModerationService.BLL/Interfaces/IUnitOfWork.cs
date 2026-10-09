using System.Data;

namespace ModerationService.BLL.Interfaces
{
	/// <summary>
	/// Unit of Work для управления транзакциями и координации работы репозиториев
	/// </summary>
	public interface IUnitOfWork : IDisposable
	{
		/// <summary>
		/// Репозиторий результатов модерации
		/// </summary>
		IModerationResultsRepository ModerationResultsRepository { get; }

		/// <summary>
		/// Транзакция
		/// </summary>
		IDbTransaction? Transaction { get; }

		/// <summary>
		/// Начинает транзакцию
		/// </summary>
		/// <param name="token">Токен отмены</param>
		Task BeginTransactionAsync(CancellationToken token = default);

		/// <summary>
		/// Сохраняет все изменения в рамках одной транзакции
		/// </summary>
		/// <param name="token">Токен отмены</param>
		Task SaveChangesAsync(CancellationToken token = default);

		/// <summary>
		/// Откатывает транзакцию
		/// </summary>
		/// <param name="token">Токен отмены</param>
		Task RollbackAsync(CancellationToken token = default);
	}
}
