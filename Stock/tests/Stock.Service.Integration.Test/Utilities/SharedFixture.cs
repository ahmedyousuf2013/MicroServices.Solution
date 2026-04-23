using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Stock.Service.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Stock.Service.Integration.Test.Utilities
{
    public class SharedFixture : IAsyncLifetime
    {
        private StockServiceContext? _dbContext;

        private readonly PostgreSqlContainer _dbContainer =
            new PostgreSqlBuilder()
                .WithDatabase("RabittMQDatabse")
                .WithUsername("postgres")
                .WithPassword("P@ssw0rd")
                .Build();

        public string DatabaseConnectionString => _dbContainer.GetConnectionString();
        public StockServiceContext SuperHeroDbContext => _dbContext;


        public readonly RabbitMqContainer rabbitMqContainer = new RabbitMqBuilder()
               .WithImage("rabbitmq:3-management")
               .WithName($"test-rabbitmq-{System.Guid.NewGuid():N}")
               .WithEnvironment("RABBITMQ_DEFAULT_USER", "guest")
               .WithEnvironment("RABBITMQ_DEFAULT_PASS", "guest")
               .WithPortBinding(5672, true)      // expose AMQP port
           .WithPortBinding(15672, true)     // expose management UI
           .WithWaitStrategy(
              Wait.ForUnixContainer().UntilInternalTcpPortIsAvailable(5672)  // أهم خطوة
           )
               .Build();


        public async Task InitializeAsync()
        {
            await this.rabbitMqContainer.StartAsync();
            await _dbContainer.StartAsync();

            var optionsBuilder = new DbContextOptionsBuilder<StockServiceContext>()
                .UseNpgsql(DatabaseConnectionString);
            _dbContext = new StockServiceContext(optionsBuilder.Options);
            await _dbContext.Database.MigrateAsync();


        }

        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
            await this.rabbitMqContainer.DisposeAsync();
        }
    }
}
