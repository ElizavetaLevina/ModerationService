using MassTransit;
using Microsoft.Extensions.Logging;
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
    public class PostSubmittedForModerationConsumer(
        IModerationResultsRepository moderationResultsRepository,
        IModerationLogic moderationLogic,
        IUnitOfWork unitOfWork,
        IProcessedMessageLogic messageLogic,
        ILogger<PostSubmittedForModerationConsumer> logger
    ) : IConsumer<PostSubmittedForModeration>
    {
        private readonly IModerationResultsRepository _moderationResultsRepository = moderationResultsRepository;
        private readonly IModerationLogic _moderationLogic = moderationLogic;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IProcessedMessageLogic _messageLogic = messageLogic;
        private readonly ILogger<PostSubmittedForModerationConsumer> _logger = logger;		

        /// <summary>
        /// Обрабатывает входящее сообщение о посте, отправленного на модерацию.
        /// Проверяет обработано ли сообщение с указанным MessageId, выполняет модерацию и сохраняет результат в БД
        /// </summary>
        /// <param name="context">Контекст сообщения, содержащий данные поста</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>
        public async Task Consume(ConsumeContext<PostSubmittedForModeration> context)
        {
            PostSubmittedForModeration message = context.Message ?? throw new InvalidOperationException("ConsumeContext.Message is null");

            if (context.MessageId == null)
            {
	            _logger.LogWarning("Пришло сообщение без MessageId, пропущено");
	            return;
            }

            if (await _messageLogic.HasMessage(context.MessageId.Value, context.CancellationToken))
            {
                _logger.LogInformation("Сообщение с MessageId: {MessageId} уже было обработано", context.MessageId);
                return;
            }
            
            var isApproved = _moderationLogic.IsApproved(message.Title, message.TextPost);

            var moderationResult = new ModerationResultDTO
            {
                PostPendingId = message.Id,
                Status = isApproved ? StatusModerationEnum.Approved : StatusModerationEnum.Rejected,
                RejectionReason = isApproved ? null : "Пост содержит нецензурную лексику",
            };

            await _unitOfWork.BeginTransactionAsync(context.CancellationToken);

            try
            {
                await _moderationResultsRepository.SaveModerationResult(moderationResult);
                await _messageLogic.AddMessage(context.MessageId.Value);
	            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackAsync(context.CancellationToken);
                throw;
            }
        }
    }
}
