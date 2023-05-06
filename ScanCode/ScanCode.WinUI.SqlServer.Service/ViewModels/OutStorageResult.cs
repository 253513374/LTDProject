namespace ScanCode.WinUI.Service
{
    public class OutStorageResult
    {
        public bool Successed { get; set; }
        public int Count { get; set; }

        public string QRCode { get; set; }

        public string Message { get; set; }

        public static OutStorageResult Success(int count, string qrCode)
        {
            return new OutStorageResult()
            {
                Successed = true,

                Count = count,
                QRCode = qrCode
            };
        }

        public static OutStorageResult Fail(string message, string qrCode)
        {
            return new OutStorageResult()
            {
                Successed = false,
                Message = message,
                QRCode = qrCode
            };
        }
    }
}