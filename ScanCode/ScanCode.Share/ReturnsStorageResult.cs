namespace ScanCode.Share
{
    public class ReturnsStorageResult
    {
        public string QrCode { set; get; }
        public string ReCount { set; get; }
        public int ResulCode { set; get; }
        public string ResultStatus { set; get; }
        public string Errorinfo { get; set; }

        public static ReturnsStorageResult Success(string qrCode)
        {
            return new ReturnsStorageResult()
            {
                QrCode = qrCode,

                ResulCode = 200,
                ResultStatus = "退货成功"
            };

            //return new ReturnsStorageResult
            //    { QrCode = tLabelxId, ReCount = i.ToString(), ResulCode = 200, ResultStatus = "退货成功" };
        }

        public static ReturnsStorageResult Fail(string qrCode, string errorinfo = "")
        {
            return new ReturnsStorageResult()
            {
                QrCode = qrCode,

                ResulCode = 400,
                ResultStatus = "退货失败",
                Errorinfo = errorinfo
            };

            //return new ReturnsStorageResult
            //    { QrCode = tLabelxId, ReCount = i.ToString(), ResulCode = 400, ResultStatus = "退货失败" };
        }

        public static ReturnsStorageResult Fail(string qrCode, int resulCode, string resultStatus, string errorinfo = "")
        {
            return new ReturnsStorageResult()
            {
                QrCode = qrCode,
                ResulCode = 400,
                ResultStatus = resultStatus,
                Errorinfo = errorinfo
            };
        }

        //异常
        public static ReturnsStorageResult Exception(string qrCode, string error)
        {
            return new ReturnsStorageResult()
            {
                QrCode = qrCode,
                ResulCode = 500,
                ResultStatus = "退货异常",
                Errorinfo = error
            };
        }

        /// <summary>
        /// 还未发货
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public static ReturnsStorageResult NotOutFail(string qrcode)
        {
            return new ReturnsStorageResult()
            {
                QrCode = qrcode,
                ResulCode = 400,
                ResultStatus = "还未发货"
            };
        }

        /// <summary>
        /// 网络不在线
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static ReturnsStorageResult Offline(string qrcode)
        {
            return new ReturnsStorageResult()
            {
                QrCode = qrcode,
                ResulCode = 400,
                ResultStatus = "网络不在线"
            };
            // throw new System.NotImplementedException();
        }
    }
}