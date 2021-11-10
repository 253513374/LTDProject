using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Weitedianlan.model.ReQuest;
using Weitedianlan.model.Response;

namespace Weitedianlan.SqlServer.Service
{
    public class WtdlSqlService
    {

        public static User _usernnfo = new User();
        private WTDLEntityStrings DbEntities { set; get; }
        public WtdlSqlService(WTDLEntityStrings wTDLEntities) {

            DbEntities = wTDLEntities;
        }

        public string GetAgentId(string agentname)
        {
            var tAgentID = DbEntities.tAgents.AsNoTracking().Where(o => o.AName.Contains(agentname)).Select(s=>s.AID).FirstOrDefault();
            if(tAgentID != null)
            {
                return tAgentID.ToString();
            }
            else
            {
                return "";
            }
        }

        public ResponseModel GetUser()
        {
            var user = DbEntities.Users.AsNoTracking().ToList();

            ResponseModel responseModel = new ResponseModel();

            responseModel.code = 200;
            responseModel.result = "用户账户获取成功";
            responseModel.data = new List<User>();

            responseModel.data = user;

            return responseModel;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="addtLabelx"></param>
        /// <returns></returns>
        public  tLabelsxModel AddtLabelX(AddtLabelx addtLabelx)
        {

            tLabelsxModel tLabelsxModel = new tLabelsxModel();
            var tlabelx = new W_LabelStorage()
            {
                QRCode = addtLabelx.QRCode,
                OrderTime = addtLabelx.OrderTime,
                OutTime = DateTime.Now,
                Dealers = addtLabelx.Dealers,
                Adminaccount = addtLabelx.Adminaccount,
                OutType = addtLabelx.OutType,
                OrderNumbels = addtLabelx.OrderNumbels,
                ExtensionName = addtLabelx.ExtensionName,
            };
            try
            {
                //var re = DbEntities.tLabels_X.AsNoTracking().Where(w=>w.QRCode.Contains(addtLabelx.QRCode)).Select(s=>s.QRCode).FirstOrDefault();
                //if(re!=null)
                //{
                //    return AddtLabelsxModel(tlabelx, addtLabelx, 200, "重复发货");
                //}

                DbEntities.W_LabelStorage.Add(tlabelx);
                int i = DbEntities.SaveChanges();
                if (i > 0)
                {
                    return AddtLabelsxModel(tlabelx, addtLabelx, 200, "出库成功");
                }
                else
                {
                    return AddtLabelsxModel(tlabelx, addtLabelx, 400, "出库失败");
                }
            }
            catch (Exception ex)
            {
                string strinfo= ex.InnerException.InnerException.Message;
                return AddtLabelsxModel(tlabelx, addtLabelx,404,"系统错误",strinfo);
            }
        }

        public tLabelsxModel AddtLabelsxModel(W_LabelStorage w_LabelStorage, AddtLabelx addtLabelx,int ResulCode, string ResultStatus, string Errorinfo="")
        {
            tLabelsxModel tLabelsxModel = new tLabelsxModel();
            tLabelsxModel.QRCode = addtLabelx.QRCode;
            tLabelsxModel.Aname = addtLabelx.DealersName;
            tLabelsxModel.OrderNumbel = addtLabelx.OrderNumbels;
            tLabelsxModel.ResulCode = ResulCode;
            tLabelsxModel.ResultStatus = ResultStatus;
            tLabelsxModel.Errorinfo = Errorinfo;
            tLabelsxModel.tLabels_X = w_LabelStorage;
            return tLabelsxModel;
        }

        public bool DeeletetLabelX(tLabelsxModel addtLabelx)
        {

            var w_LabelStorage = DbEntities.W_LabelStorage.Local.FirstOrDefault(x=>x.QRCode.Contains(addtLabelx.QRCode));
            if (w_LabelStorage != null)
            {
                return DbEntities.W_LabelStorage.Local.Remove(addtLabelx.tLabels_X);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 记录已经完成得出库单
        /// </summary>
        public  async void AddOrder(W_OrderTable orderTable)
        {
            try
            {
                if (orderTable != null)
                {
                    W_OrderTable w_OrderTable= DbEntities.W_OrderTable.AsNoTracking().Where(w=>w.OrderId.Trim().Contains(orderTable.OrderId.Trim())).FirstOrDefault();

                    if(w_OrderTable==null)
                    {
                        DbEntities.W_OrderTable.Add(orderTable);
                        int ret = await DbEntities.SaveChangesAsync();
                    }

                    //    if(ret>0)
                    //    {
                    //        return new ResponseModel { code = 200, result = "单号添加成功", data = orderTable };
                    //    }
                    //    else
                    //    {
                    //        return new ResponseModel { code = 400, result = "单号添加失败", data = orderTable };
                    //    }
                }
                //else
                //{
                //    return new ResponseModel { code = 404, result = "单号数据为空", data = null };
                //}
            }
            catch (Exception ex )
            {

              //  throw;
            }
            //你的代码
           
            
        }

        public  List<W_OrderTable> FilterOrder()
        {

           return DbEntities.W_OrderTable.AsNoTracking().Where(w => w.FinishTime > DateTime.Now.AddDays(-7)).ToList();

        }

        public ResponseModel AddAgent(AddAgent addAgent)
        {
            var addAgentcode = DbEntities.tAgents.AsNoTracking().Where(o => o.AID.Trim()==addAgent.AID.Trim()).Select(s=>s.AID).ToList();
            if (addAgentcode.Count==0  )
            {
                var agent = new tAgent()
                {
                    AID = addAgent.AID,
                    AName = addAgent.AName,
                    ABelong = addAgent.ABelong,
                    AType = addAgent.AType
                };
                DbEntities.tAgents.Add(agent);
                int i = DbEntities.SaveChanges();
                if (i > 0)
                {
                    return new ResponseModel { code = 200, result = "进销商或客户添加成功", data = agent };
                }
                else
                {
                    return new ResponseModel { code = 400, result = "进销商或客户添加失败", data = agent };
                }
            }
            else
            {
                return new ResponseModel { code = 0, result = "已经存在" };
            }
           
        }
        /// <summary>
        /// 删除tLabelX   用于退货操作
        /// </summary>
        public SendBackMode DeletetLabelX(string tLabelxId)
        {

            //var tLabelxcode = DbEntities.W_LabelStorage.AsNoTracking().Where(o => o.QRCode == tLabelxId||o.OrderNumbels.Trim()== tLabelxId.Trim()).FirstOrDefault();

            var tLabelxcode = DbEntities.W_LabelStorage.Where(o => o.QRCode == tLabelxId || o.OrderNumbels == tLabelxId).ToList();

            try
            {
              
                if (tLabelxcode != null && tLabelxcode.Count==0)
                {
                    return new SendBackMode { QrCode = tLabelxId, ReCount = "0", ResulCode = 0, ResultStatus = "还未发货" };
                }
                DbEntities.W_LabelStorage.RemoveRange(tLabelxcode);
                int i = DbEntities.SaveChanges();
                if (i > 0)
                    return new SendBackMode { QrCode = tLabelxId, ReCount= i.ToString(), ResulCode = 200, ResultStatus = "退货成功" };
                return new SendBackMode { QrCode = tLabelxId, ReCount = i.ToString(), ResulCode = 400, ResultStatus = "退货失败" };
            }
            catch (Exception ex)
            {
                string strinfo = "AddtLabelX()：\n" + ex.InnerException;
                return new SendBackMode { QrCode = tLabelxId, ReCount = "0", ResulCode = 404, ResultStatus = "系统错误", Errorinfo= strinfo};
            }
                        
          
        }

        public bool DeeleteSendBackMode(SendBackMode  sendBackMode)
        {
            //var tLabelxcode = DbEntities.W_LabelStorage.Local.Where(o => o.QRCode == sendBackMode.QrCode).FirstOrDefault();
            //if(tLabelxcode!=null)
            //{
            //    return DbEntities.W_LabelStorage.Local.Remove(tLabelxcode);
            //}
            //else
            //{
                return false;
            //}
        }

        /// <summary>
        /// 获取tLabelX  出库单实际已经出库数量集合
        /// </summary>
        public ResponseModel GettLabelXList(string beingTime,string ordernumbel="")
        {

            // DateTime dateTime = DateTime.Now.AddDays(-30);
            string Commandtext = "";
            if (ordernumbel!="")
            {
                Commandtext = string.Format(@"select OrderNumbels ,count(OrderNumbels) as OrderCount from W_LabelStorage where OutTime> '{0}' and OrderNumbels='{1}'  GROUP BY  OrderNumbels", beingTime, ordernumbel);
            }
            else
            {
                Commandtext = string.Format(@"select OrderNumbels ,count(OrderNumbels) as OrderCount from W_LabelStorage where OutTime> '{0}'  GROUP BY  OrderNumbels", beingTime);
            }
          
            var Ordercounts= DbEntities.Database.SqlQuery<tLabels_OrderCount>(Commandtext).ToList();

          //  var banners = DbEntities.tLabels_X.Where(w=>w.OrderNumbels,w).ToList().OrderByDescending(c => c.fhDate1);
            var response = new ResponseModel();
            response.code = 200;
            response.result = "Labels集合获取成功";
            response.data = new List<tLabels_OrderCount>();

            response.data = Ordercounts;

            return response;
        }



        public User UserLoging(LoginData loginData)
        {
            //string.Format("select a.AType from tUser u join tAgent a on u.AgentID = a.AID  where UserID='{0}' and PWD='{1}'", loginData.Username, new DataEnCode().Encrypt(loginData.Password));

            var redb = DbEntities.Users.AsNoTracking().Where(w => w.UserID == loginData.Username).FirstOrDefault();

            if(redb!=null)
            {
                string enstr = new DataEnCode().Encrypt(loginData.Password);
              //  string destr = new DataEnCode().Decrypt(redb.PWD);
                if (redb.PWD.Trim() == enstr.Trim())
                {
                    return _usernnfo=redb;// loginData.Username;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public ResponseModel GetOrdersFinish(string beingDatePickers)
        {
            DateTime dateTime = DateTime.Now.AddDays(-7);
            List<W_OrderTable> listfinish = null;//DbEntities.W_OrderTable.AsNoTracking().Where(T=>T.FinishTime> dateTime).ToList();
            var response = new ResponseModel();
            if (listfinish != null)
            {
              
                response.code = 200;
                response.result = "Labels集合获取成功";
                response.data = new List<W_OrderTable>();

                response.data = listfinish;
              
            }
            else
            {
              
                response.code = 400;
                response.result = "完成出库单集合获取失败";
            }
            return response;
            //  throw new NotImplementedException();
        }
    }
}

