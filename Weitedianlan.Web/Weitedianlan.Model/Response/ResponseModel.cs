namespace Weitedianlan.Model.Response
{
    public class ResponseModel
    {
        public int Code { set; get; }
        public int DataCount { set; get; }

        public int QrcodeDataCount { set; get; }
        public dynamic Data { set; get; }

        public string Status { set; get; }

        public string Error { set; get; }
    }
}