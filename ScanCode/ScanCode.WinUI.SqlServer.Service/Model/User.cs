namespace ScanCode.WinUI.Service
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tUser")]
    public partial class User
    {
        //public int ID { get; set; }

        //[StringLength(20)]
        //public string UserID { get; set; }

        //[StringLength(50)]
        //public string UserName { get; set; }

        //[StringLength(50)]
        //public string PWD { get; set; }

        //[StringLength(20)]
        //public string AgentID { get; set; }

        //public int? Flag { get; set; }
        public int ID { set; get; }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public string PWD { get; set; }
        public string AgentID { get; set; }
        public int Flag { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}