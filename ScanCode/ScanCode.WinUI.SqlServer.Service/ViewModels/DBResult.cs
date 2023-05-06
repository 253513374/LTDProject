namespace ScanCode.WinUI.Service
{
    public class DBResult<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Successed { get; set; }

        /// <summary>
        /// 操作信息
        /// </summary>
        public string Message { get; set; }

        public T Result { get; set; }

        /// <summary>
        /// 返回成功结果数据
        /// </summary>
        public static DBResult<T> Success(T value)
        {
            return new DBResult<T>
            {
                Successed = true,
                Message = "Success",

                Result = value
            };
        }

        ///返回失败信息
        public static DBResult<T> Fail(string message)
        {
            return new DBResult<T>
            {
                Successed = false,
                Message = message,
                Result = default
            };
        }
    }
}