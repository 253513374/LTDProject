using ScanCode.Share;
using ScanCode.WinUI.model.ReQuest;
using ScanCode.WinUI.Service;

namespace ScanCode.WinUI
{
    public class ScanCodeReturnViewModel
    {
        private WtdlSqlService WtdlSqlService { set; get; }
        private GroupedBdxOrder GroupedBdxOrder { get; set; }

        public ScanCodeReturnViewModel(GroupedBdxOrder GroupedBdxOrder)
        {
            this.WtdlSqlService = WtdlSqlService.TryGetSqlService();
            this.GroupedBdxOrder = GroupedBdxOrder;
        }

        public void SanCodeDeletecommand(AddtLabelx addtLabelx)
        {
        }
    }
}