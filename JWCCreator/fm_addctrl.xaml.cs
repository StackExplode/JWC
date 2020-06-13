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

using System.Collections.Concurrent;
using JWCControlLib;

namespace JWCCreator
{
    /// <summary>
    /// fm_addctrl.xaml 的交互逻辑
    /// </summary>
    public partial class fm_addctrl : Window
    {
        public fm_addctrl()
        {
            InitializeComponent();
            lst_ctrls.DisplayMemberPath = "FName";
            lst_ctrls.SelectionChanged += lst_ctrls_SelectionChanged;
        }

        void lst_ctrls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CtrlListboxItem item = (CtrlListboxItem)lst_ctrls.SelectedItem;
            lbl_name.Content = item.FName;
            lbl_fullname.Content = item.FullName;
            txt_desc.Text = item.Description;
        }

        public event Action<JWCControl> OnConfirmAdd;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var lst = JWCControlFactory.GetAllControls();
            foreach(var pai in lst)
            {
                CtrlListboxItem item = new CtrlListboxItem();
                item.FullName = pai.Key;
                Type jctp = pai.Value;
                JWCControlDescAttribute attr = (JWCControlDescAttribute)Attribute.GetCustomAttribute(jctp, typeof(JWCControlDescAttribute));
                if (attr == null)
                    item.FName = item.FullName;
                else
                    item.FName = attr.FriendlyName;
                if (attr == null || attr.Description == null || attr.Description.Length < 1)
                    item.Description = "该控件无描述文本";
                else
                    item.Description = attr.Description;
                lst_ctrls.Items.Add(item);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void chk_top_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = chk_top.IsChecked ?? false;
            JWCSerializer<object> j = new JWCSerializer<object>();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            if (OnConfirmAdd != null)
            {
                CtrlListboxItem item = lst_ctrls.SelectedItem as CtrlListboxItem;
                if (item == null)
                    return;
                JWCControl jc = JWCControlFactory.CreateInstance(item.FullName);
                jc.IsEditMode = true;
                jc.Init(true);
                OnConfirmAdd(jc);
            }
        }

       
    }

    class CtrlListboxItem
    {
        public string FName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
    }
}
