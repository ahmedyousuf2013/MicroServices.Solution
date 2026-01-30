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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Implementation.QueueProducerStrategies
{
    public class TopicQueueProducer<TQueueMessage> : QueueProducerBasic<TQueueMessage>, IQueueProducer<TQueueMessage> where TQueueMessage : IQueueMessage
    {
        private IModel _channel;
        private string _routigkey;
        private string _routingPattern;

        public TopicQueueProducer(ILogger<QueueProducerBasic<TQueueMessage>> logger, IQueueNameResolver queueNameResolver, IServiceProvider serviceProvider) : base(serviceProvider.GetRequiredService<TopicQueueChannelProvider<TQueueMessage>>(), logger, queueNameResolver)
        {

            //_channel = channelProvider.GetChannel(typeof(TQueueMessage).Name);

            _serviceProvider = serviceProvider;

            base._queueName = $"{Setting._queueNametopic}{queueNameResolver.Resolve<TQueueMessage>()}" ?? typeof(TQueueMessage).Name;

        }

        public override void PublishMessage(TQueueMessage message)
        {
            if (Equals(message, default(TQueueMessage))) throw new ArgumentNullException(nameof(message));

            if (message.TimeToLive.Ticks <= 0) throw new QueueingException($"{nameof(message.TimeToLive)} cannot be zero or negative");

            // Set message ID
            message.MessageId = Guid.NewGuid();

            try
            {
                var serializedMessage = SerializeMessage(message);

                _channel.BasicPublish(exchange: base._queueName, routingKey: this._routigkey, true, null, body: serializedMessage);
            }
            catch (Exception ex)
            {
                base._logger.LogError(ex, ex.Message);
                throw new QueueingException(ex.Message);
            }
        }
        public void SetQueueProvider(string routingPattern, string routingKey)
        {

            _routigkey = routingKey;

            _routingPattern = routingPattern;

            var topicQueueChannel = base._channelProvider as TopicQueueChannelProvider<TQueueMessage>;

            topicQueueChannel.SetRoutingKey(this._routingPattern);

            _channel = topicQueueChannel.GetChannel();
        }
    }
}
