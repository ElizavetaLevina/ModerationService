namespace ModerationService.BLL.Interfaces
{
	/// <summary>
	/// Логика публикации резлуьтатов модерации
	/// </summary>
	public interface IModerationResultPublisherLogic
	{
		/// <summary>
		/// Отправка резлуьтатов модерации и их удаление
		/// </summary>
		/// <param name="token">Токен отмены</param>
		/// <returns>Количество отправленных результатов модерации</returns>
		Task<int> PublishMessage(CancellationToken token = default);
	}
}
