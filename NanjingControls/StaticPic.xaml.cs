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

using JWCControlLib;

namespace NanjingControls
{
    /// <summary>
    /// StaticPic.xaml 的交互逻辑
    /// </summary>
    [JWCControlDesc("静态图片","用于显示一副静态的本地图片，通常这用于对背景细节的弥补，但过多的使用会带来内存开销的增加。")]
    public partial class StaticPic : JWCControlLib.JWCControl
    {
        public StaticPic():base()
        {
            InitializeComponent();
            base.BindMainGrid(grid_main);
            ShowPic = true;
            this.Height = this.Width = 100;
            img1.Stretch = Stretch.Fill;
        }

        protected BitmapImage BgImage;
        protected string BgPath = "";
        bool IsShowing = true;

        [PropDiscribe( CreatorPropType.Boolean,"显示图片","显示或隐藏图片")]
        [Outputable]
        public bool ShowPic
        {
            get { return IsShowing; }
            set
            {
                IsShowing = value;
                if (IsShowing)
                    img1.Source = BgImage;
                else
                    grid_main.Background = null;
            }
        }

        [PropDiscribe(CreatorPropType.DialogWithText, "图片路径", "要显示的图片路径，留空则使用内置图片。如果路径位于本程序子目录内则保存为相对路径否则使用绝对路径",
           new object[] { "打开", typeof(OpenFilePropDialog), new object[] { "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp", false, true } })]
        [Outputable]
        public string ImagePath
        {
            get { return BgPath; }
            set
            {
                BgPath = value;
                if (value == null || value.Length < 1)
                    BgImage = null;
                else
                {
                    BgImage = new BitmapImage(new Uri(@"file:///" + System.IO.Path.GetFullPath(value)));
                    ShowPic = ShowPic;
                }
            }
        }

        
    }
}
