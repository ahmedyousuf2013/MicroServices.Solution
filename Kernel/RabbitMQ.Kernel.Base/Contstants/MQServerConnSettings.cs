using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Contstants
{
    public class MQServerConnSettings
    {
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool IsWinOS { get; set; }
        public bool SenderEnabled { get; set; }
        public bool ReceiverEnabled { get; set; }

        public int Port { get; set; }
    }
}
