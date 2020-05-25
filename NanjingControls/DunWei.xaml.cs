using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.IO;
using System.Drawing.Imaging;

namespace NanjingControls
{
    /// <summary>
    /// DunWei.xaml 的交互逻辑
    /// </summary>
    public partial class DunWei : JWCControlLib.JWCControl
    {
        public  const string Fname = "WC蹲位";
        public  const string DescribeString = "用两张图片指示两个状态的蹲位";

        public static string FriendlyName
        {
            get
            {
                return Fname;
            }
        }

        public static string Description
        {
            get
            {
                return DescribeString;
            }
        }

        public DunWei():base()
        {
            InitializeComponent();
            base.BindMainGrid(grid1);
            UsingBitmap = new BitmapImage(new Uri("file:///" + GetPic_Debug(true)));
            FreeBitmap = new BitmapImage(new Uri("file:///" + GetPic_Debug(false)));
            IsUing = true;
            //img1.Source = ToBitMapSource(Properties.Resources._58699774_p0);
        }

     

        protected BitmapImage UsingBitmap;
        protected BitmapImage FreeBitmap;

#if DEBUG
        string GetPic_Debug(bool use)
        {
            const string picpath = @"D:\研究生所有工作\智能厕所\JWC\JWCCreator\bin\Debug\Userdata\";
            if (use)
                return picpath + "using.png";
            else
                return picpath + "free.png";
        }
#endif
        bool _isusing = false;
        [Outputable]
        public bool IsUing
        {
            get
            {
                return _isusing;
            }
            set
            {
                if (value)
                    this.img1.Source = UsingBitmap;
                else
                    this.img1.Source = FreeBitmap;
            }
        }


    }
}
