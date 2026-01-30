using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.Exceptions
{
    public class QueueingException : Exception
    {
        public QueueingException(string message) : base(message)
        {

        }
        public QueueingException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
