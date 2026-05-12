using AutoMapper;
using Shared.Contracts.DTO;

namespace ModerationService.BLL.Mappings
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile() 
        {
            CreateMap<PostSubmittedForModeration, PostModeratedEvent>()
                .ForMember(s => s.PendingId, d => d.MapFrom(x => x.Id))
                .ForMember(s => s.RejectionReason, d => d.Ignore())
                .ForMember(s => s.DateModerate, d => d.Ignore())
                .ForMember(s => s.Status, d => d.Ignore());
        }
    }
}
