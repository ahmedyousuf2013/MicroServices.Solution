using RabbitMQ.Client;
using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Base.Contstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.QueueProviderStrategies
{
    public class DirectQueueChannnelProvider<TQueueMessage> : QueueChannelProviderBase<TQueueMessage>, IQueueChannelProvider<TQueueMessage> where TQueueMessage : IQueueMessage
    {

        public DirectQueueChannnelProvider(IChannelProvider channelProvider) : base(channelProvider)
        {

        }

        public override void DeclareQueueAndDeadLetter(string _queueName = null)
        {

            var deadLetterQueueName = $"{_queueName}{Setting._exchangeDeadLetter}";

            // Declare the DeadLetter Queue
            var deadLetterQueueArgs = new Dictionary<string, object>
        {
            { "x-queue-type", "quorum" },
            { "overflow", "reject-publish" } // If the queue is full, reject the publish
        };

            _channel.ExchangeDeclare(deadLetterQueueName, ExchangeType.Direct);
            _channel.QueueDeclare(deadLetterQueueName, true, false, false, deadLetterQueueArgs);
            _channel.QueueBind(queue: deadLetterQueueName, exchange: deadLetterQueueName, routingKey: Setting._direct_init, null);

            // Declare the Queue
            var queueArgs = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", deadLetterQueueName },
            { "x-dead-letter-routing-key", deadLetterQueueName },
            { "x-queue-type", "quorum" },
            { "x-dead-letter-strategy", "at-least-once" }, // Ensure that deadletter messages are delivered in any case see: https://www.rabbitmq.com/quorum-queues.html#dead-lettering
            { "overflow", "reject-publish" } // If the queue is full, reject the publish
        };


            _channel.ExchangeDeclare(exchange: _queueName, type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: queueArgs);
            _channel.QueueBind(queue: _queueName, exchange: _queueName, routingKey: Setting._direct_init);

        }
    }
}
