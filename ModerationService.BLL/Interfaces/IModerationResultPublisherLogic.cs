namespace ModerationService.BLL.Interfaces
{
	/// <summary>
	/// Логика публикации постов на модерацию
	/// </summary>
	public interface IModerationResultPublisherLogic
	{
		/// <summary>
		/// Отправка постов на модерацию и обновление их статусов
		/// </summary>
		/// <param name="token">Токен отмены</param>
		/// <returns>Задача отправки постов</returns>
		Task PublishMessage(CancellationToken token = default);
	}
}
