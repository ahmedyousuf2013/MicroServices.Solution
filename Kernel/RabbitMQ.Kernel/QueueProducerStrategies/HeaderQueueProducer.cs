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
    public class HeaderQueueProducer<TQueueMessage> : QueueProducerBasic<TQueueMessage>, IQueueProducer<TQueueMessage> where TQueueMessage : IQueueMessage
    {
        private IBasicProperties properties;
        private IModel _channel;

        public HeaderQueueProducer(ILogger<QueueProducerBasic<TQueueMessage>> logger,
            IQueueNameResolver queueNameResolver,
            IServiceProvider serviceProvider) : base(serviceProvider.GetRequiredService<HeaderQueueChannelProvider<TQueueMessage>>(),
                logger,
                queueNameResolver)
        {

            base._queueName = $"{Setting._queueNameheader}{queueNameResolver.Resolve<TQueueMessage>()}" ?? typeof(TQueueMessage).Name;

            //Declare the queue if it does not exist

        }

        public Dictionary<string, object> Headers { get; private set; }

        public override void PublishMessage(TQueueMessage message)
        {

            properties = _channel.CreateBasicProperties();
            properties.Headers = Headers;


            if (Equals(message, default(TQueueMessage))) throw new ArgumentNullException(nameof(message));

            if (message.TimeToLive.Ticks <= 0) throw new QueueingException($"{nameof(message.TimeToLive)} cannot be zero or negative");

            // Set message ID
            message.MessageId = Guid.NewGuid();

            try
            {

                var serializedMessage = SerializeMessage(message);

                properties.Persistent = true;
                properties.Type = base._queueName;
                properties.Expiration = message.TimeToLive.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);


                _channel.BasicPublish(exchange: _queueName, routingKey: "", true, properties, body: serializedMessage);
            }
            catch (Exception ex)
            {
                base._logger.LogError(ex, ex.Message);
                throw new QueueingException(ex.Message);
            }
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            Headers = headers;

            var HeaderQueueChannel = _channelProvider as HeaderQueueChannelProvider<TQueueMessage>;

            this._channel = _channelProvider.GetChannel();
        }

    }
}
