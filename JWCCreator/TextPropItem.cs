using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using JWCControlLib;
using System.Reflection;
using System.Windows.Controls;



namespace JWCCreator
{
    class TextPropItem:IPropItem
    {
        protected JWCControl _Ctrl;
        protected PropertyInfo _PI;
        protected TextBox _Txtbox;
        protected string _HelpString;
        protected string _OldValue;

        public event Action<object, PropertyInfo, object> OnPropValueChanged;
        public event Action<string> TimeToShowHelpString;

        public TextPropItem(JWCControl jc,PropertyInfo pi)
        {
            _Ctrl = jc;
            _PI = pi;
        }

        public FrameworkElement GetControl()
        {
            DockPanel dp = new DockPanel();
            dp.Margin = new Thickness(6);
            Label lbl = new Label();
            _Txtbox = new TextBox();
            dp.Children.Add(lbl);
            dp.Children.Add(_Txtbox);
            DockPanel.SetDock(lbl, Dock.Left);
            DockPanel.SetDock(_Txtbox, Dock.Left);
            var info = PropManager.GetJWCPropDetail(_PI);
            lbl.Content = info.FriendlyName + ":";
            _HelpString = info.Describe;
            _Txtbox.Text = _Ctrl.GetProp(_PI).ToString();
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
                _Txtbox.Text = _OldValue;
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
            try
            {
                object val = Convert.ChangeType(_Txtbox.Text, _PI.PropertyType);
                _Ctrl.SetProp(_PI, val);
            }
            catch(Exception ex)
            {
                MessageBox.Show("输入值不正确，设置将不会生效！","提示", MessageBoxButton.OK,MessageBoxImage.Error);
                _Txtbox.Text = _OldValue;
            }
            
            if (OnPropValueChanged != null)
                OnPropValueChanged(_Ctrl, _PI, _Txtbox.Text);
        }

        void _Txtbox_LostFocus(object sender, RoutedEventArgs e)
        {
            ChangeProp();
        }

        public void SetPropShowValue(object obj)
        {
            _Txtbox.Dispatcher.Invoke((Action)delegate() { _Txtbox.Text = obj.ToString(); });
        }
    }
}
