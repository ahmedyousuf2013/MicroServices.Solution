using RabbitMQ.Client;
using RabbitMQ.Kernel.Base.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Basic
{
    public abstract class QueueChannelProviderBase<TQueueMessage> : IQueueChannelProvider<TQueueMessage> where TQueueMessage : IQueueMessage
    {
        protected readonly IChannelProvider channelProvider;
        protected IModel _channel;
        protected QueueChannelProviderBase(IChannelProvider _channelProvider)
        {

            channelProvider = _channelProvider;
        }

        public abstract void DeclareQueueAndDeadLetter(string _queueName = null);


        public IModel GetChannel(string queueName)
        {
            // _queueName = queueName;
            _channel = channelProvider.GetChannel();
            DeclareQueueAndDeadLetter(queueName);
            return _channel;
        }

        public IModel GetChannel()
        {
            // _queueName = queueName;
            _channel = channelProvider.GetChannel();
            return _channel;
        }
    }
}
