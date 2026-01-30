using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Implementation.QueueProducerStrategies;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Builders
{
    public class QueueProducerBuilder<TMessage> : IQueueProducerBuilder<TMessage>
       where TMessage : IQueueMessage
    {
        private readonly IQueueProducerStrategyFactory _factory;
        private IQueueProducer<TMessage> _producer;

        private Strategy? StrategyValue;

        public string RoutingPattern;

        private string RoutingKey;
        private string QueueName;
        private Dictionary<string, object> Headers;

        public QueueProducerBuilder(IQueueProducerStrategyFactory factory, IQueueProducer<TMessage> producer)
        {
            _factory = factory;
            _producer = producer;
        }

        public IQueueProducerBuilder<TMessage> UseStrategy(Strategy strategy)
        {
            this.StrategyValue = strategy;
            _producer = _factory.SetQueueStrategy<TMessage>(strategy);
            return this;
        }

        public IQueueProducerBuilder<TMessage> WithHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
            if (_producer is HeaderQueueProducer<TMessage> headerProducer)
                headerProducer.SetHeaders(headers);
            this.Validate();
            return this;
        }

        public IQueueProducerBuilder<TMessage> WithPattern(string routingPattern, string routingKey)
        {
            RoutingPattern = routingPattern;
            RoutingKey = routingKey;

            if (_producer is TopicQueueProducer<TMessage> topicProducer)
                topicProducer.SetQueueProvider(routingPattern, routingKey);
            this.Validate();
            return this;
        }

        public IQueueProducerBuilder<TMessage> WithQueue(string queueName)
        {
            this.QueueName = queueName;
            _producer.DeclareQueue(queueName);
            this.Validate();
            return this;
        }

        public IQueueProducer<TMessage> Build()
        {
            return _producer ?? throw new InvalidOperationException("Producer not configured");
        }
        private void Validate()
        {
            if (this.StrategyValue is null)
                throw new InvalidOperationException("Producer not configured");

            switch (StrategyValue)
            {
                case Strategy.Topic when string.IsNullOrWhiteSpace(RoutingPattern) && string.IsNullOrWhiteSpace(RoutingKey):
                    throw new InvalidOperationException("RoutingKey is required for Topic/Direct exchange.");

                case Strategy.Header when Headers is null:
                    throw new InvalidOperationException("RoutingKey is required for Header exchange.");
                default:
                    break;
            }
        }
    }
}
