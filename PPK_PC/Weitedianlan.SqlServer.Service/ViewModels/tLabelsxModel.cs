namespace Weitedianlan.SqlServer.Service
{
    public class tLabelsxModel : ViewModelBase
    {
        private string qRCode;

        // public string qRCode { get => qRCode1; set => qRCode1 = value; }
        private string aname;

        private string orderNumbel;
        private int resulCode;
        private string resultStatus;

        private string errorinfo;

        private W_LabelStorage tlabels_X = new W_LabelStorage();

        public string QRCode { set => qRCode = value; get => qRCode; }
        public string Aname { set => aname = value; get => aname; }
        public string OrderNumbel { set => orderNumbel = value; get => orderNumbel; }
        public int ResulCode { set => resulCode = value; get => resulCode; }
        public string ResultStatus { set => resultStatus = value; get => resultStatus; }
        public W_LabelStorage tLabels_X { set => tlabels_X = value; get => tlabels_X; }
        public string Errorinfo { get => errorinfo; set => errorinfo = value; }
    }
}