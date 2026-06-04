using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using ModerationService.BLL.Interfaces;
using Shared.Contracts.DTO;

namespace ModerationService.BLL.Logics
{
	/// <summary>
	/// Сервис для публикации результатов модерации
	/// </summary>
	/// <param name="moderationResultsRepository">Репозиторий результатов модерации</param>
	/// <param name="publishEndpoint">Endpoint для публикации сообщений в RabbitMQ</param>
	/// <param name="unitOfWork">Unit of Work для управления транзакциями</param>
	/// <param name="logger">Логгер</param>
	/// <param name="mapper">Автомаппер</param>
	public class ModerationResultPublisherLogic(IModerationResultsRepository moderationResultsRepository, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork, ILogger<IModerationResultPublisherLogic> logger, IMapper mapper) : IModerationResultPublisherLogic
	{
		private readonly IModerationResultsRepository _moderationResultsRepository = moderationResultsRepository;
		private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly ILogger<IModerationResultPublisherLogic> _logger = logger;
		private readonly IMapper _mapper = mapper;

		public async Task PublishMessage(CancellationToken token = default)
		{
			var results = await _moderationResultsRepository.GetModerationResults(token);

			foreach (var result in results)
			{
				try
				{
					await _publishEndpoint.Publish(_mapper.Map<PostModeratedEvent>(result), token);
					await _moderationResultsRepository.DeleteModerationResult(result.Id, token);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Не удалось отправить на модерацию пост {PostId}", result.Id);
				}
			}

			await _unitOfWork.SaveChangesAsync(token);
		}
	}
}
