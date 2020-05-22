using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weitedianlan.model.Entity;
using Weitedianlan.model.Response;
using Weitedianlan.model.ReQuest;
using Oracle.ManagedDataAccess.Client;
using System.Linq.Expressions;
using System.Data;
using System.Reflection;
using System.Configuration;

namespace Weitedianlan.Oracle.service
{
    public class OrderService
    {
        private Oracle_ERP_Db _db;
        public OrderService(Oracle_ERP_Db db)
        {
            this._db = db;  
        }

        protected string QuerySqlstr { set; get; }

        private dynamic GetSqlQueryString(string beingTime="",string endTime="", QuestTypeEnum typeEnum= QuestTypeEnum.DefaultQuest, string ordernum="")
        {
            string BTime = Convert.ToDateTime(beingTime).ToString(@"yyyy-MM-dd");

            string ETime = Convert.ToDateTime(endTime).Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(@"yyyy-MM-dd HH:mm:ss");

            if (typeEnum== QuestTypeEnum.DefaultQuest) return QuerySqlstr = string.Format(@"select * from T_BDX_ORDER where DDRQ >= '{0}' and DDRQ <= '{1}'  ORDER BY DDRQ DESC", BTime, ETime);


           // if (typeEnum == QuestTypeEnum.GroupByQuest) return string.Format(@"select t.DDNO, t.DDRQ,t.KH,t.DW, SUM(to_number(t.SL)) as SL from T_BDX_ORDER t where   DDRQ >= '{0}' and DDRQ <= '{1}' Group by t.DDNO ,t.DDRQ,t.KH,t.DW", BTime,ETime);


            if (typeEnum == QuestTypeEnum.OrderNumQuest) return QuerySqlstr = string.Format(@"select * from T_BDX_ORDER where DDRQ >= '{0}' and DDRQ <= '{1}' and   DDNO LIKE '%{2}%'  ORDER BY DDRQ DESC ", BTime, ETime, ordernum);
            //return string.Format(@"select t.DDNO, t.DDRQ,t.KH,t.DW, SUM(t.SL) as SL from T_BDX_ORDER t where t.DDNO = '{0}' Group by t.DDNO ,t.DDRQ,t.KH,t.DW", ordernum);

            return "";

        }

        private dynamic GetDatabase(string Commandtext)
        {
            return _db.Database.SqlQuery<T_BDX_ORDER>(Commandtext).ToList().OrderByDescending(t => t.DDRQ);
        }

        
        //public ResponseModel GetGroupByQuest(string orderNum)
        //{

        //}
        /// <summary>
        /// 获取所有表单集合
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetResponse(string beingtime,string endtime, string ordernum)
        {
            if (ordernum!="")
            {
                GetSqlQueryString(beingtime, endtime, QuestTypeEnum.OrderNumQuest, ordernum);
            }
            else
            {
                GetSqlQueryString(beingtime, endtime, QuestTypeEnum.DefaultQuest, ordernum);
            }
           
            var orders = GetOrderList<T_BDX_ORDER>(QuerySqlstr);//GetDatabase(sql);
           //  var ordersfinish = 

            var responsemodel = new ResponseModel();
            responsemodel.code = 200;
            responsemodel.result = "出库单集合获取成功";
            responsemodel.data = new List<T_BDX_ORDER>();
            responsemodel.data = orders;
            //foreach (var order in orders)
            //{
            //    responsemodel.data.Add(new T_BDX_ORDER
            //    {

            //        DDNO = order.DDNO,
            //        DDRQ = order.DDRQ,
            //        KH = order.KH,
            //        XH = order.XH,
            //        GGXH = order.GGXH,
            //        SL = order.SL,
            //        DW = order.DW,
            //        DJ = order.DJ,
            //        YS = order.YS
            //    });
            //}
            return responsemodel;
        }


        /// <summary>
        /// 根据单号获取出库单数据
        /// </summary>
        /// <param name="ordernum"></param>
        /// <returns></returns>
        public ResponseModel GetResponse( string ordernum)
        {
            GetSqlQueryString(null, null, QuestTypeEnum.OrderNumQuest, ordernum);

            var orders = GetOrderList<T_BDX_ORDER>(QuerySqlstr);//GetDatabase(sql);
            var responsemodel = new ResponseModel();
            responsemodel.code = 200;
            responsemodel.result = "出库单集合获取成功";
            responsemodel.data = new List<OrderDetail>();
            foreach (var order in orders)
              {
                responsemodel.data.Add(new OrderDetail
                {

                    DDNO = order.DDNO,
                    DDRQ = order.DDRQ,
                    KH = order.KH,
                    XH = order.XH,
                    GGXH = order.GGXH,
                    SL = order.SL,
                    DW = order.DW,
                    DJ = order.DJ,
                    YS = order.YS
                });
            };

            

            return responsemodel;
        }


        public IList<T> GetOrderList<T>(string Commandtext)
        {

            DataSet dataSet = new DataSet();

            try
            {
                var orclConnection = ConfigurationManager.ConnectionStrings["Oracle_ERP_Db"].ConnectionString;//ConfigurationSettings.AppSettings["Oracle_Db"].ToString();
                using (OracleConnection connection = new OracleConnection(orclConnection))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    // string Commandtext = @"select * FROM T_BDX_ORDER";
                    using (OracleCommand oracleCommand = new OracleCommand(Commandtext, connection))
                    {
                        oracleCommand.ExecuteReader();
                        using (OracleDataAdapter odater = new OracleDataAdapter(oracleCommand))
                        {
                            odater.Fill(dataSet);
                        }
                    }

                    return DataSetToList<T>(dataSet, 0);// dataSet.Tables[0].Rows.AsQueryable().GetEnumerator().;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //  var Commandtext = GetSqlQueryString(beingtime, endtime, QuestTypeEnum.DefaultQuest, ordernum);
           

        }


         /// <summary>       
         /// DataSetToList        
         /// </summary>        
         /// <typeparam name="T">转换类型</typeparam>        
         /// <param name="dataSet">数据源</param>       
         /// <param name="tableIndex">需要转换表的索引</param>       
         /// <returns></returns>
         public IList<T> DataSetToList<T>(DataSet dataSet, int tableIndex)
        {
            //确认参数有效            
            if (dataSet == null || dataSet.Tables.Count <= 0 || tableIndex < 0)   return null;
            DataTable dt = dataSet.Tables[tableIndex];
            IList<T> list = new List<T>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象                
                T _t = Activator.CreateInstance<T>();
                //获取对象所有属性                
                PropertyInfo[] propertyInfo = _t.GetType().GetProperties();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        //属性名称和列名相同时赋值                        
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToString()))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                info.SetValue(_t, dt.Rows[i][j], null);
                            }
                            else
                            {
                                info.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(_t);
            }
            return list;
        }

    }
}
