using AutoMapper;
using ModerationService.Common.DTO;
using ModerationService.DAL.Models;
using Shared.Contracts.DTO;

namespace ModerationService.DAL.Mappings
{
	/// <summary>
	/// Профиль маппинга AutoMapper для сущностей модерации
	/// </summary>
	public class AppMappingProfile : Profile
	{
		public AppMappingProfile()
		{
			CreateMap<ModerationResultDTO, PostModeratedEvent>()
				.ForMember(d => d.PendingId, opt => opt.MapFrom(c => c.PostPendingId));

			CreateMap<PostModeratedEvent, ModerationResultDTO>()
				.ForMember(d => d.PostPendingId, opt => opt.MapFrom(c => c.PendingId))
				.ForMember(d => d.Id, opt => opt.Ignore());

			CreateMap<ModerationResultEntity, ModerationResultDTO>().ReverseMap();
		}
	}
}
