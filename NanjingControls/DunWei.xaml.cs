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
using System.Reflection;
using JWCCommunicationLib;
using System.Collections.Concurrent;

namespace NanjingControls
{
    /// <summary>
    /// DunWei.xaml 的交互逻辑
    /// </summary>
    [JWCControlLib.JWCControlDesc("南京蹲位","南京项目中用到对蹲位有无人员控件，使用两张图片指示状态。")]
    public partial class DunWei : JWCControlLib.JWCControl,IDataReceiver
    {


        protected BitmapImage UsingBitmap;
        protected BitmapImage FreeBitmap;
        protected string UsingPath = "";
        protected string FreePath = "";

        //public static string FriendlyName
        //{
        //    get
        //    {
        //        return Fname;
        //    }
        //}

        //public static string Description
        //{
        //    get
        //    {
        //        return DescribeString;
        //    }
        //}

        protected static readonly BitmapImage Default_UsingImage = new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/Resources/using.png"));
        protected static readonly BitmapImage Default_FreeImage = new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/Resources/free.png"));

        public DunWei():base()
        {
            InitializeComponent();
            base.BindMainGrid(grid1);
            UsingBitmap = Default_UsingImage;
            FreeBitmap = Default_FreeImage;
            IsUing = true;
            RecID = "";
            DunRegAddr = "1";
            //img1.Source = ToBitMapSource(Properties.Resources._58699774_p0);
        }

       

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

        [PropDiscribe(CreatorPropType.Boolean,"有人","指示默认蹲位是否有人")]
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
                _isusing = value;
            }
        }

        [PropDiscribe(CreatorPropType.DialogWithText, "有人图片", "当蹲位有人时显示的图片路径，留空则使用内置图片。如果路径位于本程序子目录内则保存为相对路径否则使用绝对路径",
           new object[] { "打开", typeof(OpenFilePropDialog), new object[] { "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp", false ,true} })]
        [Outputable]
        public string UsingImage
        {
            get { return UsingPath; }
            set
            {
                UsingPath = value;
                if (value == String.Empty)
                    UsingBitmap = Default_UsingImage;
                else
                    UsingBitmap = new BitmapImage(new Uri(@"file:///" + System.IO.Path.GetFullPath(value)));
                IsUing = IsUing;
            }
        }

        [PropDiscribe(CreatorPropType.DialogWithText, "无人图片", "当蹲位无人时显示的图片路径，留空则使用内置图片。如果路径位于本程序子目录内则保存为相对路径否则使用绝对路径",
             new object[] { "打开", typeof(OpenFilePropDialog), new object[] { "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp", false,true } })]
        [Outputable]
        public string FreeImage
        {
            get { return FreePath; }
            set
            {
                FreePath = value;
                if (value == String.Empty)
                    FreeBitmap = Default_FreeImage;
                else
                    FreeBitmap = new BitmapImage(new Uri(@"file:///" + value));
            }
        }

        [PropDiscribe(CreatorPropType.Text, "蹲位寄存器地址", "设置通信中蹲位对应的寄存器地址")]
        [Outputable]
        public string DunRegAddr { get; set; }

        [PropDiscribe( CreatorPropType.Text,"接收ID","身为接收者的ID")]
        [Outputable]
        public string RecID
        {
            get;
            set;
        }





        public void SetRegister(string key, object data)
        {
            if(key == DunRegAddr)
            {
                IsUing = (bool)data;
            }
        }



        public event Action<object, string> OnRequestRegister;

        public RegisterType GetRegisterType(string key)
        {
            if (key == DunRegAddr)
                return RegisterType.Boolean;
            return RegisterType.Undefined;
        }
    }
}
