using RabbitMQ.Kernel.Base.Contstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Contracts
{
    public interface IQueueBuilder<TQueueMessage>
      where TQueueMessage : class, IQueueMessage
    {
        IQueueBuilder<TQueueMessage> SetQueue(string queueName);
        IQueueBuilder<TQueueMessage> SetQueueType(Strategy exchangeType);

        IQueueBuilder<TQueueMessage> WithRoutingKey(string routingKey, string routingPattern);
        IQueueBuilder<TQueueMessage> WithHeaders(Dictionary<string, object> headers);

        QueueDefinition<TQueueMessage> Build();

    }
}