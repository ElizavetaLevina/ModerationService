namespace ModerationService.Options
{
	/// <summary>
    ///  Настройки фоновой очистки таблицы обработанных сообщений
    /// </summary>
	public class ProcessedMessagesCleanupOptions
	{
        /// <summary>
        /// Имя секции в appsettings.json
        /// </summary>
        public const string SectionName = "ProcessedMessagesCleanup";

        /// <summary>
        /// Период хранения записей в днях
        /// </summary>
        public int RetentionDays { get; set; } = 7;

        /// <summary>
        /// Интервал между запусками очистки в часах
        /// </summary>
        public int IntervalHours { get; set; } = 1;
	}
}