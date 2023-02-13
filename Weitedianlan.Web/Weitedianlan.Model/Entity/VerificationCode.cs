using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weitedianlan.Model.Entity
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