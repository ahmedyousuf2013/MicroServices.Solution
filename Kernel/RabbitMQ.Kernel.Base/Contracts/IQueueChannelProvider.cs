using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Contracts
{

    public interface IQueueChannelProvider<TQueueMessage> where TQueueMessage : IQueueMessage
    {
        public IModel GetChannel(string queueName);

        public IModel GetChannel();


    }
}
