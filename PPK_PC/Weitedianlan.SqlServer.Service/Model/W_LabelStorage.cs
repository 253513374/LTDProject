namespace Weitedianlan.SqlServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
