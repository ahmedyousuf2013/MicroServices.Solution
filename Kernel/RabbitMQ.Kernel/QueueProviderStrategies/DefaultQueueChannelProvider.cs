using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Base.Contstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.QueueProviderStrategies
{
    public class DefaultQueueChannelProvider<TQueueMessage> : QueueChannelProviderBase<TQueueMessage>, IQueueChannelProvider<TQueueMessage> where TQueueMessage : IQueueMessage
    {


        public DefaultQueueChannelProvider(IChannelProvider channelProvider) : base(channelProvider)
        {
        }

        public override void DeclareQueueAndDeadLetter(string _queueName)
        {
            _channel.QueueDeclare(queue: Setting._default, durable: true, exclusive: false, autoDelete: false, arguments: null);

        }
    }
}
