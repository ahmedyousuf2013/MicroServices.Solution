using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Kernel.Base.Basic;
using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Base.Contstants;
using RabbitMQ.Kernel.Base.Exceptions;
using RabbitMQ.Kernel.Base.QueueAttribute;

using RabbitMQ.Kernel.QueueProviderStrategies;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Implementation.QueueProducerStrategies
{
    public class StreamQueueProducerr<TQueueMessage> : QueueProducerBasic<TQueueMessage>, IQueueProducer<TQueueMessage> where TQueueMessage : IQueueMessage
    {

        protected readonly ILogger<QueueProducerBasic<TQueueMessage>> _logger;
        protected readonly string _queueName;
        public StreamQueueProducerr(IQueueChannelProvider<TQueueMessage> channelProvider,
            ILogger<QueueProducerBasic<TQueueMessage>> logger,
            IQueueNameResolver queueNameResolver,
            IServiceProvider serviceProvider) : base(serviceProvider.GetRequiredService<StreamQueueChannelProvider<TQueueMessage>>(),
                logger,
                queueNameResolver)
        {

            //_channel = channelProvider.GetChannel(typeof(TQueueMessage).Name);

            // _queueName = typeof(TQueueMessage).Name;

            base._queueName = $"{Setting._queueStream}{queueNameResolver.Resolve<TQueueMessage>()}" ?? typeof(TQueueMessage).Name;
        }

        public override void PublishMessage(TQueueMessage message)
        {
            //Declare the queue if it does not exist
            var channel = _channelProvider.GetChannel(base._queueName);

            if (Equals(message, default(TQueueMessage))) throw new ArgumentNullException(nameof(message));

            if (message.TimeToLive.Ticks <= 0) throw new QueueingException($"{nameof(message.TimeToLive)} cannot be zero or negative");

            // Set message ID
            message.MessageId = Guid.NewGuid();

            try
            {
                var serializedMessage = SerializeMessage(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.Type = _queueName;
                properties.Expiration = message.TimeToLive.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);


                channel.BasicPublish(exchange: "", routingKey: base._queueName, basicProperties: properties, body: serializedMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new QueueingException(ex.Message);
            }
        }
    }
}
