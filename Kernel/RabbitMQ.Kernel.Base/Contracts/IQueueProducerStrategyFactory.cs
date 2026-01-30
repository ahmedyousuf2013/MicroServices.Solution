using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Contracts 
{ 

    public interface IQueueProducerStrategyFactory
    {
        IQueueProducer<TMessage> SetQueueStrategy<TMessage>(Strategy strategy) where TMessage : IQueueMessage;

    }
}
