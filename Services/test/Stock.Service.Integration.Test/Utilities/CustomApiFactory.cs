using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RabbitMQ.Client;
using Stock.Service.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Service.Integration.Test.Utilities
{
    public class CustomApiFactory : WebApplicationFactory<Program>
    {
        public SharedFixture SharedFixture;
        public CustomApiFactory(SharedFixture sharedFixture)
        {
            this.SharedFixture = sharedFixture;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {

                services.Remove<DbContextOptions<StockServiceContext>>();

                services.Remove<StockServiceContext>();

                // add back the container-based dbContext
                services.AddDbContext<StockServiceContext>(opts =>
                    opts.UseNpgsql(SharedFixture.DatabaseConnectionString));


                //remove and add rabbitmq config

                this.SharedFixture.rabbitMqContainer.GetConnectionString();

                services.Remove<ConnectionFactory>();

                services.AddSingleton<ConnectionFactory>(provider =>
                {
                    var factory = new ConnectionFactory
                    {
                        UserName = "guest",
                        Password = "guest",
                        HostName = this.SharedFixture.rabbitMqContainer.Hostname,
                        Port = this.SharedFixture.rabbitMqContainer.GetMappedPublicPort(5672),
                        DispatchConsumersAsync = true,
                    };
                    return factory;
                });
            });

        }
    }
}
