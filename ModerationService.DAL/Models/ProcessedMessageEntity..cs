namespace ModerationService.DAL.Models
{
	 /// <summary>
    /// Обработанное сообщение
    /// </summary>
	public class ProcessedMessageEntity
	{
		/// <summary>
		/// Уникальный идентификатор
		/// </summary>
		public Guid MessageId { get; set; }

        /// <summary>
        /// Дата и время обработки
        /// </summary>
		public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
	}
}