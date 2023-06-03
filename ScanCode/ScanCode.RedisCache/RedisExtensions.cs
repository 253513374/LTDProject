using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ScanCode.RedisCache
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, string redisconnectionString)
        {
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = connectionString;
            //    options.InstanceName = "ScanCode";
            //});
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisconnectionString));
            services.AddTransient<IRedisCache, RedisCacheService>();

            return services;
        }
    }
}