using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using ModerationService.BLL.Interfaces;
using Shared.Contracts.DTO;
using System.Collections.Concurrent;

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

		public async Task<int> PublishMessage(CancellationToken token = default)
		{
			var results = await _moderationResultsRepository.GetModerationResultsBatch(token);
			var publishedIds = new ConcurrentBag<int>();
			var failedIds = new ConcurrentBag<int>();
			var parallelOption = new ParallelOptions() { MaxDegreeOfParallelism = 10, CancellationToken = token };

			try
			{
				await Parallel.ForEachAsync(results, parallelOption, async (result, parallelToken) =>
				{
					try
					{
						await _publishEndpoint.Publish(_mapper.Map<PostModeratedEvent>(result), parallelToken);
						publishedIds.Add(result.Id);
					}
					catch (Exception ex)
					{
						failedIds.Add(result.Id);
						_logger.LogError(ex, "Не удалось отправить результат модерации {ResultId}", result.Id);
					}
				});

                await _unitOfWork.BeginTransactionAsync(token);
                
				foreach (var resultId in failedIds)
				{
					await _moderationResultsRepository.IncrementRetryCount(resultId, token: token);
				}

				if (!publishedIds.IsEmpty)
				{
					var deletedIds = await _moderationResultsRepository.DeleteModerationResults(publishedIds, token);
					foreach (var resultId in publishedIds)
					{
						if (!deletedIds.Contains(resultId))
							_logger.LogWarning("Результат модерации {ResultId} не найден при удалении", resultId);
					}
				}

				await _unitOfWork.SaveChangesAsync(token);
			}
			catch
			{
				await _unitOfWork.RollbackAsync(token);
				throw;
			}

			return publishedIds.Count;
		}
	}
}
