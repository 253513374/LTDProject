using Weitedianlan.model.ReQuest;
using Weitedianlan.SqlServer.Service;

namespace Weitedianlan.WinUI
{
    public class ScanCodeReturnViewModel
    {
        private WtdlSqlService WtdlSqlService { set; get; }
        private RequestOrder requestOrder { get; set; }

        public ScanCodeReturnViewModel(RequestOrder requestOrder)
        {
            this.WtdlSqlService = WtdlSqlService.TryGetSqlService();
            this.requestOrder = requestOrder;
        }

        public void SanCodeDeletecommand(AddtLabelx addtLabelx)
        {
        }
    }
}