using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Service.Integration.Test.Utilities
{
    public class RequiresRabbitFactAttribute : FactAttribute
    {
        private static readonly Lazy<bool> IsAvailable = new Lazy<bool>(() =>
        {
            var factory = new ConnectionFactory
            {
                HostName = TestBase.Host,
                RequestedConnectionTimeout = DateTime.Now.AddMinutes(1).TimeOfDay
            };

            try
            {
                using (var connection = factory.CreateConnection())
                    return connection.IsOpen;
            }
            catch (Exception)
            {
                return false;
            }
        });

        public override string Skip
        {
            get { return IsAvailable.Value ? "" : "RabbitMQ is not available"; }
            set { /* nothing */ }
        }
    }
}
