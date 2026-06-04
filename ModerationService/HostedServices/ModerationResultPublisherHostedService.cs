
using ModerationService.BLL.Interfaces;

namespace ModerationService.HostedServices
{
	/// <summary>
	/// Фоновый процесс для отправки результатов модерации.
	/// Запускается каждые 5 секунд, создаёт scope для получения Scoped-сервисов.
	/// </summary>
	/// <param name="serviceProvider">Провайдер сервисов для создания scope</param>
	/// <param name="logger">Логгер</param>
	public class ModerationResultPublisherHostedService(IServiceProvider serviceProvider, ILogger<ModerationResultPublisherHostedService> logger) : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider = serviceProvider;
		private readonly ILogger<ModerationResultPublisherHostedService> _logger = logger;

		/// <summary>
		/// Основной цикл фонового процесса. Каждые 5 секунд получает батч результатов модерации и отправляет их в очередь.
		/// При недоступности RabbitMQ ожидает его восстановления.
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

					await logic.PublishMessage(token);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Ошибка в фоновом процессе отправки результатов модерации");
				}

				await Task.Delay(TimeSpan.FromSeconds(5), token);
			}
		}
	}
}
