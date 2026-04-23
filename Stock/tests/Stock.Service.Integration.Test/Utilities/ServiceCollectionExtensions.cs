using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Service.Integration.Test.Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static void Remove<T>(this IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                 d => d.ServiceType == typeof(T));
            if (descriptor is not null) services.Remove(descriptor!);
        }
    }
}
