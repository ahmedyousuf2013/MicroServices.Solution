using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Contracts
{
    public interface IQueueProducer<in TQueueMessage> where TQueueMessage : IQueueMessage
    {
        void PublishMessage(TQueueMessage message);

        void DeclareQueue(string queueName = null);
    }

    public enum Strategy
    {
        Deafult = 0,
        Direct = 1,
        Topic = 2,
        Header = 3,
        Fanout = 4,
        Stream = 5
    }
}
