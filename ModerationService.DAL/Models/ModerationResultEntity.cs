using Shared.Contracts.Enum;

namespace ModerationService.DAL.Models
{
	/// <summary>
	/// Сущность результата модерации
	/// </summary>
	public class ModerationResultEntity
	{
		/// <summary>
		/// Уникальный идентификатор
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Ссылка на PostPendingEntity
		/// </summary>
		public int PostPendingId { get; set; }

		/// <summary>
		/// Статус модерации
		/// </summary>
		public StatusModerationEnum Status { get; set; }

		/// <summary>
		/// Причина отклонения
		/// </summary>
		public string? RejectionReason { get; set; } = null;

		/// <summary>
		/// Дата завершения модерации
		/// </summary>
		public DateTime DateModerate { get; set; } = DateTime.Now;
	}
}
