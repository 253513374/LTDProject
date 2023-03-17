namespace   Wtdl.Model.ResponseModel
{
    public class PrizeResult
    {
        /// <summary>
        /// 奖品编号。
        /// </summary>
        public int Id { get; set; }//奖品ID

        /// <summary>
        /// 奖品名称
        /// </summary>
        public string Name { get; set; }//奖品名称

        /// <summary>
        /// 奖品描述
        /// </summary>
        public string Description { get; set; }//奖品描述

        /// <summary>
        /// 奖品图片
        /// </summary>
        public string ImageUrl { get; set; }//奖品图片

        public double Probability { get; set; }
    }
}