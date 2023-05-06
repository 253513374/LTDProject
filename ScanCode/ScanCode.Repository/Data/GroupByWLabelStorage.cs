namespace ScanCode.Repository.Data
{
    public class GroupByWLabelStorage
    {
        public GroupByWLabelStorage()
        { }

        public string ID { set; get; }
        public string OrderNumbels { set; get; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { set; get; }

        public double Count { set; get; }

        public string AgentName { set; get; }

        public DateTime Time { set; get; }
    }
}