using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Kernel.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Implementation.QueueProducerStrategies
{
    public class QueueProducerStrategyFactory : IQueueProducerStrategyFactory
    {
        private readonly IServiceProvider serviceProvider;
        private Strategy? strategy;

        public QueueProducerStrategyFactory(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }
        public IQueueProducer<TMessage> SetQueueStrategy<TMessage>(Strategy strategy) where TMessage : IQueueMessage
        {
            this.strategy = strategy;
            return strategy switch
            {
                Strategy.Deafult => serviceProvider.GetRequiredService<DefaultQueueProducer<TMessage>>(),
                Strategy.Direct => serviceProvider.GetRequiredService<DirectQueueProducer<TMessage>>(),
                Strategy.Header => serviceProvider.GetRequiredService<HeaderQueueProducer<TMessage>>(),
                Strategy.Fanout => serviceProvider.GetRequiredService<FanoutQueueProducer<TMessage>>(),
                Strategy.Topic => serviceProvider.GetRequiredService<TopicQueueProducer<TMessage>>(),
                Strategy.Stream => serviceProvider.GetRequiredService<StreamQueueProducerr<TMessage>>(),
                //_ => throw new NotImplementedException($"The strategy '{strategy}' is not implemented."),
            };
        }

    }
}
