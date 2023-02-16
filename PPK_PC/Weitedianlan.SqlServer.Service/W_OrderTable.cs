namespace Weitedianlan.SqlServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class W_OrderTable
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string OrderId { get; set; }

        public DateTime FinishTime { get; set; }

        [Required]
        [StringLength(20)]
        public string Accounts { get; set; }
    }
}
