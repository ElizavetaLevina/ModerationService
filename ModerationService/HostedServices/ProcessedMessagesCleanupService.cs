using Microsoft.Extensions.Options;
using ModerationService.BLL.Interfaces;
using ModerationService.Options;

namespace ModerationService.HostedServices
{
	/// <summary>
    /// Фоновый сервис очистки таблицы обработанных сообщений.
    /// Запускается с интервалом IntervalHours часов, создаёт scope для получения Scoped-сервисов
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов для создания scope</param>
    /// <param name="options">Настройки очистки</param>
    /// <param name="logger">Логгер</param>
	public class ProcessedMessagesCleanupService(IServiceProvider serviceProvider, IOptions<ProcessedMessagesCleanupOptions> options, ILogger<ProcessedMessagesCleanupService> logger) : BackgroundService
	{
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly IOptions<ProcessedMessagesCleanupOptions> _options = options;
		private readonly ILogger<ProcessedMessagesCleanupService> _logger = logger;
        
		/// <summary>
		/// Основной цикл фонового процесса.
		/// Каждые IntervalHours часов находит записи в таблице, хранящиеся больше RetentionDays дней и удаляет их.
		/// </summary>
		/// <param name="token">Токен отмены</param>
		protected override async Task ExecuteAsync(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
                try
				{
					using var scope = _serviceProvider.CreateScope();
					var logic = scope.ServiceProvider.GetRequiredService<IProcessedMessageLogic>();
					var deleteBefore = DateTime.UtcNow.AddDays(-_options.Value.RetentionDays);

					await logic.DeleteMessages(deleteBefore, token);
				}
				catch (OperationCanceledException) when (token.IsCancellationRequested)
				{
					break;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Ошибка очистки ProcessedMessages");
				}
                
                await Task.Delay(TimeSpan.FromHours(_options.Value.IntervalHours), token);
			}
		}
	}
}