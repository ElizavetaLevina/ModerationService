using Microsoft.Extensions.Options;
using ModerationService.Options;
using System.Net;
using System.Text.Json;

namespace ModerationService.HostedServices
{
	/// <summary>
    /// Фоновый сервис проверки очереди _error
    /// </summary>
    /// <param name="factory">Фабрика для HttpClient</param>
    /// <param name="options">Настройки проверки</param>
    /// <param name="logger">Логгер</param>
	public class ErrorQueueMonitorService(IHttpClientFactory factory, IOptions<ErrorQueueMonitorOptions> options, ILogger<ErrorQueueMonitorService> logger) : BackgroundService
	{
		private readonly IHttpClientFactory _factory = factory;
		private readonly IOptions<ErrorQueueMonitorOptions> _options = options;
		private readonly ILogger<ErrorQueueMonitorService> _logger = logger;
        
        /// <summary>
        /// Основной цикл фонового процесса. Каждые IntervalMinutes минут делает запрос для получения сообщений в очереди _error.
        /// </summary>
        /// <param name="token">Токен отмены</param>
		protected override async Task ExecuteAsync(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				try
				{
					var client = _factory.CreateClient();
                    var credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_options.Value.UserName}:{_options.Value.Password}"));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
					var url = $"{_options.Value.ManagementApiUrl}/api/queues/%2F/{_options.Value.QueueName}";
					var response = await client.GetAsync(url, token);
                    
					if (response.IsSuccessStatusCode)
					{
						var json = await response.Content.ReadAsStreamAsync(token);
						using var doc = JsonDocument.Parse(json);
						if (doc.RootElement.TryGetProperty("messages", out var messages))
						{
							var count = messages.GetInt32();
                            if (count > 0)
                                _logger.LogWarning("В  очереди {QueueName} есть {Count} необработанных сообщений", _options.Value.QueueName, count);
						}
					}
                    else if (response.StatusCode == HttpStatusCode.NotFound)
					{
                        _logger.LogInformation("Не существует очереди {QueueName}", _options.Value.QueueName);
					}
					else
					{
                        _logger.LogWarning("Не удалось получить статистику очереди {QueueName}: {StatusCode}", _options.Value.QueueName, response.StatusCode);
					}
				}
                catch (OperationCanceledException) when (token.IsCancellationRequested)
				{
					break;
				}
                catch (Exception ex)
				{
					_logger.LogError(ex, "Ошибка мониторинга очереди {QueueName}", _options.Value.QueueName);
				}

				await Task.Delay(TimeSpan.FromMinutes(_options.Value.IntervalMinutes), token);
			}
		}
	}
}