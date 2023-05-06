using CsvHelper.Configuration.Attributes;
using System;

namespace ScanCode.Model.Entity
{
    public class VerificationCode : IEntityBase
    {
        public int Id { get; set; }

        /// <summary>
        /// 防伪编码
        /// </summary>
        [Index(0)]
        public string QRCode { get; set; }

        /// <summary>
        /// 4位验证码
        /// </summary>
        [Index(1)]
        public string Captcha { get; set; }

        public string FileHash { get; set; }
    }
}