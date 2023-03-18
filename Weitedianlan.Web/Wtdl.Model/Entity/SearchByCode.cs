using System;

namespace Wtdl.Model.Entity
{
    public class SearchByCode
    {
        public int Id { get; set; }
        public string LabelNum { get; set; } = "";
        public string Results { get; set; } = "";
        public string FirstQueryTime { get; set; } = "";
        public string Lang { get; set; } = "";
        public string Info { get; set; } = "";
        public string? ImgUrl { get; set; }
        public string ProductName { get; set; } = "";
        public string CorpName { get; set; } = "";
        public string ProductVoice { get; set; } = "";
        public string FibreColor { get; set; } = "";
        public string QueryCount { get; set; } = "";
        public string? WINNERPRODUCT { set; get; } = "";
        public string Email { set; get; } = "";
        public string UserName { set; get; } = "";
        public DateTime? QueryTime { get; set; }
        public string? FIELD1 { set; get; } = "";
    }
}