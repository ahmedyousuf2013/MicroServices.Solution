using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Contstants
{
    public class QueueSettings
    {
        public Dictionary<string, string> Queues { get; set; } = new();
    }
}
