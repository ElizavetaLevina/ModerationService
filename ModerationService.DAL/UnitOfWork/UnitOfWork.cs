using ModerationService.BLL.Interfaces;
using ModerationService.DAL.Models;

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

		public IModerationResultsRepository ModerationResultsRepository => moderationResultsRepository;

		public async Task SaveChangesAsync(CancellationToken token = default)
		{
			await _dbContext.SaveChangesAsync(token);
		}
		public void Dispose() => _dbContext.Dispose();
	}
}
