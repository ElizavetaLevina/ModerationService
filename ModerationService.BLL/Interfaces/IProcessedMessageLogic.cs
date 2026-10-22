namespace ModerationService.BLL.Interfaces
{
	public interface IProcessedMessageLogic
	{
		/// <summary>
		/// Сохранение сообщения
		/// </summary>
		/// <param name="messageId">Идентификатор сообщения</param>
		/// <param name="token">Токен отмены</param>
		/// <returns>Задача добавлния</returns>
		Task AddMessage(Guid messageId, CancellationToken token = default);

        /// <summary>
        /// Проверка, обработано ли сообщение
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Результат проверки</returns>
		Task<bool> HasMessage(Guid messageId, CancellationToken token = default);

		/// <summary>
        /// Удаление сообщений, которые старше указанной даты
        /// </summary>
        /// <param name="deleteBefore">Дата, до которой записи считаются устаревшими</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Задача удаления</returns>
        Task DeleteMessages(DateTime deleteBefore, CancellationToken token = default);
	}
}