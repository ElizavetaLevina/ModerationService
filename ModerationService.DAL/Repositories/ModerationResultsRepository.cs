using AutoMapper;
using Dapper;
using ModerationService.BLL.Interfaces;
using ModerationService.Common.DTO;
using ModerationService.DAL.Models;

namespace ModerationService.DAL.Repositories
{
	/// <summary>
	/// Реализация репозитория для работы с результатами модерации
	/// </summary>
	/// <param name="dbContext">Контекст базы данных</param>
	/// <param name="mapper">Автомаппер</param>
	public class ModerationResultsRepository(ApplicationDbContext dbContext, IMapper mapper) : IModerationResultsRepository
	{
		private readonly ApplicationDbContext _dbContext = dbContext;
		private readonly IMapper _mapper = mapper;

		public async Task<List<ModerationResultDTO>> GetModerationResults(CancellationToken token = default)
		{
			const string sql = @"
				select 
					id, 
					post_pending_id, 
					status, 
					rejection_reason, 
					date_moderate 
				from moderation_results";

			using var connection = _dbContext.CreateConnection();

			return (await connection.QueryAsync<ModerationResultDTO>(new CommandDefinition(sql, cancellationToken: token))).AsList();
		}

		public async Task SaveModerationResult(ModerationResultDTO postSubmitted, CancellationToken token = default)
		{
			const string sql = @"
				insert into moderation_results (post_pending_id, status, rejection_reason) 
				values (@post_pending_id, @status, @rejection_reason)";

			using var connection = _dbContext.CreateConnection();

			await connection.ExecuteAsync(new CommandDefinition(
				sql, 
				new { post_pending_id = postSubmitted.PostPendingId, status = postSubmitted.Status, rejection_reason = postSubmitted.RejectionReason }, 
				cancellationToken: token));
		}

		public async Task DeleteModerationResult(int moderationResultId, CancellationToken token = default)
		{
			const string sql = @"
				delete from moderation_results
				where id = @id";

			using var connection = _dbContext.CreateConnection();

			await connection.ExecuteAsync(new CommandDefinition(sql, new { id = moderationResultId }, cancellationToken: token));
		}
	}
}
