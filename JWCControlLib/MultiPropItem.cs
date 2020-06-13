using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using JWCControlLib;
using System.Reflection;
using System.Windows.Controls;
using System.Collections;



namespace JWCControlLib
{
    class MultiPropItem:APropItem
    {

        public override event OnPropValueChangedDele OnPropValueChanged;
        public override event Action<string> TimeToShowHelpString;


        protected string _OldValue;
        private object[] _Values;
        private APropItem[] _SubItems;


        public MultiPropItem(IPropGWAble jc, PropertyInfo pi, PropDiscribeAttribute attr)
        {
            _Ctrl = jc;
            _PI = pi;
            _Attr = attr;
        }

        public override FrameworkElement GetControl()
        {
            DockPanel dp = new DockPanel();
            dp.Margin = new Thickness(6);
            Label lbl = new Label();
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.Content = _Attr.FriendlyName + ":";
            DockPanel.SetDock(lbl, Dock.Left);
            dp.Children.Add(lbl);
            StackPanel sp = new StackPanel();
            dp.Children.Add(sp);

            SubPropAttribute[] attrs = (SubPropAttribute[])_PI.GetCustomAttributes(typeof(SubPropAttribute), true);
            _Values = (object[])_Ctrl.GetProp(_PI);
            _SubItems = new APropItem[_Values.Length];
            foreach(var x in attrs)
            {
                PropDiscribeAttribute a = new PropDiscribeAttribute(x.ShowType, x.FriendlyName, x.Describe, x.Param);
                APropItem itm = PropItemFactory.GetPropItem(a, _Ctrl, _PI);
                sp.Children.Add(itm.GetControl());
                itm.SetPropShowValue(_Values[(int)itm.AttrParam]);
                itm.TimeToShowHelpString += itm_TimeToShowHelpString;
                itm.AutoSet = false;
                itm.OnPropValueChanged += itm_OnPropValueChanged;
                _SubItems[(int)itm.AttrParam] = itm;
            }
            return dp;
        }

        void itm_TimeToShowHelpString(string obj)
        {
            if (TimeToShowHelpString != null)
                TimeToShowHelpString(obj);
        }


        void itm_OnPropValueChanged(APropItem sender, IPropGWAble setee, PropertyInfo pi, object value)
        {
            int index = (int)sender.AttrParam;
            _Values[index] = value;
            setee.SetProp(pi, _Values);
            
        }

        public override void SetPropShowValue(object obj)
        {
            object[] objs = (object[])obj;
            foreach (APropItem it in _SubItems)
            {
                int index = (int)it.AttrParam;
                it.SetPropShowValue(objs[index]);
                _Values[index] = objs[index];
            }
                
        }

        public override void ResumeValue()
        {
             
        }
    }
}
