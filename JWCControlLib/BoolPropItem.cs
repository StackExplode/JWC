using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace JWCControlLib
{
    public class BoolPropItem:APropItem
    {
        public override event OnPropValueChangedDele OnPropValueChanged;
        public override event Action<string> TimeToShowHelpString;


        protected CheckBox _Chkbox;
        protected bool _OldValue;


        public BoolPropItem(IPropGWAble jc, PropertyInfo pi, PropDiscribeAttribute attr)
        {
            _Ctrl = jc;
            _PI = pi;
            _Attr = attr;
            AutoSet = true;
        }

        public override void ResumeValue()
        {
            _Chkbox.IsChecked = _OldValue;
        }

        public override FrameworkElement GetControl()
        {
            DockPanel dp = new DockPanel();
            dp.Margin = new Thickness(6);
            Label lbl = new Label();
            _Chkbox = new CheckBox();
            lbl.VerticalAlignment =  _Chkbox.VerticalAlignment = VerticalAlignment.Center;
            dp.Children.Add(lbl);
            dp.Children.Add(_Chkbox);
            DockPanel.SetDock(lbl, Dock.Left);
            DockPanel.SetDock(_Chkbox, Dock.Left);
            var info =  _Attr;
            lbl.Content = info.FriendlyName + ":";
            _HelpString = info.Describe;
            _Chkbox.IsChecked = Convert.ToBoolean(_Ctrl.GetProp(_PI));
            _Chkbox.LostFocus += _Txtbox_LostFocus;
            _Chkbox.GotFocus += _Txtbox_GotFocus;
            _Chkbox.Click +=_Chkbox_Click;
            return dp;
        }
        void ChangeProp()
        {
            if(AutoSet)
            {
                try
                {
                    object val = Convert.ChangeType(_Chkbox.IsChecked, _PI.PropertyType);
                    _Ctrl.SetProp(_PI, val);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("输入值不正确，设置将不会生效！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    ResumeValue();
                }
            }
            
            if (OnPropValueChanged != null)
                OnPropValueChanged(this,_Ctrl, _PI, _Chkbox.IsChecked);
        }


        void _Txtbox_GotFocus(object sender, RoutedEventArgs e)
        {
            _OldValue = (bool)_Chkbox.IsChecked;
            if (TimeToShowHelpString != null)
                TimeToShowHelpString(_HelpString);
        }

        void _Chkbox_Click(object sender, RoutedEventArgs e)
        {
 	        ChangeProp();
        }

         void _Txtbox_LostFocus(object sender, RoutedEventArgs e)
        {
            ChangeProp();
        }

         public override void SetPropShowValue(object obj)
        {
            _Chkbox.Dispatcher.Invoke((Action)delegate() { _Chkbox.IsChecked = Convert.ToBoolean(obj); });
        }

    }
}
