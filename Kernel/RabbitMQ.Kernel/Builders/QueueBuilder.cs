
using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Base.Contstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Builders
{
    public class QueueBuilder<TMessage> : IQueueBuilder<TMessage>
     where TMessage : class, IQueueMessage
    {
        private readonly QueueDefinition<TMessage> _definition = new();

        public IQueueBuilder<TMessage> SetQueue(string queueName)
        {
            _definition.QueueName = queueName;
            return this;
        }

        public IQueueBuilder<TMessage> SetQueueType(Strategy exchangeType)
        {
            _definition.ExchangeType = exchangeType;
            return this;
        }

        public IQueueBuilder<TMessage> WithRoutingKey(string routingKey, string routingPattern)
        {
            _definition.RoutingKey = routingKey;
            return this;
        }
        public IQueueBuilder<TMessage> WithHeaders(Dictionary<string, object> headers)
        {
            _definition.Headers = headers;
            return this;
        }

        public QueueDefinition<TMessage> Build()
        {
            Validate();
            return _definition;
        }

        private void Validate()
        {
            switch (_definition.ExchangeType)
            {
                case Strategy.Header when _definition.Headers == null:
                    throw new InvalidOperationException("Header exchange requires headers.");

                case Strategy.Topic
                    when string.IsNullOrWhiteSpace(_definition.RoutingKey) || string.IsNullOrWhiteSpace(_definition.RoutingPattern):
                    throw new InvalidOperationException("RoutingKey is required for Topic/Direct exchange.");
            }
        }
    }
}
