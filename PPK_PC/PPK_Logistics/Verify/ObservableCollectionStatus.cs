using PPK_Logistics.DataSource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace PPK_Logistics.Verify
{
    public static class ObservableCollectionEx
    {
        public static bool VerifyData( this ObservableCollection<Checkin_ltjy> PackData_Simpl,string _code)
        {
            Checkin_ltjy _Checkin_ltjy = PackData_Simpl.Where(F => F.barcode == _code).First();

            if (_Checkin_ltjy != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool VerifyDataD(this ObservableCollection<Checkin_ltjyStack> ListCheckin_ltjyStack,string _code)
        {
            Checkin_ltjyStack _Checkin_ltjyStack=ListCheckin_ltjyStack.Where(F=>F.CRIB== _code).First();

            if(_Checkin_ltjyStack!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool MergeOutOrder(this ObservableCollection<OutboundOrder> _OutboundOrder, List<OutboundOrder> dbck,List<OutboundOrder> xsck)
        {
            dbck.AddRange(xsck);

            _OutboundOrder = new ObservableCollection<OutboundOrder>(dbck);
            return true;
        }

        public static OutboundOrder GetOutOrderFind(this ObservableCollection<OutboundOrder> _OutboundOrder,string strorder)
        {
            return _OutboundOrder.ToList().Find(F=>F.KCSWZ_SWKCBH1== strorder);
        }

        public static bool UpDataOutSCount(this ObservableCollection<OutboundOrder> _OutboundOrder,string ckbh)
        {
            try {
                DataTable _datatable = new PDAServer().MarketOutSCount(ckbh);
                if (_datatable != null && _datatable.Rows.Count > 0)
                {
                    _OutboundOrder.Where(F => F.KCSWZ_SWKCBH1.Contains(ckbh)).First().KCSWZMX_SFSL1 = _datatable.Rows[0]["yfsl"].ToString();
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static ObservableCollection<OutboundOrder> GetOutOrderStatus_Not(this ObservableCollection<OutboundOrder> _OutboundOrder)
        {
            try
            {
                return new ObservableCollection<OutboundOrder>( _OutboundOrder.Where(F => Convert.ToInt32(F.KCSWZMX_SFSL1)<= 0));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ObservableCollection<OutboundOrder> GetOutOrderStatus_Yet(this ObservableCollection<OutboundOrder> _OutboundOrder)
        {
            try
            {
                return new ObservableCollection<OutboundOrder>(_OutboundOrder.Where(F => Convert.ToInt32(F.KCSWZMX_SFSL1) > 0));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int GetSendStatusCheckout(this ObservableCollection<Checkout> _ListCheckout, string _Status)
        {
            IEnumerable<Checkout> items = from objdataitem in _ListCheckout
                                   where objdataitem.STATUS.ToString().Contains(_Status)
                                   select objdataitem;
            return items.ToList().Count;
        }

        public static bool RemoveCheckout(this ObservableCollection<Checkout> _ListCheckout, string code)
        {
            try {
                IEnumerable<Checkout> items = from objdataitem in _ListCheckout
                                              where objdataitem.BRACODE.ToString().Contains(code)
                                              select objdataitem;

                foreach (Checkout reout in items.ToList())
                {
                    _ListCheckout.Remove(reout);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int GetSendStatusPackDataDelete(this ObservableCollection<PackDataDelete> _ListCheckout, string _Status)
        {
            IEnumerable<PackDataDelete> items = from objdataitem in _ListCheckout
                                          where objdataitem.STATUS.ToString().Contains(_Status)
                                          select objdataitem;
            return items.ToList().Count;
        }
    }
}