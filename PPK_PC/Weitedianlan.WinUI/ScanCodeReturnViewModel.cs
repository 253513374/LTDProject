using Weitedianlan.model.ReQuest;
using Weitedianlan.SqlServer.Service;
using Wtdl.Share;

namespace Weitedianlan.WinUI
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