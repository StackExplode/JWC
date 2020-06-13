using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JWCControlLib
{
    /// <summary>
    /// XXControl.xaml 的交互逻辑
    /// </summary>
    [DataContract]
    public partial class XXControl : JWCControl
    {
        public XXControl():base()
        {
            InitializeComponent();
            //base.BindSelector(board1);
            base.BindMainGrid(grid1);
            this.Height = this.Width = 350;
        }



        [Outputable]
        [RedirectGS("GSR_ForeColor")]
        public Color ForeColor { 
            get { return (grid1.Background as SolidColorBrush).Color; }
            set { grid1.Background = new SolidColorBrush(value); } 
        }

        protected object[] GSR_ForeColor(bool isset,object[] val)
        {
            XXControl me = this;
            if(isset)
            {
                //var mth = Regex.Match((string)val, "([0-9]{1,3}),([0-9]{1,3}),([0-9]{1,3}),([0-9]{1,3})");
                //var rst = mth.Groups;
                //me.ForeColor = Color.FromArgb(byte.Parse(rst[4].Value), byte.Parse(rst[1].Value), byte.Parse(rst[2].Value), byte.Parse(rst[3].Value));

                byte R = Convert.ToByte(val[0]);
                byte G = Convert.ToByte(val[1]);
                byte B = Convert.ToByte(val[2]);
                byte A = Convert.ToByte(val[3]);

                me.ForeColor = Color.FromArgb(A, R, G, B);
                return null;
            }
            else
            {
                Color cl = me.ForeColor;
                return new object[] { cl.R, cl.G, cl.B, cl.A };
            }
        }

        
        bool _showpick = false;

        [PropDiscribe(CreatorPropType.Boolean,"显示图片","测试！")]
        [Outputable]
        public bool ShowPic
        {
            get { return _showpick; }
            set
            {
                _showpick = value;
                if(_showpick)
                {
                    img1.Source = Properties.Resources.pixiv5227038.ToBitMapSource();
                }
                else
                {
                    img1.Source = null;
                }
            }
        }

        //private void Resize(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        //{
        //    if (this.Width+ e.HorizontalChange > 10)
        //        this.Width += e.HorizontalChange;
        //    if (this.Height + e.VerticalChange > 10)
        //        this.Height += e.VerticalChange;
        //}
    }
}
