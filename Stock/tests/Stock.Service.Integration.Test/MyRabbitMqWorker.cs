using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Service.Integration.Test
{
    public class MyRabbitMqWorker : IHostedService
    {
        private readonly IConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public MyRabbitMqWorker(IConnectionFactory factory)
        {
            _factory = factory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare("test-queue", durable: false, exclusive: false, autoDelete: false, null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                // File.WriteAllText("/tmp/consumed.txt", body);
            };

            _channel.BasicConsume("test-queue", autoAck: true, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel?.Close();
            _connection?.Close();
            return Task.CompletedTask;
        }
    }
}
