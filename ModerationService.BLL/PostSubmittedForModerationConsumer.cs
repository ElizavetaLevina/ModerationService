using AutoMapper;
using MassTransit;
using Shared.Contracts.Enum;
using Shared.Contracts.DTO;

namespace ModerationService.BLL
{
    public class PostSubmittedForModerationConsumer : IConsumer<PostSubmittedForModeration>
    {
        public async Task Consume(ConsumeContext<PostSubmittedForModeration> context)
        {
            var message = context.Message;

            var isApproved = await Moderate(message);

            await context.Publish(new PostModeratedEvent
            {
                PendingId = message.Id,
                Status = isApproved ? StatusModerationEnum.Approved : StatusModerationEnum.Rejected,
                RejectionReason = isApproved ? null : "Причина отклонения"
            });
        }

        private Task<bool> Moderate(PostSubmittedForModeration post)
        {
            return Task.FromResult(true);
        }
    }
}
