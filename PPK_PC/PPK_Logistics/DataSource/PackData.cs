using System.ComponentModel;//INotifyPropertyChanged

namespace PPK_Logistics.DataSource
{
    public class PackData : INotifyPropertyChanged//ObservableCollection<ProductPackNumber>//
    {
        public static int Jcount = 0;
        public static string[] Norms = null;
        public static bool Rank_no_off = true;
        public static int Threecount = 0;

        /// <summary>
        /// 箱标扫描开关
        /// </summary>
        public static bool Threeno_off = true;

        public static int Twocount = 0;
        public static bool Twono_off = true;
        private string rankone = "";
        private string rankthree = "";
        private string ranktwo = "";

        public PackData()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 包装等级一，箱标
        /// </summary>
        public string RankOne
        {
            get { return rankone; }
            set
            {
                if (value.Length > 0)
                {
                    rankone = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("RankOne"));
                    Rank_no_off = false;
                }
            }
        }

        /// <summary>
        /// 包装等级三，瓶标
        /// </summary>
        public string RankThree
        {
            get { return rankthree; }
            set
            {
                if (value.Length > 0)
                {
                    rankthree = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("RankThree"));
                    Threecount++;
                }
            }
        }

        /// <summary>
        /// 包装等级二， 盒标
        /// </summary
        public string RankTwo
        {
            get { return ranktwo; }
            set
            {
                if (value.Length > 0)
                {
                    ranktwo = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("RankTwo"));
                    Twocount++;
                }
            }
        }

        public static void Initialize()
        {
            Twono_off = true;
            Threeno_off = true;
            Rank_no_off = true;
            Threecount = 0;
            Twocount = 0;
        }

        /// <summary>
        /// 判断包装关系最后一级是否完成
        /// </summary>
        /// <param name="JCount"></param>
        /// <returns></returns>
        public bool ISEquals(int JCount)
        {
            bool _return = false;
            if (JCount == Twocount)
            {
                _return = true;
            }
            return _return;
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }

            //this.PropertyChanged(this, e);
        }
    }
}