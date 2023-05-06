using ScanCode.Model.Entity;
using System;

namespace ScanCode.Model.ResponseModel
{
    /// <summary>
    /// 防伪查询结果
    /// </summary>
    public class AntiFakeResult : TResult
    {
        /// <summary>
        /// 查询状态
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 查询信息
        /// </summary>
        public string Message { get; set; }

        public int Id { get; set; }
        public string LabelNum { get; set; }
        public string Results { get; set; }
        public string FirstQueryTime { get; set; }
        public string Lang { get; set; }
        public string Info { get; set; }
        public string? ImgUrl { get; set; }
        public string ProductName { get; set; }
        public string CorpName { get; set; }
        public string ProductVoice { get; set; }
        public string FibreColor { get; set; }
        public string QueryCount { get; set; }
        public string? WINNERPRODUCT { set; get; }
        public string Email { set; get; }
        public string UserName { set; get; }
        public DateTime? QueryTime { get; set; }
        public string? FIELD1 { set; get; }

        public static AntiFakeResult Fail(string message, SearchByCode searchByCode = null)
        {
            return new AntiFakeResult()
            {
                IsSuccess = false,
                Message = message
            };
        }

        public static AntiFakeResult Success(SearchByCode searchByCode)
        {
            return new AntiFakeResult
            {
                IsSuccess = true,
                Message = "",
                Id = searchByCode.Id,
                LabelNum = searchByCode.LabelNum,
                Results = searchByCode.Results,
                FirstQueryTime = searchByCode.FirstQueryTime,
                Lang = searchByCode.Lang,
                Info = searchByCode.Info,
                ImgUrl = searchByCode.ImgUrl,
                ProductName = searchByCode.ProductName,
                CorpName = searchByCode.CorpName,
                ProductVoice = searchByCode.ProductVoice,
                FibreColor = searchByCode.FibreColor,
                QueryCount = searchByCode.QueryCount,
                WINNERPRODUCT = searchByCode.WINNERPRODUCT,
                Email = searchByCode.Email,
                UserName = searchByCode.UserName,
                QueryTime = searchByCode.QueryTime,
                FIELD1 = searchByCode.FIELD1
            };
        }
    }
}