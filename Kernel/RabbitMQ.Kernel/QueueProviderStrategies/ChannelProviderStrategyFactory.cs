using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.QueueProviderStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Implementation.QueueProviderStrategies
{
    public class ChannelProviderStrategyFactory : IChannelProviderStrategyFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ChannelProviderStrategyFactory(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }
        public IQueueChannelProvider<TQueueMessage> Create<TQueueMessage>(Strategy strategy) where TQueueMessage : IQueueMessage
        {

            return strategy switch
            {
                Strategy.Deafult => serviceProvider.GetRequiredService<DefaultQueueChannelProvider<TQueueMessage>>(),
                Strategy.Direct => serviceProvider.GetRequiredService<DirectQueueChannnelProvider<TQueueMessage>>(),
                Strategy.Header => serviceProvider.GetRequiredService<HeaderQueueChannelProvider<TQueueMessage>>(),
                Strategy.Fanout => serviceProvider.GetRequiredService<FanoutQueueChannelProvider<TQueueMessage>>(),
                Strategy.Topic => serviceProvider.GetRequiredService<TopicQueueChannelProvider<TQueueMessage>>(),
                Strategy.Stream => serviceProvider.GetRequiredService<StreamQueueChannelProvider<TQueueMessage>>(),
                //_ => throw new NotImplementedException($"The strategy '{strategy}' is not implemented."),
            };
        }
    }
}
