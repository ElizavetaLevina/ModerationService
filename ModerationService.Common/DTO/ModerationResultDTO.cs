using Shared.Contracts.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModerationService.Common.DTO
{
	/// <summary>
	/// DTO для передачи данных о результате модерации
	/// </summary>
	public class ModerationResultDTO
    {
		/// <summary>
		/// Уникальный идентификатор
		/// </summary>
		[Column("id")]
		public int Id { get; set; }

		/// <summary>
		/// Идентификатор поста на модерации
		/// </summary>
		[Column("post_pending_id")]
		public int PostPendingId { get; set; }

		/// <summary>
		/// Статус модерации
		/// </summary>
		[Column("status")]
		public StatusModerationEnum Status { get; set; }

		/// <summary>
		/// Причина отклонения
		/// </summary>
		[Column("rejection_reason")]
		public string? RejectionReason { get; set; } = null;

		/// <summary>
		/// Дата завершения модерации
		/// </summary>
		[Column("date_moderate")]
		public DateTime DateModerate { get; set; } = DateTime.Now;
    }
}
