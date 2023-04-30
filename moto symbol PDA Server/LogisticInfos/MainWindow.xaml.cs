using Hprose.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using BackEndServer.Service;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Net;
using ControlzEx.Standard;

namespace BackEndServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private HproseHttpListenerServer httpserver = new HproseHttpListenerServer();

        private Queryservice Queryservice = new Queryservice();

        public ObservableCollection<ScanQuery> ScanqueryList { set; get; } = new ObservableCollection<ScanQuery>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this.Queryservice;
        }

        public void AddQueryCount(ScanQuery code)
        {
            ScanqueryList.Insert(0, code);

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                MsgText.Text = string.Format("截至目前接收到{0}次查询", ScanqueryList.Count.ToString());
            }));
        }

        private string HttpWebClient(string args)
        {
            string url = $"https://www.rewt.cn/Traceability?qrcode={args}";

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    string response = webClient.DownloadString(url);
                    JObject jsonResponse = JObject.Parse(response);

                    JToken dataToken = jsonResponse["data"];
                    if (dataToken == null || dataToken.Type == JTokenType.Null)
                    {
                        return IsNull(args);
                    }
                    else
                    {
                        JObject data = dataToken.Value<JObject>();
                        bool status = data["status"].Value<bool>();
                        string msg = data["msg"].Value<string>();
                        string qrCode = data["qrCode"].Value<string>();
                        string agentName = data["agentName"].Value<string>();
                        string orderNumbels = data["orderNumbels"].Value<string>();
                        DateTime outTime = data["outTime"].Value<DateTime>();
                        bool isActive = data["isActive"].Value<bool>();

                        return NotNull(qrCode, agentName, outTime.ToString(), "");
                        //Console.WriteLine("JSON Response: " + jsonResponse.ToString());
                    }
                }
                catch (WebException ex)
                {
                    return IsNull(args);
                }
            }
        }

        private string NotNull(string code, string AName, string fhDate1, string fhAgent1)
        {
            StringBuilder _result = new StringBuilder();
            _result.Append("<Info xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

            _result.Append(string.Format("<LabelNumber>{0}</LabelNumber>", code));
            _result.Append("<Cheat></Cheat>");
            _result.Append("<ProductName>电缆</ProductName>");
            _result.Append(string.Format("<PackDate></PackDate>"));
            _result.Append(string.Format("<PackBatch></PackBatch>"));
            _result.Append(string.Format("<fhProductid>{0}</fhProductid>", "fhAgent1"));
            _result.Append(string.Format("<fhDate1>{0}</fhDate1>", "fhDate1"));
            _result.Append(string.Format("<fhNumbel>{0}</fhNumbel>", "fhPaper1"));
            _result.Append(string.Format("<fhType1>{0}</fhType1>", "fhType1"));
            _result.Append(string.Format("<Content></Content>"));
            _result.Append(string.Format("<Cheat></Cheat>"));
            _result.Append(string.Format("<First>{0}|{1}|{2}|{3}</First>", "", "", AName, fhDate1));
            _result.Append("</Info>");

            return _result.ToString();
        }

        private string IsNull(string code)
        {
            StringBuilder _result = new StringBuilder();
            _result.Append(string.Format("<LabelNumber>{0}</LabelNumber>", ""));
            _result.Append("<Cheat></Cheat>");
            _result.Append("<ProductName ></ ProductName>");
            _result.Append("<PackDate></PackDate>");
            _result.Append("<PackBatch></PackBatch>");
            _result.Append("<fhProductid></fhProductid>");
            _result.Append("<fhDate1></fhDate1>");
            _result.Append("<fhNumbel></fhNumbel>");
            _result.Append("<fhType1></fhType1>");
            _result.Append("<Content></Content>");
            _result.Append("<Cheat></Cheat>");
            _result.Append(string.Format("<First>{0}|{1}|{2}|{3}</First>", "", "", "", ""));
            _result.Append("</Info>");

            return _result.ToString();
        }

        /// <summary>
        /// 获取物流串货数据
        /// </summary>
        /// <param name="_label"></param>
        /// <returns></returns>
        public string GetLogistinInfo(string code)
        {
            return HttpWebClient(code);
            StringBuilder _result = new StringBuilder();
            try
            {
                _result.Append("<Info xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

                string sqltext = string.Format(@"select w.OrderTime as fhDate1,w.Dealers as fhAgent1,w.ExtensionName as fhType1,w.OrderNumbels as fhPaper1,t.AName from W_LabelStorage w join tAgent  t on w.Dealers =t.AID  where QRCode = '{0}';", code);

                DataTable dt = SqlServerDb.ExceDataTable(sqltext);
                if (dt != null)
                {
                    _result.Append("<Info xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

                    _result.Append(string.Format("<LabelNumber>{0}</LabelNumber>", code));
                    _result.Append("<Cheat></Cheat>");
                    _result.Append("<ProductName>电缆</ProductName>");
                    _result.Append(string.Format("<PackDate></PackDate>"));
                    _result.Append(string.Format("<PackBatch></PackBatch>"));
                    _result.Append(string.Format("<fhProductid>{0}</fhProductid>", dt.Rows[0]["fhAgent1"].ToString().Trim()));
                    _result.Append(string.Format("<fhDate1>{0}</fhDate1>", dt.Rows[0]["fhDate1"].ToString().Trim()));
                    _result.Append(string.Format("<fhNumbel>{0}</fhNumbel>", dt.Rows[0]["fhPaper1"].ToString().Trim()));
                    _result.Append(string.Format("<fhType1>{0}</fhType1>", dt.Rows[0]["fhType1"].ToString().Trim()));
                    _result.Append(string.Format("<Content></Content>"));
                    _result.Append(string.Format("<Cheat></Cheat>"));
                    _result.Append(string.Format("<First>{0}|{1}|{2}|{3}</First>", "", "", dt.Rows[0]["AName"].ToString().Trim(), dt.Rows[0]["fhDate1"].ToString().Trim()));
                    _result.Append("</Info>");

                    AddQueryCount(new ScanQuery() { QrCode = code, Name = dt.Rows[0]["AName"].ToString().Trim() });
                    //new Thread(() =>
                    //{
                    //    AddQueryCount(new ScanQuery() { QrCode = code, Name = dt.Rows[0]["AName"].ToString().Trim() });
                    //}).Start();
                }
                else
                {
                    _result.Append(string.Format("<LabelNumber>{0}</LabelNumber>", code));
                    _result.Append("<Cheat></Cheat>");
                    _result.Append("<ProductName ></ ProductName>");
                    _result.Append("<PackDate></PackDate>");
                    _result.Append("<PackBatch></PackBatch>");
                    _result.Append("<fhProductid></fhProductid>");
                    _result.Append("<fhDate1></fhDate1>");
                    _result.Append("<fhNumbel></fhNumbel>");
                    _result.Append("<fhType1></fhType1>");
                    _result.Append("<Content></Content>");
                    _result.Append("<Cheat></Cheat>");
                    _result.Append(string.Format("<First>{0}|{1}|{2}|{3}</First>", "", "", "", ""));
                    _result.Append("</Info>");
                }
            }
            catch (Exception ex)
            {
                _result.Append(ex.Message);
                //  MessageBox.Show(ex.Message);
            }
            // MessageBox.Show(_result.ToString());

            return _result.ToString();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;
            button.IsEnabled = false;
            httpserver.Stop();
            ListBoxServerMsg.Items.Insert(0, "服务关闭成功:" + DateTime.Now.ToString());
            StartButton.IsEnabled = true;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;
            try
            {
                if (Queryservice.IsUrl())
                {
                    httpserver = new HproseHttpListenerServer(Queryservice.GetUrl());
                    httpserver.Methods.AddInstanceMethods(Queryservice);
                    httpserver.Methods.AddInstanceMethods(new MainWindow());

                    httpserver.IsDebugEnabled = true;
                    httpserver.Start();
                    ListBoxServerMsg.Items.Insert(0, "服务启动成功:" + DateTime.Now.ToString());
                    //this.Queryservice.ServerMsg = "服务启动成功";
                    button.IsEnabled = false;
                    StopButton.IsEnabled = true;
                }
                //}
                //else
                //{
                //    MessageBox.Show("请输入正确的服务器IP或者端口！");
                //}
            }
            catch (Exception ex)
            {
                ListBoxServerMsg.Items.Insert(0, "服务启动错误:" + ex.Message);
                // MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowListBox.ItemsSource = ScanqueryList;
            //GetSendproductinfo();//模拟下载发货单数据
            //PDAServer server = new PDAServer();
            //server.GetERP_Kcswz("N");
            //server.GetERP_KcswzCount("CC15100217");
            //server.GetERP_KcswzSearch("CC15100217");
            string Re = GetLogistinInfo("143852493854");
            //Hprose.Client.HproseHttpClient Client = new Hprose.Client.HproseHttpClient("http://124.225.216.98:2015/");
            //string Re=Client.Invoke<string>("GetLogistinInfo", new object[]{ "143804782711" });

            MessageBox.Show(Re);
            // string Reu =GetLogistinInfo("143804782711");
            // MessageBox.Show(Reu);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}