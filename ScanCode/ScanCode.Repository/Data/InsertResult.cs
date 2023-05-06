namespace ScanCode.Repository.Data
{
    /// <summary>
    /// 批量插入数据返回结果
    /// </summary>
    public class InsertResult
    {
        /// <summary>
        /// 插入的结果信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 插入的数据条数
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 重复数据
        /// </summary>
        public string FailedData { get; set; }

        /// <summary>
        /// 批量插入数据返回结果
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        public InsertResult()
        {
            Message = string.Empty;
            SuccessCount = 0;
            FailedData = string.Empty;
            TotalCount = 0;
            IsSuccess = false;
        }

        public InsertResult(string message, int successCount, string failedData, int totalCount, bool isSuccess)
        {
            Message = message;
            SuccessCount = successCount;
            FailedData = failedData;
            TotalCount = totalCount;
            IsSuccess = isSuccess;
        }
    }
}