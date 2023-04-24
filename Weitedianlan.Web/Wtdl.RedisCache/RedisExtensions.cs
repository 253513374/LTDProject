using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Wtdl.RedisCache
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, string redisconnectionString)
        {
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = connectionString;
            //    options.InstanceName = "Wtdl";
            //});
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisconnectionString));
            services.AddScoped<IRedisCache, RedisCacheService>();

            return services;
        }
    }
}