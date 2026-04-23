using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Base.QueueAttribute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stock.Service.Domain.Dtos
{
    [QueueName("orders-queue")]
    public class OrderDto : IQueueMessage
    {
        public OrderDto()
        {

        }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public Guid MessageId { get; set; }
        [JsonIgnore]
        public TimeSpan TimeToLive { get; set; } = TimeSpan.FromSeconds(1);
    }
}
