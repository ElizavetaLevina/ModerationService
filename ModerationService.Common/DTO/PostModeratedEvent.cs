using ModerationService.Common.Enum;

namespace ModerationService.Common.DTO
{
    public class PostModeratedEvent
    {
        /// <summary>
        /// Идентификатор поста на модерации
        /// </summary>
        public int PendingId { get; set; }

        /// <summary>
        /// Статус модерации
        /// </summary>
        public StatusModerationEnum Status { get; set; }

        /// <summary>
        /// Причина отклонения
        /// </summary>
        public string? RejectionReason { get; set; } = null;

        /// <summary>
        /// Дата завершения модерации
        /// </summary>
        public DateTime DateModerate { get; set; } = DateTime.Now;
    }
}
