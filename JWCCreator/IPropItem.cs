using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows;


namespace JWCCreator
{
    interface IPropItem
    {
        event Action<object, PropertyInfo, object> OnPropValueChanged;
        event Action<string> TimeToShowHelpString;
        FrameworkElement GetControl();
        void SetPropShowValue(object obj);
    }
}
