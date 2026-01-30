using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Contracts
{
    public interface IQueueProducerBuilder<TMessage>
        where TMessage : IQueueMessage
    {
        IQueueProducerBuilder<TMessage> UseStrategy(Strategy strategy);
        IQueueProducerBuilder<TMessage> WithHeaders(Dictionary<string, object> headers);
        IQueueProducerBuilder<TMessage> WithQueue(string queueName);

        IQueueProducerBuilder<TMessage> WithPattern(string routingPattern, string routingKey);

        IQueueProducer<TMessage> Build();
    }
}
