using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using JWCControlLib;
using System.Collections.ObjectModel;

namespace JWCCreator
{
    class PropManager
    {
        private ComboBox _CombInd;
        private StackPanel _PropPanel;
        private TextBlock _TxtHelp;
        private static readonly SolidColorBrush color1;
        private static readonly SolidColorBrush color2;

        private Action<object> SetMarginFun;
        private Action<object> SetSizeFun;

         static PropManager()
        {
            color1 = new SolidColorBrush(Color.FromArgb(255, 0xB0, 0xF0, 0xF0));
            color2 = new SolidColorBrush(Color.FromArgb(255, 0x9A, 0xDE, 0xF7));
        }

        public PropManager(ComboBox cc,StackPanel pan,TextBlock hp)
        {
            _CombInd = cc;
            _PropPanel = pan;
            _TxtHelp = hp;
        }

        private string GetJWCCFullName(JWCControl ctrl)
        {
            Type tp = ctrl.GetType();
            return tp.FullName;
        }

        public void RefreshStageProp(Stage st)
        {
            _CombInd.SelectedItem = null;

            _PropPanel.Children.Clear();
        }

        public void RefreshCurrName(JWCControl ctrl)
        {
            //string fn = GetJWCCFullName(ctrl);
            //string name = string.IsNullOrEmpty(ctrl.Name)?"[无名称]":ctrl.Name;
            //_Indicator.Dispatcher.Invoke(
            //    (Action)(delegate()
            //            {
            //                _Indicator.Content = string.Format("{0}({1})", name, fn);
            //            })
            //    );
            _CombInd.SelectedItem = null;
            _CombInd.SelectedItem = ctrl;
            
        }

        public void RefreshPropList(JWCControl ctrl)
        {
            _PropPanel.Children.Clear();
            Type tp = ctrl.GetType();
            bool cl = false;
            foreach(PropertyInfo pi in tp.GetProperties())
            {
                if(Attribute.IsDefined(pi,typeof(PropDiscribeAttribute)))
                {
                    PropDiscribeAttribute pat = pi.GetCustomAttributes(typeof(PropDiscribeAttribute), true)[0] as PropDiscribeAttribute;
                    APropItem itm = PropItemFactory.GetPropItem(pat, ctrl, pi);
                    var showitm = itm.GetControl();
                    Grid grd = new Grid();
                    grd.Background = cl ? color1 : color2;
                    cl = !cl;
                    grd.Children.Add(showitm);
                    _PropPanel.Children.Add(grd);
                    itm.TimeToShowHelpString += itm_TimeToShowHelpString;
                    if(pi.Name == "Name")
                        itm.OnPropValueChanged += itm_OnPropValueChanged;
                    else if (pi.Name == "Size")
                        this.SetSizeFun = itm.SetPropShowValue;
                    else if (pi.Name == "Margin")
                        this.SetMarginFun = itm.SetPropShowValue;
                    
                }
            }
        }

        public void RefreshMargin(double x,double y)
        {
            SetMarginFun(new object[] { x, y, 0, 0 });
        }
        
        public void RefreshSize(double x,double y)
        {
            SetSizeFun(new object[] { x, y });
        }

        void itm_OnPropValueChanged(APropItem sender, JWCControl arg1, PropertyInfo arg2, object arg3)
        {
            JWCControl jc = arg1 as JWCControl;
            if (jc == null)
                return;
            if(jc.Focused)
                RefreshCurrName(jc);
        }

        void itm_TimeToShowHelpString(string obj)
        {
            _TxtHelp.Dispatcher.Invoke((Action)delegate() { _TxtHelp.Text = obj; });
        }

        
    }
}
