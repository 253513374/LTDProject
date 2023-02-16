using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABenNetCore.Redis.Xunit
{
    public class RedisClientFactory
    {
        /// <summary>
        /// 连接字符串，一般写在配置文件里面
        /// </summary>
        //private static readonly string ConnectionString = "127.0.0.1:16379,password=123456,connectTimeout=1000,connectRetry=1,syncTimeout=10000";

        /// <summary>
        /// 上锁，单例模式
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 连接对象
        /// </summary>
        private static IConnectionMultiplexer _connection;

        /// <summary>
        /// 获取并发链接管理器对象
        /// </summary>
        private static IConnectionMultiplexer _redis;

        public static IConnectionMultiplexer RedisInstance(string connectionstring)
        {
            if (_redis == null)
            {
                lock (locker)
                {
                    _redis = _redis ?? CreateConnection(connectionstring);
                    return _redis;
                }
            }
            return _redis;
        }

        public static IDatabase GetDatabase()
        {
            return _redis.GetDatabase();
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        private static IConnectionMultiplexer CreateConnection(string connectionstring)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection;
            }
            lock (locker)
            {
                if (_connection != null && _connection.IsConnected)
                {
                    return _connection;
                }

                if (_connection != null)
                {
                    _connection.Dispose();
                }
                _connection = ConnectionMultiplexer.Connect(connectionstring);
            }
            return _connection;
        }
    }
}