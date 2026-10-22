namespace ModerationService.Options
{
    /// <summary>
    /// Настройки фонового сервиса отправки сообщений в очередь после модерации
    /// </summary>
	public class ModerationResultPublisherOptions
	{
		/// <summary>
		/// Имя секции в appsettings.json
		/// </summary>
		public const string SectionName = "ModerationResultPublisher";
        
        /// <summary>
        /// Задержка перед следующей итерацией при пустой очереди или ошибке в секундах
        /// </summary>
        public int DelaySeconds { get; set; } = 5;
	}
}