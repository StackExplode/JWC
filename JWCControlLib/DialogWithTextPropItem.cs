using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace JWCControlLib
{
    class DialogWithTextPropItem:APropItem
    {
        

        public override event OnPropValueChangedDele OnPropValueChanged;
        public override event Action<string> TimeToShowHelpString;
      
        protected TextBox _Txtbox;
        
        protected object OldVal;
        public object CurrValue { get; private set; }

        public DialogWithTextPropItem(JWCControl jc,PropertyInfo pi,PropDiscribeAttribute attr)
        {
            _Ctrl = jc;
            _PI = pi;
            _Attr = attr;
            AutoSet = true;

        }

        private string ConvertProp(object pp)
        {
            object[] parr = pp as object[];
            if (parr != null)
                return parr.ToArrString();
            else
                return pp.ToString();
        }

        public override System.Windows.FrameworkElement GetControl()
        {
            DockPanel dp = new DockPanel();
            dp.LastChildFill = true;
            Button _Btn;
            dp.Margin = new Thickness(6);
            Label lbl = new Label();
            _Txtbox = new TextBox();
            _Btn = new Button();
            _Btn.VerticalAlignment = VerticalAlignment.Center;
            lbl.VerticalAlignment = _Txtbox.VerticalAlignment = VerticalAlignment.Center;
            
            _Txtbox.IsReadOnly = true;
            dp.Children.Add(lbl);
            dp.Children.Add(_Btn);
            dp.Children.Add(_Txtbox);
            
            DockPanel.SetDock(lbl, Dock.Left);
            DockPanel.SetDock(_Txtbox, Dock.Left);
            DockPanel.SetDock(_Btn, Dock.Right);
            
            
            lbl.Content = _Attr.FriendlyName + ":";
            _HelpString = _Attr.Describe;
            CurrValue = _Ctrl.GetProp(_PI);
            _Txtbox.Text = ConvertProp(CurrValue);
            object [] arr = base.AttrParam as object[];
            string cap = arr[0].ToString();
            _Btn.Content = cap;
            _Btn.Width = cap.Length * 12 + 10;
            _Btn.Click += _Btn_Click;
            _Txtbox.GotFocus += _Txtbox_GotFocus;

            return dp;
        }

        void _Txtbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TimeToShowHelpString != null)
                TimeToShowHelpString(_HelpString);
        }

        void _Btn_Click(object sender, RoutedEventArgs e)
        {
            OldVal = CurrValue;
            object[] arr = base.AttrParam as object[];
            Type tp = arr[1] as Type;
            PropDialog dia = (PropDialog)Activator.CreateInstance(tp);
            object[] pars = arr[2] as object[];
            object rt = null;
            System.Windows.Forms.DialogResult dr = dia.ShowDialog(out rt,pars);
            if(dr == System.Windows.Forms.DialogResult.OK)
            {
                CurrValue = rt;
                SetPropShowValue(rt);
                _Ctrl.SetProp(_PI, rt);
                if (OnPropValueChanged != null)
                    OnPropValueChanged(this, _Ctrl, _PI, rt);
            }
            
        }

        public override void SetPropShowValue(object obj)
        {
            _Txtbox.Dispatcher.Invoke((Action)delegate() { _Txtbox.Text = ConvertProp(obj); });
        }

        public override void ResumeValue()
        {
            _Txtbox.Text = ConvertProp(OldVal);
            _Ctrl.SetProp(_PI, OldVal);
        }
    }

    public static class IEExtender
    {
        public static string ToArrString<T>(this IEnumerable<T> arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach(T x in arr)
            {
                sb.Append(x);
                sb.Append(",");
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static void FromStringArray(this object[] arr,string str)
        {
            string sub = str.Substring(1, str.Length - 2);
            string[] ss = sub.Split(',');
            int num = ss.Length < arr.Length ? ss.Length : arr.Length;
            for(int i=0;i<num;i++)
            {
                arr[i] = ss[i];
            }
        }
    }
}
