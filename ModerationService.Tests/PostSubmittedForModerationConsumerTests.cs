using MassTransit;
using ModerationService.BLL.Consumers;
using ModerationService.BLL.Interfaces;
using ModerationService.Common.DTO;
using Moq;
using ModerationService.BLL.Logics;
using Shared.Contracts.DTO;
using Shared.Contracts.Enum;

namespace ModerationService.Tests
{
	public class PostSubmittedForModerationConsumerTests
	{
		private readonly Mock<IModerationResultsRepository> _moderationRepository;
		private readonly Mock<IModerationLogic> _moderationLogic;
		private readonly Mock<ConsumeContext<PostSubmittedForModeration>> _consumeContext;
		private readonly PostSubmittedForModerationConsumer consumer;

		public PostSubmittedForModerationConsumerTests()
		{
			_moderationRepository = new Mock<IModerationResultsRepository>();
			_moderationLogic = new Mock<IModerationLogic>();
			_consumeContext = new Mock<ConsumeContext<PostSubmittedForModeration>>();
			consumer = new PostSubmittedForModerationConsumer(_moderationRepository.Object, _moderationLogic.Object);
		}

		[Fact]
		public async Task Consume_WithApprovedPost_SavesApprovedResult()
		{
			var message = new PostSubmittedForModeration
			{
				Id = 1,
				Title = "Title",
				TextPost = "Text"
			};

			_moderationLogic.Setup(c => c.IsApproved(message.Title, message.TextPost)).Returns(true);
			_consumeContext.Setup(c => c.Message).Returns(message);

			await consumer.Consume(_consumeContext.Object);

			var resultDTO = new ModerationResultDTO { PostPendingId = message.Id, Status = StatusModerationEnum.Approved, RejectionReason = null };

			_moderationRepository.Verify(c => c.SaveModerationResult(It.Is<ModerationResultDTO>(x =>
				x.PostPendingId == message.Id &&
				x.Status == StatusModerationEnum.Approved &&
				x.RejectionReason == null), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task Consume_WithRejectedPost_SavesRejectedResult()
		{
			var message = new PostSubmittedForModeration
			{
				Id = 1,
				Title = "Title",
				TextPost = "Fuck"
			};

			_moderationLogic.Setup(c => c.IsApproved(message.Title, message.TextPost)).Returns(false);
			_consumeContext.Setup(c => c.Message).Returns(message);

			await consumer.Consume(_consumeContext.Object);

			var resultDTO = new ModerationResultDTO { PostPendingId = message.Id, Status = StatusModerationEnum.Approved, RejectionReason = null };

			_moderationRepository.Verify(c => c.SaveModerationResult(It.Is<ModerationResultDTO>(x =>
				x.PostPendingId == message.Id &&
				x.Status == StatusModerationEnum.Rejected &&
				x.RejectionReason != null), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task Consume_CallModerationWithCorrectData()
		{
			var message = new PostSubmittedForModeration
			{
				Id = 1,
				Title = "Title",
				TextPost = "Text"
			};

			_consumeContext.Setup(c => c.Message).Returns(message);

			await consumer.Consume(_consumeContext.Object);

			_moderationLogic.Verify(c => c.IsApproved(message.Title, message.TextPost), Times.Once);
		}
	}
}