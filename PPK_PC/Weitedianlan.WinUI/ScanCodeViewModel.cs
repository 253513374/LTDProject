using Weitedianlan.model.ReQuest;
using Weitedianlan.SqlServer.Service;

namespace Weitedianlan.WinUI
{
    public class ScanCodeViewModel
    {
        private WtdlSqlService WtdlSqlService { set; get; }
        private RequestOrder requestOrder { get; set; }

        // public ObservableCollection<tLabelsxModel> addtLabelxes;// = new ObservableCollection<tLabelsxModel>();
        public ScanCodeViewModel(RequestOrder requestOrder, WtdlSqlService WtdlSqlService)
        {
            this.WtdlSqlService = WtdlSqlService;
            this.requestOrder = (RequestOrder)requestOrder;

            //addtLabelxes = new ObservableCollection<tLabelsxModel>() {
            //     new tLabelsxModel { QRCode="000000" , OrderNumbel="111111", ResultStatus ="111", Aname="456456", },
            //      new tLabelsxModel { QRCode="000000" , OrderNumbel="111111", ResultStatus ="111", Aname="456456", },
            //       new tLabelsxModel { QRCode="000000" , OrderNumbel="111111", ResultStatus ="111", Aname="456456", }
            //};
        }

        //public void SanCodeAddcommand(string sancode)
        //{
        //    var AddtLabelx = new AddtLabelx
        //    {
        //        QRCode = sancode,
        //        OrderTime = DateTime.Parse(this.requestOrder.DDRQ),
        //        Dealers = WtdlSqlService.GetAgentId(this.requestOrder.KH),
        //        DealersName = this.requestOrder.KH,
        //        Adminaccount = "",
        //        ExtensionName = "",
        //        OrderNumbels = this.requestOrder.DDNO,

        //        OutType = "THFX",
        //    };
        //    tLabelsxModel remodel = WtdlSqlService.AddtLabelX(AddtLabelx);
        //    if (remodel.ResulCode == 200)
        //    {
        //        //addtLabelxes.Insert(0, remodel);
        //    }

        //}

        public void Add(AddtLabelx addtLabelx)
        {
        }
    }
}