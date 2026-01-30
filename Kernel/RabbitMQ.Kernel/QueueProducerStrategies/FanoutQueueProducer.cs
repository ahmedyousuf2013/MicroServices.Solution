using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Kernel.Base.Basic;
using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Base.Contstants;
using RabbitMQ.Kernel.Base.Exceptions;
using RabbitMQ.Kernel.Base.QueueAttribute;
using RabbitMQ.Kernel.QueueProviderStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Implementation.QueueProducerStrategies
{
    public class FanoutQueueProducer<TQueueMessage> : QueueProducerBasic<TQueueMessage>, IQueueProducer<TQueueMessage> where TQueueMessage : IQueueMessage
    {
        public FanoutQueueProducer(IQueueChannelProvider<TQueueMessage> channelProvider,
            ILogger<QueueProducerBasic<TQueueMessage>> logger,
            IQueueNameResolver queueNameResolver,
            IServiceProvider serviceProvider) : base(serviceProvider.GetRequiredService<FanoutQueueChannelProvider<TQueueMessage>>(),
                logger,
                queueNameResolver)
        {
            //_channel = channelProvider.GetChannel(typeof(TQueueMessage).Name);


            // _queueName = typeof(TQueueMessage).Name;

            base._queueName = $"{Setting._queueNamefanout}{queueNameResolver.Resolve<TQueueMessage>()}" ?? typeof(TQueueMessage).Name;
        }

        public override void PublishMessage(TQueueMessage message)
        {
            //Declare the queue if it does not exist
            var channel = base._channelProvider.GetChannel(_queueName);

            if (Equals(message, default(TQueueMessage))) throw new ArgumentNullException(nameof(message));

            if (message.TimeToLive.Ticks <= 0) throw new QueueingException($"{nameof(message.TimeToLive)} cannot be zero or negative");

            // Set message ID
            message.MessageId = Guid.NewGuid();

            try
            {
                var serializedMessage = SerializeMessage(message);

                channel.BasicPublish(exchange: base._queueName, routingKey: "", true, null, body: serializedMessage);
            }
            catch (Exception ex)
            {
                base._logger.LogError(ex, ex.Message);
                throw new QueueingException(ex.Message);
            }
        }
    }
}
