using BogaNet.BWF;
using MassTransit;
using Shared.Contracts.DTO;
using Shared.Contracts.Enum;

namespace ModerationService.BLL
{
    /// <summary>
    /// Потребитель (consumer) сообщений о посте, поданном на модерацию.
    /// Реализует логику автоматической модерации и публикации результата
    /// </summary>
    public class PostSubmittedForModerationConsumer() : IConsumer<PostSubmittedForModeration>
    {
        /// <summary>
        /// Обрабатывает входящее сообщение о посте, отправленного на модерацию
        /// </summary>
        /// <param name="context">Контекст сообщения, содержащий данные поста</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>
        public async Task Consume(ConsumeContext<PostSubmittedForModeration> context)
        {
            var message = context.Message;

            var isApproved = Moderate(message);

            // Публикует событие 'PostModeratedEvent' с результатом модерации
            await context.Publish(new PostModeratedEvent
            {
                PendingId = message.Id,
                Status = isApproved ? StatusModerationEnum.Approved : StatusModerationEnum.Rejected,
                RejectionReason = isApproved ? null : "Пост содержит нецензурную лексику"
            });
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
