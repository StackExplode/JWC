using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

using JWCCommunicationLib;
using System.Reflection;
using JWCControlLib;

namespace JWCCreator
{
    class ComAdPropManager
    {
        private StackPanel _PropPanel;
        private TextBlock _TxtHelp;
        private static readonly SolidColorBrush color1;
        private static readonly SolidColorBrush color2;

        static ComAdPropManager()
        {
            color1 = new SolidColorBrush(Color.FromArgb(255, 0xe1, 0x98, 0xb4));
            color2 = new SolidColorBrush(Color.FromArgb(255, 0xee, 0xbb, 0xcb));
        }

        public ComAdPropManager(StackPanel st,TextBlock tx)
        {
            _TxtHelp = tx;
            _PropPanel = st;
        }

        public void RefreshComPropList(IPropGWAble com)
        {
            _PropPanel.Children.Clear();
            Type tp = com.GetType();
            bool cl = false;
            foreach (PropertyInfo pi in tp.GetProperties())
            {
                if (Attribute.IsDefined(pi, typeof(PropDiscribeAttribute)))
                {
                    PropDiscribeAttribute pat = pi.GetCustomAttributes(typeof(PropDiscribeAttribute), true)[0] as PropDiscribeAttribute;
                    APropItem itm = PropItemFactory.GetPropItem(pat, com, pi);
                    var showitm = itm.GetControl();
                    Grid grd = new Grid();
                    grd.Background = cl ? color1 : color2;
                    cl = !cl;
                    grd.Children.Add(showitm);
                    _PropPanel.Children.Add(grd);
                    itm.TimeToShowHelpString += itm_TimeToShowHelpString;
                }
            }
            if (_PropPanel.Children.Count == 0)
                _TxtHelp.Text = "该控件没有可以设置的属性。";
        }

        void itm_TimeToShowHelpString(string obj)
        {
            _TxtHelp.Dispatcher.Invoke((Action)delegate() { _TxtHelp.Text = obj; });
        }

    }
}
