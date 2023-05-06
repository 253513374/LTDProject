using ScanCode.Share;
using ScanCode.WinUI.model.ReQuest;
using ScanCode.WinUI.Service;

namespace ScanCode.WinUI
{
    public class ScanCodeViewModel
    {
        private WtdlSqlService WtdlSqlService { set; get; }
        private GroupedBdxOrder GroupedBdxOrder { get; set; }

        // public ObservableCollection<tLabelsxModel> addtLabelxes;// = new ObservableCollection<tLabelsxModel>();
        public ScanCodeViewModel(GroupedBdxOrder GroupedBdxOrder, WtdlSqlService WtdlSqlService)
        {
            this.WtdlSqlService = WtdlSqlService;
            this.GroupedBdxOrder = (GroupedBdxOrder)GroupedBdxOrder;

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
        //        OrderTime = DateTime.Parse(this.GroupedBdxOrder.DDRQ),
        //        Dealers = WtdlSqlService.GetAgentId(this.GroupedBdxOrder.KH),
        //        DealersName = this.GroupedBdxOrder.KH,
        //        Adminaccount = "",
        //        ExtensionName = "",
        //        OrderNumbels = this.GroupedBdxOrder.DDNO,

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