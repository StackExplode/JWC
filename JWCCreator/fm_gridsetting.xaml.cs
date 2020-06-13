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
using System.Windows.Shapes;

using JWCControlLib;

namespace JWCCreator
{
    /// <summary>
    /// fm_gridsetting.xaml 的交互逻辑
    /// </summary>
    public partial class fm_gridsetting : Window
    {
        public fm_gridsetting(Stage st)
        {
            InitializeComponent();
            stage = st;
        }

        Stage stage;

        void SetColor(Color cl)
        {
            brd_color.Background = new SolidColorBrush(cl);
            lbl_color.Content = cl.R.ToString() + ", " + cl.G.ToString() + ", " + cl.B.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt_w.Text = ((int)stage.MainStage.Width).ToString();
            txt_h.Text = ((int)stage.MainStage.Height).ToString();
            if (stage.BgUsePic)
                rd_pic.IsChecked = true;
            else
                rd_color.IsChecked = true;
            SetColor(stage.BgColor);
            if (stage.BgFilename != null && stage.BgFilename.Length > 0)
                txt_pic.Text = stage.BgFilename;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            stage.MainStage.Width = Convert.ToInt32(txt_w.Text);
            stage.MainStage.Height = Convert.ToInt32(txt_h.Text);
            bool b = false;
            if (rd_pic.IsChecked == true)
                b = true;
            var brs = (SolidColorBrush)brd_color.Background;

            stage.SetBg(b, brs.Color, txt_pic.Text);
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog dia = new System.Windows.Forms.ColorDialog();
            var dr = dia.ShowDialog();
            if(dr == System.Windows.Forms.DialogResult.OK)
            {
                Color cl = Color.FromArgb(dia.Color.A, dia.Color.R, dia.Color.G, dia.Color.B);
                SetColor(cl);

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFilePropDialog op = new OpenFilePropDialog();
            object obj = null;
            var dr = op.ShowDialog(out obj, new object[] { "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp", false, true });
            if(dr == System.Windows.Forms.DialogResult.OK)
                txt_pic.Text = obj.ToString();            
        }
    }
}
