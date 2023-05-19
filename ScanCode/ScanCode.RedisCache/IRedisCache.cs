namespace ScanCode.RedisCache
{
    public interface IRedisCache
    {
        void AppendData(string key, string data);

        Task<T?> GetObjectAsync<T>(string key);

        /// <summary>
        /// 使用位图缓存数据出库状态，定的 qrcode 存在于 Redis 位图中，设置为true。
        /// </summary>
        /// <param name="qrcode"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<bool> SetBitAsync(string qrcode, bool bit = true);

        /// <summary>
        /// 使用位图缓存数据出库状态，qrcode 存在于 Redis 位图中， 设置为true。
        /// </summary>
        /// <param name="qrcode"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task SetBulkBitAsync(List<string> qrcodesList);

        /// <summary>
        /// 在位图中查找标签数据，如果给定的 qrcode 存在于 Redis 位图中，返回 true。
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        Task<bool> GetBitAsync(string qrcode);

        Task SetObjectAsync(string key, object data, TimeSpan? expiry = null);

        Task<bool> SetRedPacketAsync(string qrcode);
    }
}