using RabbitMQ.Kernel.Base.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.QueueProviderStrategies
{
    public class StreamQueueChannelProvider<TQueueMessage> : QueueChannelProviderBase<TQueueMessage>, IQueueChannelProvider<TQueueMessage> where TQueueMessage : IQueueMessage
    {
        public StreamQueueChannelProvider(IChannelProvider channelProvider) : base(channelProvider) { }

        public override void DeclareQueueAndDeadLetter(string _queueName)
        {
            // Declare the DeadLetter Queue
            var streamQueueArgs = new Dictionary<string, object>
        {
            { "x-queue-type", "stream" }
        };
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, streamQueueArgs);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 100, global: false);
        }
    }
}
