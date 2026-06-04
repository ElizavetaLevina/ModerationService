using Shared.Contracts.Enum;

namespace ModerationService.Common.DTO
{
	/// <summary>
	/// DTO для передачи данных о результате модерации
	/// </summary>
	public class ModerationResultDTO
    {
		/// <summary>
		/// Уникальный идентификатор
		/// </summary>
		public int Id { get; set; }

        /// <summary>
        /// Идентификатор поста на модерации
        /// </summary>
        public int PostPendingId { get; set; }

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
