namespace Wtdl.Repository.Data
{
    public class YMDGroupByCount
    {
        public YMDGroupByCount(string Year, string Month, string Day, int Count)
        {
            this.Year = Year;
            this.Month = Month;
            this.Day = Day;
            this.Count = Count;
        }

        public YMDGroupByCount()
        {
        }

        public override string ToString()
        {
            return $"{{ Year = {Year}, Month = {Month}, Day = {Day} }}";
        }

        public string Year { get; init; }
        public string Month { get; init; }
        public string Day { get; init; }
        public double Count { get; init; }
        public string OrderNumbels { get; set; }
    }
}