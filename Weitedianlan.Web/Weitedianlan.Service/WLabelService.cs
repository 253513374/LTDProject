using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Request;
using Weitedianlan.Model.Response;

namespace Weitedianlan.Service
{
    public class WLabelService
    {
        public static DateTime _DateTime = DateTime.Now;

        public static List<WlabelCount> WlabelStatic;

        private Db _Db;

        public WLabelService(Db db)
        {
            _Db = db;
            WlabelStatic = new List<WlabelCount>(1000);
        }

        public ResponseModel GetWLabelServiceList(DateTime dateTime, string selectId = "0", string datetimestr = "")
        {
            try
            {
                DateTime DateTimeMax = DateTime.Now;
                if (datetimestr != "")
                {
                    if (datetimestr.Contains('/')) datetimestr = datetimestr.Replace('/', '-').Trim();
                    dateTime = DateTime.ParseExact(datetimestr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

                    DateTimeMax = dateTime.Date.AddDays(int.Parse(selectId));
                    DateTimeMax = DateTimeMax.AddHours(23);
                    DateTimeMax = DateTimeMax.AddMinutes(59);
                    DateTimeMax = DateTimeMax.AddSeconds(59);
                }
                else
                {
                    var KeyID = _Db.W_LabelStorage.AsNoTracking().OrderByDescending(o => o.ID).Select(s => s.ID).Max();
                    dateTime = _Db.W_LabelStorage.Find(KeyID).OutTime.Date;

                    DateTimeMax = dateTime.Date.AddDays(int.Parse(selectId));
                    DateTimeMax = DateTimeMax.AddHours(23);
                    DateTimeMax = DateTimeMax.AddMinutes(59);
                    DateTimeMax = DateTimeMax.AddSeconds(59);
                }

                var Wlabel = _Db.W_LabelStorage.AsNoTracking().Where(s => s.OutTime.Date >= dateTime && s.OutTime <= DateTimeMax).Join(_Db.Agent, w => w.Dealers, a => a.AID, (a, w) => new
                {
                    a.OrderNumbels,
                    a.OrderTime,
                    a.OutTime,
                    a.ExtensionName,
                    w.AName
                }).OrderByDescending(c => c.OutTime).ToList();

                var responsemodel = new ResponseModel()
                {
                    Code = 200,
                    Status = "出库集合获取成功",
                    QrcodeDataCount = Wlabel.Count,
                    Data = new List<WlabelCount>()
                };

                var WlabelCount = from p in Wlabel
                                  group p by new { p.OrderNumbels, p.AName, p.OrderTime } into g
                                  select new WlabelCount
                                  {
                                      Aname = g.Key.AName.Trim(),
                                      OrderNumbels = g.Key.OrderNumbels.Trim(),
                                      OrderTime = g.Key.OrderTime.ToString().Trim(),
                                      Count = g.Count().ToString()
                                  };
                responsemodel.Data = WlabelCount;
                responsemodel.DataCount = WlabelCount.Count();
                return responsemodel;
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Code = 404,
                    Status = ex.Message,
                    Data = new List<WlabelCount>()
                };
            }
        }

        public ResponseModel GetDetails(string ordernumbel)
        {
            string sql = string.Format(@"SELECT w.ID, w.Adminaccount, w.Dealers, w.ExtensionName,w.ExtensionOrder, w.OrderNumbels, w.OrderTime, w.OutTime, w.OutType, w.QRCode FROM W_LabelStorage AS w WHERE w.OrderNumbels = '{0}'", ordernumbel);

            var Wcount = _Db.W_LabelStorage.FromSql(sql).ToList();
            //var Wcount = (from a in _Db.W_LabelStorage
            //           where a.OrderNumbels.Trim() == ordernumbel.Trim() select(new W_LabelStorage {
            //               ID = a.ID,
            //               OrderNumbels = a.OrderNumbels,
            //               OrderTime = a.OrderTime,
            //               OutTime = a.OutTime,
            //               OutType = a.OutType,
            //               QRCode = a.QRCode,
            //               Adminaccount = a.Adminaccount,
            //               Dealers = a.Dealers,
            //               ExtensionName = a.ExtensionName
            //           })).ToList();

            if (Wcount != null && Wcount.Count > 0)
            {
                var re = new ResponseModel()
                {
                    Code = 200,
                    Status = "详细信息获取失败",
                    Data = new List<W_LabelStorage>()
                };

                re.Data = Wcount;
                return re;
            }
            else
            {
                return new ResponseModel()
                {
                    Code = 400,
                    Status = "找不到相关信息，请刷新列表",
                    Data = new List<WlabelCount>()
                };
            }
        }

        public ResponseModel GetQuerys(string qrcodeid, string orderid, out int total)
        {
            try
            {
                var Wcount = new List<W_LabelStorage>(2000);

                if (!string.IsNullOrEmpty(qrcodeid))
                {
                    string sqlqr = string.Format(@"SELECT w.ID, w.Adminaccount, w.Dealers, w.ExtensionName,w.ExtensionOrder, w.OrderNumbels, w.OrderTime, w.OutTime, w.OutType, w.QRCode FROM W_LabelStorage AS w WHERE w.QRCode = '{0}'", qrcodeid);
                    Wcount = _Db.W_LabelStorage.FromSql(sqlqr).ToList();

                    var response = new ResponseModel
                    {
                        Code = 200,
                        Status = "查询成功"
                    };
                    response.Data = new List<W_LabelStorage>(3000);
                    response.Data = Wcount;
                    total = Wcount.Count();
                    //var query = list.OrderByDescending(o => o.ID).ToList();
                    //response.Data = list.ToList();
                    return response;
                }
                if (!string.IsNullOrEmpty(orderid))
                {
                    string sqlor = string.Format(@"SELECT w.ID, w.Adminaccount, w.Dealers, w.ExtensionName,w.ExtensionOrder, w.OrderNumbels, w.OrderTime, w.OutTime, w.OutType, w.QRCode FROM W_LabelStorage AS w WHERE w.OrderNumbels = '{0}'", orderid);
                    Wcount = _Db.W_LabelStorage.FromSql(sqlor).ToList();
                    var response = new ResponseModel
                    {
                        Code = 200,
                        Status = "查询成功"
                    };
                    response.Data = new List<W_LabelStorage>(3000);
                    response.Data = Wcount;
                    total = Wcount.Count();
                    //var query = list.OrderByDescending(o => o.ID).ToList();
                    //response.Data = list.ToList();
                    return response;
                }
                total = 0;
                return new ResponseModel
                {
                    Code = 400,
                    Status = "没有查询条件",
                    Data = new List<W_LabelStorage>()
                };
            }
            catch (Exception ex)
            {
                total = 0;
                return new ResponseModel
                {
                    Code = 404,
                    Status = ex.Message,
                    Data = new List<W_LabelStorage>()
                };
            }
        }
    }
}