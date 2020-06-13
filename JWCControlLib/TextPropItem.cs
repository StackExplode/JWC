using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using JWCControlLib;
using System.Reflection;
using System.Windows.Controls;



namespace JWCControlLib
{
    public class TextPropItem:APropItem
    {
        public override event OnPropValueChangedDele OnPropValueChanged;
        public override event Action<string> TimeToShowHelpString;
        protected string _OldValue;
        protected TextBox _Txtbox;
        public TextPropItem(IPropGWAble jc, PropertyInfo pi, PropDiscribeAttribute attr)
        {
            _Ctrl = jc;
            _PI = pi;
            _Attr = attr;
            AutoSet = true;
        }

        public override void ResumeValue()
        {
            _Txtbox.Text = _OldValue;
        }

        public override FrameworkElement GetControl()
        {
            DockPanel dp = new DockPanel();
            dp.Margin = new Thickness(6);
            Label lbl = new Label();
            _Txtbox = new TextBox();
            lbl.VerticalAlignment =  _Txtbox.VerticalAlignment = VerticalAlignment.Center;
            dp.Children.Add(lbl);
            dp.Children.Add(_Txtbox);
            DockPanel.SetDock(lbl, Dock.Left);
            DockPanel.SetDock(_Txtbox, Dock.Left);
            _Txtbox.TextWrapping = TextWrapping.NoWrap;
            var info =  _Attr;
            
            lbl.Content = info.FriendlyName + ":";
            _HelpString = info.Describe;
            var pp = _Ctrl.GetProp(_PI);
            _Txtbox.Text = pp == null ? "" : pp.ToString();
            //_Txtbox.MaxWidth = _Txtbox.ActualWidth;
            _Txtbox.LostFocus += _Txtbox_LostFocus;
            _Txtbox.GotFocus += _Txtbox_GotFocus;
            _Txtbox.KeyUp += _Txtbox_KeyUp;
            return dp;
        }

        void _Txtbox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                ChangeProp();
            }
            else if(e.Key == System.Windows.Input.Key.Escape)
            {
                ResumeValue();
            }
        }

        void _Txtbox_GotFocus(object sender, RoutedEventArgs e)
        {
            _OldValue = _Txtbox.Text;
            if (TimeToShowHelpString != null)
                TimeToShowHelpString(_HelpString);
        }

        void ChangeProp()
        {
            if(AutoSet)
            {
                try
                {
                    object val = Convert.ChangeType(_Txtbox.Text, _PI.PropertyType);
                    _Ctrl.SetProp(_PI, val);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("输入值不正确，设置将不会生效！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    ResumeValue();
                }
            }
            
            if (OnPropValueChanged != null)
                OnPropValueChanged(this,_Ctrl, _PI, _Txtbox.Text);
        }

        void _Txtbox_LostFocus(object sender, RoutedEventArgs e)
        {
            ChangeProp();
        }

        public override void SetPropShowValue(object obj)
        {
            _Txtbox.Dispatcher.Invoke((Action)delegate() { _Txtbox.Text = obj.ToString(); });
        }


   
    }
}
