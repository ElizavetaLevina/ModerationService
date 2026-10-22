namespace ModerationService.Options
{
	/// <summary>
    /// Настройки фонового сервиса проверки очереди _error
    /// </summary>
	public class ErrorQueueMonitorOptions
	{
		/// <summary>
		/// Имя секции в appsettings.json
		/// </summary>
		public const string SectionName = "ErrorQueueMonitor";

        /// <summary>
        /// Имя очереди
        /// </summary>
		public string QueueName { get; set; } = string.Empty;

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = string.Empty;    
        
		/// <summary>
		/// URL RabbitMQ Management API
		/// </summary>
		public string ManagementApiUrl { get; set; } = "http://localhost:15672";

        /// <summary>
        /// Интервал между запусками проверки в минутах
        /// </summary>
		public int IntervalMinutes { get; set; } = 5;
	}
}