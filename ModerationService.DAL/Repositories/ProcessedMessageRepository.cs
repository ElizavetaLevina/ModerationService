using Dapper;
using ModerationService.BLL.Interfaces;
using ModerationService.DAL.Data;

namespace ModerationService.DAL.Repositories
{
	public class ProcessedMessageRepository(ConnectionProvider dbContext, IUnitOfWork unitOfWork) : IProcessedMessageRepository
	{
        private readonly ConnectionProvider _dbContext = dbContext;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        
		public async Task AddMessage(Guid messageId, CancellationToken token = default)
		{
			var connection = _unitOfWork.Connection ?? throw new InvalidOperationException("Соединение не инициализировано");
			const string sql = @"
				insert into processed_messages 
				values(@message_id, @processed_at)";
            
			await connection.ExecuteAsync(new CommandDefinition(sql,
				new { message_id = messageId, processed_at = DateTime.UtcNow }, transaction: _unitOfWork.Transaction, cancellationToken: token));
		}

		public async Task<bool> HasMessage(Guid messageId, CancellationToken token = default)
		{
			const string sql = @"
				select exists(
					select 1 from processed_messages
					where message_id = @message_id)";

			using var connection = _dbContext.CreateConnection();
            
			return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { message_id = messageId },
				cancellationToken: token));
		}

		public async Task DeleteMessages(DateTime deleteBefore, CancellationToken token = default)
		{
			const string sql = @"
				delete from processed_messages
				where processed_at < @delete_before";

			using var connection = _dbContext.CreateConnection();
			await connection.ExecuteAsync(new CommandDefinition(sql, new { delete_before = deleteBefore }, cancellationToken: token));
		}
	}
}