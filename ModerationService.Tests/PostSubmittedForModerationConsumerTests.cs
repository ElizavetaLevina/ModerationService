using MassTransit;
using Microsoft.Extensions.Logging;
using ModerationService.BLL.Consumers;
using ModerationService.BLL.Interfaces;
using ModerationService.Common.DTO;
using Moq;
using Shared.Contracts.DTO;
using Shared.Contracts.Enum;

namespace ModerationService.Tests
{
	public class PostSubmittedForModerationConsumerTests
	{
		private readonly Mock<IModerationResultsRepository> _moderationRepository;
		private readonly Mock<IModerationLogic> _moderationLogic;
		private readonly Mock<ConsumeContext<PostSubmittedForModeration>> _consumeContext;
		private readonly Mock<IUnitOfWork> _unitOfWork;
		private readonly Mock<IProcessedMessageLogic> _messageLogic;
		private readonly Mock <ILogger<PostSubmittedForModerationConsumer>> _logger;
		private readonly PostSubmittedForModerationConsumer consumer;

		public PostSubmittedForModerationConsumerTests()
		{
			_moderationRepository = new Mock<IModerationResultsRepository>();
			_moderationLogic = new Mock<IModerationLogic>();
			_consumeContext = new Mock<ConsumeContext<PostSubmittedForModeration>>();
			_unitOfWork = new Mock<IUnitOfWork>();
			_messageLogic = new Mock<IProcessedMessageLogic>();
			_logger = new Mock<ILogger<PostSubmittedForModerationConsumer>>();
			consumer = new PostSubmittedForModerationConsumer(_moderationRepository.Object, _moderationLogic.Object, _unitOfWork.Object, _messageLogic.Object, _logger.Object);
		}

		[Fact]
		public async Task Consume_WithApprovedPost_SavesApprovedResult()
		{
            var messageId = Guid.NewGuid();
			var message = new PostSubmittedForModeration
			{
				Id = 1,
				Title = "Title",
				TextPost = "Text"
			};

            _messageLogic.Setup(c => c.HasMessage(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
			_moderationLogic.Setup(c => c.IsApproved(message.Title, message.TextPost)).Returns(true);
			_consumeContext.Setup(c => c.Message).Returns(message);
			_consumeContext.Setup(c => c.MessageId).Returns(messageId);
			_unitOfWork.Setup(c => c.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
			_messageLogic.Setup(c => c.AddMessage(messageId, It.IsAny<CancellationToken>()))
				.Returns(Task.CompletedTask);
			_unitOfWork.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

			await consumer.Consume(_consumeContext.Object);

			_moderationRepository.Verify(c => c.SaveModerationResult(It.Is<ModerationResultDTO>(x =>
				x.PostPendingId == message.Id &&
				x.Status == StatusModerationEnum.Approved &&
				x.RejectionReason == null), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task Consume_WithRejectedPost_SavesRejectedResult()
		{
            var messageId = Guid.NewGuid();
			var message = new PostSubmittedForModeration
			{
				Id = 1,
				Title = "Title",
				TextPost = "Fuck"
			};

			_messageLogic.Setup(c => c.HasMessage(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
			_moderationLogic.Setup(c => c.IsApproved(message.Title, message.TextPost)).Returns(false);
			_consumeContext.Setup(c => c.Message).Returns(message);
			_consumeContext.Setup(c => c.MessageId).Returns(messageId);
            _unitOfWork.Setup(c => c.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
			_messageLogic.Setup(c => c.AddMessage(messageId, It.IsAny<CancellationToken>()))
				.Returns(Task.CompletedTask);
			_unitOfWork.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

			await consumer.Consume(_consumeContext.Object);
            
			_moderationRepository.Verify(c => c.SaveModerationResult(It.Is<ModerationResultDTO>(x =>
				x.PostPendingId == message.Id &&
				x.Status == StatusModerationEnum.Rejected &&
				x.RejectionReason != null), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task Consume_CallModerationWithCorrectData()
		{
			var messageId = Guid.NewGuid();
			var message = new PostSubmittedForModeration
			{
				Id = 1,
				Title = "Title",
				TextPost = "Text"
			};
            
			_messageLogic.Setup(c => c.HasMessage(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
			_consumeContext.Setup(c => c.Message).Returns(message);
			_consumeContext.Setup(c => c.MessageId).Returns(messageId);

			await consumer.Consume(_consumeContext.Object);

			_messageLogic.Verify(c => c.HasMessage(messageId, It.IsAny<CancellationToken>()), Times.Once);
			_moderationLogic.Verify(c => c.IsApproved(message.Title, message.TextPost), Times.Once);
		}
	}
}