namespace Weitedianlan.SqlServer.Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tAgent")]
    public partial class tAgent
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string AID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string AName { get; set; }

        [StringLength(10)]
        public string AProvince { get; set; }

        [StringLength(30)]
        public string ACity { get; set; }

        [StringLength(500)]
        public string AAddr { get; set; }

        [StringLength(20)]
        public string ATel { get; set; }

        [StringLength(20)]
        public string APeople { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string ABelong { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AType { get; set; }

        public DateTime? datetiem { get; set; }
    }
}
