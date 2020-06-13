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

namespace JWCCreator
{
    /// <summary>
    /// fm_new.xaml 的交互逻辑
    /// </summary>
    public partial class fm_new : Window
    {
        public fm_new()
        {
            InitializeComponent();
        }

        public event Action<int, int> OnConfirmNew;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int h = Convert.ToInt32(txt_h.Text);
            int w = Convert.ToInt32(txt_w.Text);
            this.Close();
            if (OnConfirmNew != null)
                OnConfirmNew(w, h);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
