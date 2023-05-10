namespace ScanCode.WPF.HubServer.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class W_LabelStorage
    {
        public int ID { get; set; }

        public string QRCode { get; set; }

        public DateTime OrderTime { get; set; }

        public DateTime OutTime { get; set; }

        public string Dealers { get; set; }

        public string Adminaccount { get; set; }

        public string OutType { get; set; }

        public string OrderNumbels { get; set; }

        public string ExtensionName { get; set; }

        public string ExtensionOrder { get; set; }
    }
}