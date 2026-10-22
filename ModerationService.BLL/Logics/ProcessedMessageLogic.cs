using ModerationService.BLL.Interfaces;

namespace ModerationService.BLL.Logics
{
	public class ProcessedMessageLogic(IProcessedMessageRepository messageRepository) : IProcessedMessageLogic
	{
		private readonly IProcessedMessageRepository _messageRepository = messageRepository;
        
		public async Task AddMessage(Guid messageId, CancellationToken token = default)
		{
			await _messageRepository.AddMessage(messageId, token);
		}

		public async Task<bool> HasMessage(Guid messageId, CancellationToken token = default)
		{
			return await _messageRepository.HasMessage(messageId, token);
		}

		public async Task DeleteMessages(DateTime deleteBefore, CancellationToken token = default)
		{
			_messageRepository.DeleteMessages(deleteBefore, token);
		}
	}
}