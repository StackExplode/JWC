using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Diagnostics;

namespace JWCCreator
{
    /// <summary>
    /// fm_about.xaml 的交互逻辑
    /// </summary>
    public partial class fm_about : Window
    {
        public fm_about()
        {
            InitializeComponent();
            lbl_ver.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start("https://github.com/StackExplode/JWC");
            e.Handled = true;
        }

        private void Hyperlink_RequestNavigate_1(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start("https://www.gnu.org/licenses/gpl-3.0.zh-cn.html");
            e.Handled = true;

        }

        private void Hyperlink_RequestNavigate_2(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start("https://blog.jloli.cc");
            e.Handled = true;
        }
    }
}
