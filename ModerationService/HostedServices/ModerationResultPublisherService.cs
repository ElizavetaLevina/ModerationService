
using Microsoft.Extensions.Options;
using ModerationService.BLL.Interfaces;
using ModerationService.Options;

namespace ModerationService.HostedServices
{
	/// <summary>
	/// Фоновый процесс для отправки результатов модерации.
	/// Создаёт scope для получения Scoped-сервисо
	/// </summary>
	/// <param name="serviceProvider">Провайдер сервисов для создания scope</param>
	/// <param name="options">Настройки задержки между итерациями</param>
	/// <param name="logger">Логгер</param>
	public class ModerationResultPublisherService(IServiceProvider serviceProvider, IOptions<ModerationResultPublisherOptions> options, ILogger<ModerationResultPublisherService> logger) : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider = serviceProvider;
		private readonly IOptions<ModerationResultPublisherOptions> _options = options;
		private readonly ILogger<ModerationResultPublisherService> _logger = logger;

		/// <summary>
		/// Основной цикл фонового процесса. Получает батч результатов модерации, кроме тех кто со статусом Failed и отправляет их обратно в BackendService.
		/// При отсутствии постов или ошибке ждёт DelaySeconds секунд перед следующей итерацией.
		/// </summary>
		/// <param name="token">Токен отмены</param>
		/// <returns>Задача фонового процесса</returns>
		protected override async Task ExecuteAsync(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				try
				{
					using var scope = _serviceProvider.CreateScope();
					var logic = scope.ServiceProvider.GetRequiredService<IModerationResultPublisherLogic>();

					var count = await logic.PublishMessage(token);

					if (count == 0)
						await Task.Delay(TimeSpan.FromMinutes(_options.Value.DelaySeconds), token);
				}
				catch (OperationCanceledException) when (token.IsCancellationRequested)
				{
					break;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Ошибка в фоновом процессе отправки результатов модерации");
                    await Task.Delay(TimeSpan.FromSeconds(_options.Value.DelaySeconds), token);
				}
			}
		}
	}
}
