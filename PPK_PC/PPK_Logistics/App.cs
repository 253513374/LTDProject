using System.Collections.ObjectModel;
using System.Windows.Data;
using PPK_Logistics.COM;
using PPK_Logistics.Config;
using PPK_Logistics.DataSource;
using PPK_Logistics.DataSync;

namespace PPK_Logistics
{
    public partial class App : System.Windows.Application
    {
        /// <summary>
        /// 出库单数据 视图 包含过滤、分组、排列
        /// </summary>
        public static CollectionViewSource ProductListView;

        private static Data_Sync data_Sync = new Data_Sync(Tools.HproseUrl);

        private static DateCom dateCom = new DateCom();

        /// <summary>
        /// 垛数据源
        /// </summary>
        private static ObservableCollection<Checkin_ltjyStack> listCheckin_ltjyStack = new ObservableCollection<Checkin_ltjyStack>();

        private static OracleDB orcldb = new OracleDB();

        private static UserReadonly uread = new UserReadonly();

        public static Data_Sync Data_Sync
        {
            get { return data_Sync; }
        }

        public static DateCom DateCom
        {
            get { return dateCom; }
        }

        public static ObservableCollection<Checkin_ltjyStack> ListCheckin_ltjyStack
        {
            get { return listCheckin_ltjyStack; }
            set { listCheckin_ltjyStack = value; }
        }

        public static OracleDB OracleDB
        {
            get { return orcldb; }
        }

        public static UserReadonly UserReadonly
        {
            get
            {
                return uread;//= Tools.DeSerializeShared(new UserReadonly());
            }
            set
            {
                uread = value;
            }
        }
    }
}