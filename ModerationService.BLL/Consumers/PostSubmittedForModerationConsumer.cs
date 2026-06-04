using BogaNet.BWF;
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
	public class PostSubmittedForModerationConsumer(IModerationResultsRepository moderationResultsRepository) : IConsumer<PostSubmittedForModeration>
	{
		private readonly IModerationResultsRepository _moderationResultsRepository = moderationResultsRepository;

		/// <summary>
		/// Обрабатывает входящее сообщение о посте, отправленного на модерацию. 
		/// Выполняет проверку и сохраняет результат в БД
		/// </summary>
		/// <param name="context">Контекст сообщения, содержащий данные поста</param>
		/// <returns>Задача, представляющая асинхронную операцию</returns>
		public async Task Consume(ConsumeContext<PostSubmittedForModeration> context)
		{
			var message = context.Message;

			var isApproved = Moderate(message);

			var moderationResult = new ModerationResultDTO
			{
				PostPendingId = message.Id,
				Status = isApproved ? StatusModerationEnum.Approved : StatusModerationEnum.Rejected,
				RejectionReason = isApproved ? null : "Пост содержит нецензурную лексику"
			};

			await _moderationResultsRepository.SaveModerationResult(moderationResult);
		}

		/// <summary>
		/// Проверяет, содержит ли пост нецензурную лексику
		/// </summary>
		/// <param name="post">Пост</param>
		/// <returns>Результат проверки</returns>
		private bool Moderate(PostSubmittedForModeration post)
		{
			var fullText = $"{post.Title} {post.TextPost}";
			return !Pacifier.Instance.Contains(fullText);
		}
	}
}
