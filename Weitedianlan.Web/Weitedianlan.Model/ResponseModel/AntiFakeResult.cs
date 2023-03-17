using Wtdl.Model.Entity;

namespace Wtdl.Model.ResponseModel
{
    /// <summary>
    /// 防伪查询结果
    /// </summary>
    public class AntiFakeResult
    {
        /// <summary>
        /// 查询状态
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 查询信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 防伪数据
        /// </summary>
        public SearchByCode AntiFakeByData { get; set; }
    }
}