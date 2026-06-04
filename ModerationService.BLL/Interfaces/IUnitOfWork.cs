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
		/// Сохраняет все изменения в рамках одной транзакции
		/// </summary>
		/// <param name="token">Токен отмены</param>
		Task SaveChangesAsync(CancellationToken token = default);
	}
}
