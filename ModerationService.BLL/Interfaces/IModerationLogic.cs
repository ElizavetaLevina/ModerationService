namespace ModerationService.BLL.Interfaces
{
	/// <summary>
	/// Логика модерации поста
	/// </summary>
	public interface IModerationLogic
	{
		/// <summary>
		/// Проверяет, одобрен ли пост (не содержит нецензурной лексики)
		/// </summary>
		/// <param name="title">Заголовок</param>
		/// <param name="textPost">Контент</param>
		/// <returns>Результат проверки</returns>
		bool IsApproved(string title, string textPost);
	}
}
