using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Contracts
{
    public interface IChannelProviderStrategyFactory
    {
        public IQueueChannelProvider<TQueueMessage> Create<TQueueMessage>(Strategy strategy) where TQueueMessage : IQueueMessage;

    }
}
