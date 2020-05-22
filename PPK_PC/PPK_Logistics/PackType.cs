using System.Collections.ObjectModel;

namespace PPK_Logistics
{
    public class PackType : ObservableCollection<string>
    {
        public PackType()
        {
            Add("选择单个产品包装");
            Add("选择入库单号包装");
        }
    }
}