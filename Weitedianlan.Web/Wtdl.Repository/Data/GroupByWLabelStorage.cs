namespace Wtdl.Repository.Data
{
    public class GroupByWLabelStorage
    {
        public GroupByWLabelStorage()
        { }

        public string ID { set; get; }
        public string OrderNumbels { set; get; }

        public int Count { set; get; }

        public string AgentName { set; get; }

        public DateTime Time { set; get; }
    }
}