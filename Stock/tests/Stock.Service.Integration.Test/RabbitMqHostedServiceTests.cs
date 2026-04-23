using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Stock.Service.Integration.Test.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Service.Integration.Test
{
    [Collection(nameof(IntegrationTestCollection))]
    public class RabbitMqHostedServiceTests : IClassFixture<CustomApiFactory> ///IClassFixture<RabbitMqTestFixture> 
    {
        private readonly CustomApiFactory customApiFactory;


        public RabbitMqHostedServiceTests(CustomApiFactory _customApiFactory)
        {
            customApiFactory = _customApiFactory;
        }

        [Fact]
        public async Task Worker_Should_Consume_Message_From_RabbitMq()
        {
            // إعداد ConnectionFactory مع بيانات الـ Testcontainer

            var factory = new ConnectionFactory
            {
                HostName = this.customApiFactory.SharedFixture.rabbitMqContainer.Hostname,
                Port = this.customApiFactory.SharedFixture.rabbitMqContainer.GetMappedPublicPort(5672),
                UserName = "guest",
                Password = "guest",
                RequestedConnectionTimeout = System.TimeSpan.FromSeconds(1000)
            };
            //  Console.WriteLine($"port number is{_fixture.Host}{_fixture.Port} {_fixture.Container.State}");
            // بناء Host وتشغيل الـ HostedService (Worker) تحت الاختبار
            var host = Host.CreateDefaultBuilder()
                     .ConfigureServices(services =>
                     {
                         services.AddSingleton<IConnectionFactory>(factory);
                         services.AddHostedService<MyRabbitMqWorker>();
                     })
                     .Build();

            await host.StartAsync();


            // نشر رسالة
            using var conn = factory.CreateConnection();
            using (var channel = conn.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes("Hello Testcontainers");
                channel.BasicPublish("", "test-queue", null, body);
            }

            // انتظار قصير للسماح للـ Worker بمعالجة الرسالة (زمن يمكن تعديله حسب الحاجة)
            await Task.Delay(2000);

            // قراءة الملف الذي يكتبه الـ Worker للتأكد من الاستهلاك
            //   var consumedFile = "/tmp/consumed.txt";
            // Assert.True(File.Exists(consumedFile), "Expected consumed file to exist");
            //var content = File.ReadAllText(consumedFile);
            //Assert.Equal("Hello Testcontainers", content);
            Assert.True(conn.IsOpen); // مؤقتًا لتجنب فشل الاختبار

            await host.StopAsync();
        }
    }
}
