using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using JWCCommunicationLib;
using System.IO;

namespace JWC
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            JWCCommunicationLib.JWCCommunicatorFactory.LoadLibs(AppDomain.CurrentDomain.BaseDirectory + "\\Communicators");
            JWCControlFactory.LoadLibs(AppDomain.CurrentDomain.BaseDirectory + "\\Controls");
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            this.Left = 0;
            this.Top = 0;
            this.Topmost = true;
        }

        AJWCComunicator Communicator;
        AJWCCAdaptor Adaptor;
        string Filepath = "./run.jwc";
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Dispose all componet
            if(Communicator != null)
            {
                Communicator.Stop(false);
                Communicator.Dispose();
            }
            
            foreach(var c in grid_main.Children)
            {
                JWCControl jc = c as JWCControl;
                if (jc != null)
                    jc.DeInit(false);
            }
        }




        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
           var dr = MessageBox.Show("确认退出吗？","询问", MessageBoxButton.YesNo, MessageBoxImage.Question);
           if (dr == MessageBoxResult.Yes)
           {
     
               this.Close();
           }
        }

        void SetRunFilePath()
        {
            //FileStream fs = new FileStream("./autorun.conf", FileMode.Open);
            //StreamReader sr = new StreamReader
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetRunFilePath();
            if (!System.IO.File.Exists(Filepath))
                return;

            JWCSerializer<JWCSaveFile> jse = new JWCSerializer<JWCSaveFile>();
            JWCSaveFile file = jse.Deserialize(this.Filepath);
            Color cl = Color.FromArgb(file.BackColor[0], file.BackColor[1], file.BackColor[2], file.BackColor[3]);

            Communicator = JWCCommunicatorFactory.CreateCommunicator(file.ComName); 
            if(Communicator == null)
            {
                MessageBox.Show("创建通信器失败！这可能是由于没有导入相关插件导致的。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            Communicator.InputProperty(file.Communicator);
            Adaptor = JWCCommunicatorFactory.CreateAdapter(file.AdaName);
            if (Adaptor == null)
            {
                MessageBox.Show("创建适配器失败！这可能是由于没有导入相关插件导致的。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            Adaptor.InputProperty(file.ComAdapter);
            Adaptor.SetCommunicator(Communicator);
            
            foreach (var s in file.AllControls)
            {
                string fullname = s["FullName"].ToString();
                JWCControl jc = JWCControlFactory.CreateInstance(fullname);
                if (jc == null)
                {
                    MessageBox.Show("读入控件失败！这可能是由于没有导入相关插件导致的。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    return;
                }
                jc.InputProperty(s);
                jc.IsEditMode = false;
                jc.Init(false);
                jc.Parent = grid_main;
                grid_main.Children.Add(jc);
                IDataSender se = jc as IDataSender;
                if (se != null)
                    Adaptor.AppendSender(se);
                IDataReceiver re = jc as IDataReceiver;
                if (re != null)
                    Adaptor.AppendReceiver(re);

            }
            this.Height = grid_main.Height = file.Height;
            this.Width = grid_main.Width = file.Width;
            SetBg(file.BgUsePic, cl, file.BackGroundPic);
            Communicator.Initialization();
            Adaptor.Initialization();
            Communicator.Start();
        }

        public void SetBg(bool b, Color cl, string fn)
        {
            //BgUsePic = b;
            if (!b)
            {
                //BgColor = cl;
                grid_main.Background = new SolidColorBrush(cl);
            }
            else
            {
                //BgFilename = fn;
                fn = System.IO.Path.GetFullPath(fn);
                ImageBrush br = new ImageBrush();
                br.Stretch = Stretch.Fill;
                br.ImageSource = new BitmapImage(new Uri(fn));
                grid_main.Background = br;
            }
        }
    }
}
