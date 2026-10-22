using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using Npgsql;

namespace ModerationService.BLL.Consumers
{
	public class PostSubmittedForModerationConsumerDefinition : ConsumerDefinition<PostSubmittedForModerationConsumer>
	{
		public PostSubmittedForModerationConsumerDefinition()
		{
            ConcurrentMessageLimit = 10; // ограничение числа сообщений, обрабатываемых потребителем параллельно
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<PostSubmittedForModerationConsumer> consumerConfigurator)
		{
			base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);
            
            endpointConfigurator.PrefetchCount = 20; // ограничение числа сообщений, которые брокер доставляет потребителю без подтверждения
            
            endpointConfigurator.UseMessageRetry(x =>
            {
                x.Intervals(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5));
				x.Handle<NpgsqlException>();
				x.Handle<TimeoutException>();
            });
		}
	}
}