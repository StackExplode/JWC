using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Net;
using System.Web;
using System.Threading;

using JWCControlLib;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace NanjingControls
{
    /// <summary>
    /// WeatherBanner.xaml 的交互逻辑
    /// </summary>
    [JWCControlDesc("天气预报栏","和固定格式接口通信的天气预报栏，目前还在测试中")]
    public partial class WeatherBanner : JWCControl
    {
        public WeatherBanner()
        {
            InitializeComponent();
            base.BindMainGrid(grid1);
            PoolInterval = 10;
            HttpTimeout = 5000;
        }

        string wurl = "";
        DispatcherTimer tm;
        public override void Init(bool iseditmode)
        {
            if(!iseditmode)
            {
                if (wurl.Length < 1)
                    return;
                UpdateDataAsync(true);
                tm = new DispatcherTimer();
                tm.Interval = new TimeSpan(0, 0, PoolInterval);
                tm.Tick += tm_Tick;
                tm.Start();
            }
        }


        public override void DeInit(bool iseditmode)
        {
            if(!iseditmode)
            {
                if (tm != null)
                {
                    tm.Stop();
                    tm = null;
                }
                    
            }
        }

        void tm_Tick(object sender, EventArgs e)
        {
            tm.Stop();
            UpdateDataAsync(false);
            
        }

        void UpdateDataAsync(bool first)
        {
            Task ta = new Task((
                () =>
                {
                    string s = "";
                    try
                    {
                        s = RequestData();
                    }
                    catch (Exception ex)
                    {
                        s = "Http通信异常:" + ex.Message;
                    }

                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        txt_weather.Text = s;
                        if(!first)
                         tm.Start();
                    }));
                }
                ));
            ta.Start();
            
        }

        string RequestData()
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(WeatherURL);
            req.Method = "GET";
            req.Timeout = HttpTimeout;
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Encoding en = Encoding.GetEncoding("utf-8");
            StreamReader sr = new StreamReader(res.GetResponseStream(),en);
            string data = sr.ReadToEnd();
            sr.Close();
            res.Close();
            return data;
        }

        [PropDiscribe( CreatorPropType.Text,"天气接口URL")]
        [Outputable]
        public string WeatherURL
        {
            get { return wurl; }
            set { wurl = value; }
        }

        [PropDiscribe( CreatorPropType.Text,"轮询间隔","单位：秒")]
        [Outputable]
        public int PoolInterval { get; set; }

        [PropDiscribe(CreatorPropType.Text,"通信超时","单位：毫秒")]
        [Outputable]
        public int HttpTimeout { get; set; }
    }
}
