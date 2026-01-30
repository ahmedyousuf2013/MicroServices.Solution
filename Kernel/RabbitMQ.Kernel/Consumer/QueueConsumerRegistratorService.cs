using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Base.Contstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Kernel.Consumer
{
    public class QueueConsumerRegistratorService<TMessageConsumer, TQueueMessage> : IHostedService
      where TMessageConsumer : IQueueConsumer<TQueueMessage>
      where TQueueMessage : class, IQueueMessage
    {
        private readonly ILogger<QueueConsumerRegistratorService<TMessageConsumer, TQueueMessage>> _logger;
        private readonly IServiceProvider _serviceProvider;
        private QueueDefinition _queueDefinition;
        private IQueueConsumerHandler<TMessageConsumer, TQueueMessage> _consumerHandler;
        private IServiceScope _scope;

        // استقبال الإعدادات هنا
        public QueueConsumerRegistratorService(
            ILogger<QueueConsumerRegistratorService<TMessageConsumer, TQueueMessage>> logger,
            IServiceProvider serviceProvider,
            QueueDefinition queueDefinition)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _queueDefinition = queueDefinition;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _scope = _serviceProvider.CreateScope();

            _consumerHandler = ActivatorUtilities.CreateInstance<QueueConsumerHandler<TMessageConsumer, TQueueMessage>>(
                _scope.ServiceProvider,
                _queueDefinition
            );

            _consumerHandler.RegisterQueueConsumer();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stop Service: Canceling {typeof(TMessageConsumer).Name}");

            _consumerHandler?.CancelQueueConsumer();
            _scope?.Dispose();

            return Task.CompletedTask;
        }
    }
}
