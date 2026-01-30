using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Contracts
{
    public interface IQueueConsumerHandler<TMessageConsumer, TQueueMessage>
      where TMessageConsumer : IQueueConsumer<TQueueMessage>
      where TQueueMessage : class, IQueueMessage
    {

        void CancelQueueConsumer();

        void RegisterQueueConsumer();

    }
}
