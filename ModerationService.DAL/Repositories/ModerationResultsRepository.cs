using Dapper;
using ModerationService.BLL.Interfaces;
using ModerationService.Common.DTO;
using ModerationService.DAL.Models;
using Shared.Contracts.Enum;

namespace ModerationService.DAL.Repositories
{
	/// <summary>
	/// Реализация репозитория для работы с результатами модерации
	/// </summary>
	/// <param name="dbContext">Контекст базы данных</param>
	/// <param name="unitOfWork">Unit of Work для предоставления соедниенения с бд и управления транзакциями</param>
	public class ModerationResultsRepository(ApplicationDbContext dbContext, IUnitOfWork unitOfWork) : IModerationResultsRepository
	{
		private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private const int BatchSize = 50;

		public async Task<List<ModerationResultDTO>> GetModerationResultsBatch(CancellationToken token = default)
		{
			const string sql = @"
				select 
					id, 
					post_pending_id, 
					status, 
					rejection_reason, 
					date_moderate 
				from moderation_results
				where status != @failed
				limit @batch_size";

			using var connection = _dbContext.CreateConnection();

			return (await connection.QueryAsync<ModerationResultDTO>(new CommandDefinition(
				sql, 
				new { failed = (int)StatusModerationEnum.Failed, batch_size = BatchSize}, 
				cancellationToken: token))).AsList();
		}

		public async Task SaveModerationResult(ModerationResultDTO postSubmitted, CancellationToken token = default)
		{
			var connection = _unitOfWork.Connection ?? throw new InvalidOperationException("Соединение не инициализировано");
			const string sql = @"
				insert into moderation_results (post_pending_id, status, rejection_reason) 
				values (@post_pending_id, @status, @rejection_reason)";

			await connection.ExecuteAsync(new CommandDefinition(
				sql, 
				new
				{
					post_pending_id = postSubmitted.PostPendingId, 
					status = postSubmitted.Status, 
					rejection_reason = postSubmitted.RejectionReason
				}, 
                transaction: _unitOfWork.Transaction,
				cancellationToken: token));
		}

		public async Task<IReadOnlyCollection<int>> DeleteModerationResults(IReadOnlyCollection<int> moderationResultIds,  CancellationToken token = default)
		{
			var connection = _unitOfWork.Connection ?? throw new InvalidOperationException("Соединение не инициализировано");
			const string sql = @"
				delete from moderation_results
				where id in @id
				returning id";

			return (await connection.QueryAsync<int>(new CommandDefinition(
				sql, 
				new { id = moderationResultIds }, 
                transaction: _unitOfWork.Transaction,
				cancellationToken: token))).AsList();
		}

		public async Task IncrementRetryCount(int moderationResultId, int maxRetries = 5, CancellationToken token = default)
		{
			var connection = _unitOfWork.Connection ?? throw new InvalidOperationException("Соединение не инициализировано");
            const string sql = @"
				update moderation_results 
				set 
					retry_count = retry_count + 1,
					status = case when retry_count + 1 >= @max_retries then @failed else status end
				where id = @id";

            await connection.ExecuteAsync(new CommandDefinition(
	            sql,
	            new { max_retries = maxRetries, failed = (int)StatusModerationEnum.Failed, id = moderationResultId },
	            transaction: _unitOfWork.Transaction,
	            cancellationToken: token));
		}
	}
}
