using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Kernel.Base.Contracts;
using Stock.Service.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Service.Application.Consumers
{
    public class OrderCosumer : IQueueConsumer<OrderDto>
    {


        private readonly ILogger<OrderCosumer> logger;

        public OrderCosumer(ILogger<OrderCosumer> _logger)
        {
            logger = _logger;
        }
        public Task ConsumeAsync(OrderDto message)
        {
            logger.LogInformation(JsonConvert.SerializeObject(message));
            return Task.CompletedTask;
        }
    }
}
