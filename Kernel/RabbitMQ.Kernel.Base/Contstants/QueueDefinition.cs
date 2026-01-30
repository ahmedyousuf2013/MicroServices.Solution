using RabbitMQ.Kernel.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Contstants
{
    public class QueueDefinition
    {
        public string QueueName { get; set; } = default!;
        public string ExchangeName { get; set; } = default!;
        public Strategy ExchangeType { get; set; }

        public string? RoutingKey { get; set; }
        public Dictionary<string, object>? Headers { get; set; }

        public virtual Type? MessageType { get; }

        public string? RoutingPattern { get; set; }

    }

    public class QueueDefinition<TQueueMessage> : QueueDefinition
    where TQueueMessage : class, IQueueMessage
    {
        public override Type MessageType => typeof(TQueueMessage);
    }
}
