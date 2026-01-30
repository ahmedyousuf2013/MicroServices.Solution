using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Base.QueueAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Basic
{
    public abstract class QueueProducerBasic<TQueueMessage> : IQueueProducer<TQueueMessage> where TQueueMessage : IQueueMessage
    {
        protected readonly ILogger<QueueProducerBasic<TQueueMessage>> _logger;
        protected readonly IQueueChannelProvider<TQueueMessage> _channelProvider;
        protected string _queueName;
        protected IServiceProvider _serviceProvider;

        public QueueProducerBasic(IQueueChannelProvider<TQueueMessage> channelProvider, ILogger<QueueProducerBasic<TQueueMessage>> logger, IQueueNameResolver queueNameResolver)
        {
            _logger = logger;
            //_channel = channelProvider.GetChannel(typeof(TQueueMessage).Name);

            _channelProvider = channelProvider;
            // _queueName = typeof(TQueueMessage).Name;

        }
        public abstract void PublishMessage(TQueueMessage message);


        public void DeclareQueue(string queueName = null)
        {
            queueName ??= _queueName;
            if (queueName is not null) _queueName = queueName;
            _channelProvider.GetChannel(queueName);
        }



        protected static byte[] SerializeMessage(TQueueMessage message)
        {
            var stringContent = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(stringContent);
        }

    }
}
