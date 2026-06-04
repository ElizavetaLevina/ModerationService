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
		Task<List<ModerationResultDTO>> GetModerationResults(CancellationToken token = default);

		/// <summary>
		/// Сохранение результата модерации
		/// </summary>
		/// <param name="postSubmitted">Отмодерированный пост</param>
		/// <param name="token">Токен отмены</param>
		/// <returns>Задача сохранения</returns>
		Task SaveModerationResult(ModerationResultDTO postSubmitted, CancellationToken token = default);

		/// <summary>
		/// Удаление результата модерации
		/// </summary>
		/// <param name="moderationResultId">Идентификатор результата</param>
		/// <param name="token">Токен отмены</param>
		/// <returns>Задача удаления</returns>
		Task DeleteModerationResult(int moderationResultId,  CancellationToken token = default);
	}
}
