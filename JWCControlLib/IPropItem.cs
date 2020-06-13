using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using System.Windows;
using System.Windows.Controls;


namespace JWCControlLib
{
    public delegate void OnPropValueChangedDele(APropItem sender,IPropGWAble setee,PropertyInfo pi,object value);
    public abstract class APropItem
    {
        public abstract event OnPropValueChangedDele OnPropValueChanged;
        public abstract event Action<string> TimeToShowHelpString;
        public abstract FrameworkElement GetControl();
        public abstract void SetPropShowValue(object obj);
        public abstract void ResumeValue();
        public bool AutoSet { get; set; }
        public object AttrParam { get { return _Attr.Param; } }
        protected IPropGWAble _Ctrl;
        protected PropertyInfo _PI;
        protected string _HelpString;
        protected PropDiscribeAttribute _Attr;
    }
}
