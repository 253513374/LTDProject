using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weitedianlan.Model.Enum;

namespace Weitedianlan.Model.Entity.Analysis
{
    public class CountAnalysis
    {
        public int Id { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 年出库总量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 统计类型
        /// </summary>
        public AnalysisType Type { get; set; }
    }
}