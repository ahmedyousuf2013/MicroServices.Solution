using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Base.QueueAttribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class QueueNameAttribute : Attribute
    {
        public string Name { get; }
        public QueueNameAttribute(string name) => Name = name;
    }
}
