namespace ScanCode.WPF.HubServer.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

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