using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Base.Contstants;
using RabbitMQ.Kernel.Builders;
using RabbitMQ.Kernel.Consumer;
using RabbitMQ.Kernel.Implementation;
using RabbitMQ.Kernel.Implementation.QueueProducerStrategies;
using RabbitMQ.Kernel.Implementation.QueueProviderStrategies;
using RabbitMQ.Kernel.QueueProviderStrategies;
using Stock.Service.Application.Consumers;
using Stock.Service.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Kernel.QueueingStartupInjection;

namespace Stock.Service.Application.DependencyInjection
{
    public static class RabbitMQConsumernjection
    {
        public static IServiceCollection AddAddQueueMessageConsumer(this IServiceCollection services)
        {

            var headerqueue = new QueueBuilder<OrderDto>()
               .SetQueue("orders.header.queue")
               .SetQueueType(Strategy.Header)
               .WithHeaders(new Dictionary<string, object>
               {
                    { "x-match", "all" },
                    { "event", "OrderCreated" }
               })
               .Build();


            var streamqueue = new QueueBuilder<OrderDto>()
                .SetQueue("new stream channel")
                .SetQueueType(Strategy.Stream)
                .Build();


            var fanoutqueue = new QueueBuilder<OrderDto>()
                .SetQueue("new fanout channel")
                .SetQueueType(Strategy.Fanout)
                .Build();

           

            services.AddQueueMessageConsumer<OrderCosumer, OrderDto>(headerqueue);

            services.AddQueueMessageConsumer<OrderCosumer, OrderDto>(streamqueue);


            services.AddQueueMessageConsumer<OrderCosumer, OrderDto>(fanoutqueue);

            return services;
        }

    }
}
