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
    public class FanoutQueueChannelProvider<TQueueMessage> : QueueChannelProviderBase<TQueueMessage>, IQueueChannelProvider<TQueueMessage> where TQueueMessage : IQueueMessage
    {
        public FanoutQueueChannelProvider(IChannelProvider channelProvider) : base(channelProvider)
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

            base._channel.ExchangeDeclare(exchange: deadLetterQueueName, type: ExchangeType.Fanout);
            base._channel.QueueDeclare(queue: deadLetterQueueName, durable: true, exclusive: false, autoDelete: false, arguments: deadLetterQueueArgs);
            base._channel.QueueBind(queue: deadLetterQueueName, exchange: deadLetterQueueName, routingKey: "", null);

            // Declare the Queue
            var queueArgs = new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", deadLetterQueueName },
                    { "x-dead-letter-routing-key", deadLetterQueueName },
                    { "x-queue-type", "quorum" },
                    { "x-dead-letter-strategy", "at-least-once" }, // Ensure that deadletter messages are delivered in any case see: https://www.rabbitmq.com/quorum-queues.html#dead-lettering
                    { "overflow", "reject-publish" } // If the queue is full, reject the publish
                };


            base._channel.ExchangeDeclare(exchange: _queueName, type: ExchangeType.Fanout);

            base._channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: queueArgs);

            base._channel.QueueBind(queue: _queueName, exchange: _queueName, routingKey: "", null);
        }
    }
}
