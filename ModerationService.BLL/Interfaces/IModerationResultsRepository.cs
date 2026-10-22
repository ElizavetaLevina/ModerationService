using ModerationService.Common.DTO;

namespace ModerationService.BLL.Interfaces
{
	/// <summary>
	/// Репозиторий для работы с результатами модерации
	/// </summary>
	public interface IModerationResultsRepository
	{
		/// <summary>
		/// Получение списка результатов модераций
		/// </summary>
		/// <param name="token">Токен отмены</param>
		/// <returns>Список результатов</returns>
		Task<List<ModerationResultDTO>> GetModerationResultsBatch(CancellationToken token = default);

		/// <summary>
		/// Сохранение результата модерации
		/// </summary>
		/// <param name="postSubmitted">Отмодерированный пост</param>
		/// <param name="token">Токен отмены</param>
		/// <returns>Задача сохранения</returns>
		Task SaveModerationResult(ModerationResultDTO postSubmitted, CancellationToken token = default);

		/// <summary>
		/// Удаление результатов модерации
		/// </summary>
		/// <param name="moderationResultIds">Коллекция идентификаторов результатов</param>
		/// <param name="token">Токен отмены</param>
		/// <returns>Задача удаления</returns>
		Task<IReadOnlyCollection<int>> DeleteModerationResults(IReadOnlyCollection<int> moderationResultIds,  CancellationToken token = default);

        /// <summary>
        /// Увеличивает счетчик попыток отправления результата модерации
        /// </summary>
        /// <param name="moderationResultId">Идентификаторы результата модерации</param>
        /// <param name="maxRetries">Максимальное количество повторов</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Задача инкремента</returns>
		Task IncrementRetryCount(int moderationResultId, int maxRetries = 5, CancellationToken token = default);
	}
}
