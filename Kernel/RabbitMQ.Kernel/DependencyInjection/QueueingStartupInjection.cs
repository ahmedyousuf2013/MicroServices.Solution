using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Base.Contstants;
using RabbitMQ.Kernel.Base.QueueAttribute;
using RabbitMQ.Kernel.Builders;
using RabbitMQ.Kernel.Consumer;
using RabbitMQ.Kernel.Implementation;
using RabbitMQ.Kernel.Implementation.QueueProducerStrategies;
using RabbitMQ.Kernel.Implementation.QueueProviderStrategies;
using RabbitMQ.Kernel.QueueProviderStrategies;

namespace RabbitMQ.Kernel.QueueingStartupInjection
{
    public static class QueueingStartupInjection
    {

        public static void AddQueueing(this IServiceCollection services, IConfiguration configuration )
        {
            services.AddSingleton<ConnectionFactory>(provider =>
            {
                var MQSereversettings = configuration.GetSection("MQServerConnSettings").Get<MQServerConnSettings>();
                var factory = new ConnectionFactory
                {
                    UserName = MQSereversettings.User,
                    Password = MQSereversettings.Password,
                    HostName = MQSereversettings.Host,
                    Port = MQSereversettings.Port,
                    DispatchConsumersAsync = true,

                    // Configure the amount of concurrent consumers within one host
                    //   ConsumerDispatchConcurrency = settings.RabbitMqConsumerConcurrency.GetValueOrDefault(),
                };

                return factory;
            });

            // The RabbitMQ documentation states that Connections are meant to be long lived
            // and should be used to perform all operations. We chose to implement the connection as a Singleton to ensure that.
            // In case of high concurrent usage multiple connections could be used, but for most usage one connection per host will be sufficient
            // See https://www.rabbitmq.com/dotnet-api-guide.html#connecting and https://www.rabbitmq.com/dotnet-api-guide.html#concurrency-thread-usage
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();

            // The RabbitMQ documentation states that IModels (or Channels) should not be used between threads simultaniously.
            // When using transactions in the consumers, giving each scope its own Channel will insure reliability in the processing of a Queue message
            // See https://www.rabbitmq.com/dotnet-api-guide.html#concurrency-channel-sharing
            services.AddScoped<IChannelProvider, ChannelProvider>();

            services.AddScoped<IQueueProducerStrategyFactory, QueueProducerStrategyFactory>();

            services.AddScoped<IChannelProviderStrategyFactory, ChannelProviderStrategyFactory>();
            services.AddScoped(typeof(IQueueChannelProvider<>), typeof(DefaultQueueChannelProvider<>));

            services.AddScoped(typeof(IQueueProducerBuilder<>), typeof(QueueProducerBuilder<>));


            services.AddScoped(typeof(IQueueProducer<>), typeof(DefaultQueueProducer<>));


            services.AddScoped(typeof(DefaultQueueProducer<>));

            services.AddScoped(typeof(DefaultQueueChannelProvider<>));


            services.AddScoped(typeof(DirectQueueProducer<>));


            services.AddScoped(typeof(DirectQueueChannnelProvider<>));

            services.AddScoped(typeof(HeaderQueueProducer<>));

            services.AddScoped(typeof(HeaderQueueChannelProvider<>));

            services.AddScoped(typeof(FanoutQueueProducer<>));

            services.AddScoped(typeof(FanoutQueueChannelProvider<>));

            services.AddScoped(typeof(TopicQueueProducer<>));

            services.AddScoped(typeof(TopicQueueChannelProvider<>));


            services.AddScoped(typeof(StreamQueueProducerr<>));

            services.AddScoped(typeof(StreamQueueChannelProvider<>));


            services.AddScoped<IChannelProvider, ChannelProvider>();

            services.AddScoped(typeof(IQueueChannelProvider<>), typeof(DefaultQueueChannelProvider<>));

            services.AddScoped(typeof(IQueueProducer<>), typeof(DefaultQueueProducer<>));

            services.AddSingleton<IQueueNameResolver, QueueNameResolver>();

        }
        public static void AddQueueMessageConsumer<TMessageConsumer, TQueueMessage>(this IServiceCollection services, QueueDefinition rabbitmq) where TMessageConsumer : IQueueConsumer<TQueueMessage> where TQueueMessage : class, IQueueMessage
        {
            // تسجيل الـ Consumer نفسه (Scoped) لأنه يُطلب داخل الـ Handler
            // TryAddScoped تضمن عدم تكرار التسجيل إذا تم استدعاء الدالة عدة مرات (وهو أمر جيد هنا)
            services.TryAddScoped(typeof(TMessageConsumer));

            // ملاحظة هامة: قمنا بحذف تسجيل IQueueConsumerHandler من هنا
            // لأن الـ HostedService سيقوم بإنشائه بنفسه داخلياً

            // تسجيل الـ HostedService مع تمرير الـ rabbitmq object الخاص بهذه المرة
            services.AddSingleton<IHostedService>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<QueueConsumerRegistratorService<TMessageConsumer, TQueueMessage>>>();

                return new QueueConsumerRegistratorService<TMessageConsumer, TQueueMessage>(
                    logger,
                    sp,
                    rabbitmq // تمرير الإعدادات المحددة (Stream أو Fanout)
                );
            });
        }
    }
}
