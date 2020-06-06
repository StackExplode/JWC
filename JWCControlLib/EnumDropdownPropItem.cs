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
    class EnumDropdownPropItem:APropItem
    {

        public override event OnPropValueChangedDele OnPropValueChanged;

        public override event Action<string> TimeToShowHelpString;

        int _oldIndex;
        ComboBox _Comb;

        public EnumDropdownPropItem(JWCControl jc, PropertyInfo pi, PropDiscribeAttribute attr)
        {
            _Ctrl = jc;
            _PI = pi;
            _Attr = attr;
            AutoSet = true;
        }

        public override System.Windows.FrameworkElement GetControl()
        {
            DockPanel dp = new DockPanel();
            dp.Margin = new Thickness(6);
            Label lbl = new Label();
            _Comb = new ComboBox();
            lbl.VerticalAlignment = _Comb.VerticalAlignment = VerticalAlignment.Center;
            dp.Children.Add(lbl);
            dp.Children.Add(_Comb);
            DockPanel.SetDock(lbl, Dock.Left);
            DockPanel.SetDock(_Comb, Dock.Left);
            var info = _Attr;

            lbl.Content = info.FriendlyName + ":";
            _HelpString = info.Describe;


            var lst = Enum.GetValues(_PI.PropertyType);
            _Comb.ItemsSource = lst;
            Enum curr = (Enum)_Ctrl.GetProp(_PI);
            _Comb.SelectedItem = curr;
            _oldIndex = _Comb.SelectedIndex;
            _Comb.SelectionChanged += _Comb_SelectionChanged;
            _Comb.GotFocus += _Comb_GotFocus;

            return dp;
        }

        void _Comb_GotFocus(object sender, RoutedEventArgs e)
        {
            _oldIndex = _Comb.SelectedIndex;
            if (TimeToShowHelpString != null)
                TimeToShowHelpString(_HelpString);
        }

        void _Comb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AutoSet)
            {
                _Ctrl.SetProp(_PI, _Comb.SelectedItem);
            }
            _oldIndex = _Comb.SelectedIndex;
            if (OnPropValueChanged != null)
                OnPropValueChanged(this, _Ctrl, _PI, _Comb.SelectedItem);
        }

        public override void SetPropShowValue(object obj)
        {
            throw new NotImplementedException();
        }

        public override void ResumeValue()
        {
            _Comb.SelectedIndex = _oldIndex;
        }
    }
}
