using ModerationService.Common.DTO;
using Shared.Contracts.DTO;

namespace ModerationService.Common.Mappings
{
	public static class ModerationMappingExtensions
	{
		public static PostModeratedEvent ToPostModeratedEvent(this ModerationResultDTO dto) => new PostModeratedEvent()
		{
			PendingId = dto.PostPendingId, RejectionReason = dto.RejectionReason, Status = dto.Status
		};
	}
}