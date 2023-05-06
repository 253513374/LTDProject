namespace ScanCode.WPF.HubServer.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class W_LabelStorage
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string QRCode { get; set; }

        public DateTime OrderTime { get; set; }

        public DateTime OutTime { get; set; }

        [StringLength(20)]
        public string Dealers { get; set; }

        [StringLength(20)]
        public string Adminaccount { get; set; }

        [StringLength(20)]
        public string OutType { get; set; }

        [Required]
        [StringLength(20)]
        public string OrderNumbels { get; set; }

        [StringLength(20)]
        public string ExtensionName { get; set; }

        [StringLength(20)]
        public string ExtensionOrder { get; set; }
    }
}