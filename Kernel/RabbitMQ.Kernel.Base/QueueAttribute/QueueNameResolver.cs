
using RabbitMQ.Kernel.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.QueueAttribute
{
    public interface IQueueNameResolver
    {
        string Resolve<TQueueMessage>() where TQueueMessage : IQueueMessage;
    }

    public class QueueNameResolver : IQueueNameResolver
    {

        public string Resolve<TQueueMessage>() where TQueueMessage : IQueueMessage
        {
            var typeName = typeof(TQueueMessage).Name;

            var attr = typeof(TQueueMessage).GetCustomAttribute<QueueNameAttribute>();
            if (attr != null)
                return attr.Name;

            throw new InvalidOperationException($"Queue name not configured for {typeName}");
        }
    }

}
