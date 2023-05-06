namespace ScanCode.WinUI.Service
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tLabels_Re
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string XCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string PCode { get; set; }

        [StringLength(50)]
        public string PName { get; set; }

        [StringLength(20)]
        public string bzStock { get; set; }

        [StringLength(20)]
        public string bzPt { get; set; }

        public DateTime? bzDate { get; set; }

        [StringLength(20)]
        public string bzPici { get; set; }

        [StringLength(200)]
        public string Content { get; set; }

        public DateTime? fhOriginalDate1 { get; set; }

        public DateTime? fhDate1 { get; set; }

        public DateTime? fhReDate1 { get; set; }

        [StringLength(20)]
        public string fhRept1 { get; set; }

        [StringLength(20)]
        public string fhAgent1 { get; set; }

        [StringLength(20)]
        public string fhPt1 { get; set; }

        [StringLength(20)]
        public string fhStock1 { get; set; }

        [StringLength(10)]
        public string fhType1 { get; set; }

        [StringLength(20)]
        public string fhPaper1 { get; set; }

        [StringLength(200)]
        public string Content1 { get; set; }

        public DateTime? fhDate2 { get; set; }

        [StringLength(20)]
        public string fhAgent2 { get; set; }

        [StringLength(20)]
        public string fhPt2 { get; set; }

        [StringLength(20)]
        public string fhStock2 { get; set; }

        [StringLength(1)]
        public string fhType2 { get; set; }

        [StringLength(20)]
        public string fhPaper2 { get; set; }

        [StringLength(200)]
        public string Content2 { get; set; }

        public DateTime? fhDate3 { get; set; }

        [StringLength(20)]
        public string fhAgent3 { get; set; }

        [StringLength(20)]
        public string fhPt3 { get; set; }

        [StringLength(20)]
        public string fhStock3 { get; set; }

        [StringLength(1)]
        public string fhType3 { get; set; }

        [StringLength(20)]
        public string fhPaper3 { get; set; }

        [StringLength(200)]
        public string Content3 { get; set; }

        [StringLength(10)]
        public string chPt { get; set; }

        public DateTime? chDate { get; set; }

        [StringLength(50)]
        public string chAddr { get; set; }
    }
}