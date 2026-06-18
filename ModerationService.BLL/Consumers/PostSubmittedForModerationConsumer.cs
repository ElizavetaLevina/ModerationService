using MassTransit;
using ModerationService.BLL.Interfaces;
using ModerationService.Common.DTO;
using Shared.Contracts.DTO;
using Shared.Contracts.Enum;

namespace ModerationService.BLL.Consumers
{
	/// <summary>
	/// Потребитель (consumer) сообщений о посте, поданном на модерацию.
	/// Реализует логику автоматической модерации и сохранения результата в базу данных
	/// </summary>
	public class PostSubmittedForModerationConsumer(IModerationResultsRepository moderationResultsRepository, IModerationLogic moderationLogic) : IConsumer<PostSubmittedForModeration>
	{
		private readonly IModerationResultsRepository _moderationResultsRepository = moderationResultsRepository;
		private readonly IModerationLogic _moderationLogic = moderationLogic;

		/// <summary>
		/// Обрабатывает входящее сообщение о посте, отправленного на модерацию. 
		/// Выполняет проверку и сохраняет результат в БД
		/// </summary>
		/// <param name="context">Контекст сообщения, содержащий данные поста</param>
		/// <returns>Задача, представляющая асинхронную операцию</returns>
		public async Task Consume(ConsumeContext<PostSubmittedForModeration> context)
		{
			var message = context.Message;

			var isApproved = _moderationLogic.IsApproved(message.Title, message.TextPost);

			var moderationResult = new ModerationResultDTO
			{
				PostPendingId = message.Id,
				Status = isApproved ? StatusModerationEnum.Approved : StatusModerationEnum.Rejected,
				RejectionReason = isApproved ? null : "Пост содержит нецензурную лексику"
			};

			await _moderationResultsRepository.SaveModerationResult(moderationResult);
		}
	}
}
