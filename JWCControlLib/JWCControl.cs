using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
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

namespace JWCControlLib
{
   
    public class JWCControl :  System.Windows.Controls.UserControl
    {
        public event Action<object> OnMoved;
        public event Action<object> OnGotFocus;
        public event Action<object> OnLostFocus;

        private bool Mousedown,_mousedd=false;
        private double CurX = 0;
        private double CurY = 0;
        private bool _selecting = false;

        protected Border _selector;

        public new Panel Parent { set; get; }
        public bool IsEditMode { get; set; }
        
        public JWCControl()
        {
            IsEditMode = false;
            this.PreviewMouseLeftButtonDown += JWCControl_MouseRightButtonDown;
            this.PreviewMouseLeftButtonUp += JWCControl_MouseRightButtonUp;
            this.PreviewMouseMove += JWCControl_MouseMove;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            
        }


        public JControlOutputData OutputProperty()
        {
            JControlOutputData rst = new JControlOutputData();
            PropertyInfo[] pis = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach(PropertyInfo pi in pis)
            {
                if(Attribute.IsDefined(pi,typeof(OutputableAttribute)))
                {
                    if (Attribute.IsDefined(pi, typeof(RedirectGSAttribute)))
                    {
                        object[] attrs = pi.GetCustomAttributes(typeof(RedirectGSAttribute), true);
                        RedirectGSAttribute attr = (RedirectGSAttribute)attrs[0];
                        var meth = this.GetType().GetMethod(attr.Fun, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        object val = meth.Invoke(this, new object[] { false, null });
                        rst.Add(pi.Name,val);
                    }
                    else
                        rst.Add(pi.Name, pi.GetValue(this, null));
                }
            }
            return rst;
        }

        public void InputProperty(JControlOutputData dic)
        {
            PropertyInfo[] pis = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in pis)
            {
                if (Attribute.IsDefined(pi, typeof(OutputableAttribute)))
                {
                    object val = null;
                    if(dic.TryGetValue(pi.Name,out val))
                    {
                        if (Attribute.IsDefined(pi, typeof(RedirectGSAttribute)))
                        {
                            object[] attrs = pi.GetCustomAttributes(typeof(RedirectGSAttribute), true);
                            RedirectGSAttribute attr = (RedirectGSAttribute)attrs[0];
                            var meth = this.GetType().GetMethod(attr.Fun, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                            meth.Invoke(this, new object[] { true, val });
                        }
                        else
                            pi.SetValue(this, val, null);
                    }
                }
            }
        }


        public bool Focused
        {
            get
            {
                return _selecting;
            }
            set
            {
                if (value)
                    GetFocus();
                else
                    LoseFocus();
            }
        }

        [Outputable]
        public int ZIndex
        {
            set
            {
                Canvas.SetZIndex(this, value);
            }
            get
            {
                return Canvas.GetZIndex(this);
            }
        }



        public void ClearEditorEvents()
        {
            foreach (Delegate d in OnMoved.GetInvocationList())
            {
                OnMoved -= (Action<object>)d;
            }
        }

        public void GetFocus()
        {
            _selector.Background = new SolidColorBrush(Color.FromArgb(90, 0, 0, 255));
            _selecting = true;
 
            if (this.OnGotFocus != null)
                this.OnGotFocus(this);
        }

        public void LoseFocus()
        {
            _selector.Background = null;
            _selecting = false;
            if (this.OnLostFocus != null)
                this.OnLostFocus(this);
        }

        public void Show()
        {
            Parent.Children.Add(this);
        }


        protected virtual void JWCControl_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            if (Mousedown)
            {
                // 获取当前屏幕的光标坐标
                //Point pTemp = new Point(Cursor.Position.X, Cursor.Position.Y);
                // 转换成工作区坐标
                //pTemp = this.PointToClient(pTemp);
                
                this.CaptureMouse();
                Point pTemp = Mouse.GetPosition(Parent);
                // 定位事件源的 Location
                double diff_x = pTemp.X - CurX;
                double diff_y = pTemp.Y - CurY;
                
                FrameworkElement control = (FrameworkElement)this;
                control.Margin = new Thickness(diff_x, diff_y, 0, 0);
 
            }
            
        }

        protected virtual void JWCControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (!IsEditMode)
                return;
            if(Mousedown)
            {
                Mousedown = false;
                if (sender is FrameworkElement)
                {
                    ((FrameworkElement)this).Cursor = Cursors.Arrow;
                    this.ReleaseMouseCapture();
                    if (OnMoved != null)
                        OnMoved(this);
                }
            }      
        }

        
        protected virtual void JWCControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
           e.Handled = true;
            if (!IsEditMode)
                return;
            if (!_selecting)
            {
                GetFocus();
                return;
            }
                
            if(_selecting)
            {
                Point pt = Mouse.GetPosition((FrameworkElement)this);
                CurX = pt.X;
                CurY = pt.Y;
                Mousedown = true;
                this.Cursor = Cursors.SizeAll;
            }
            
        }
    }

    [Serializable]
    public class JControlOutputData:Dictionary<string,object>
    {
        public JControlOutputData() : base() { }
        protected JControlOutputData(SerializationInfo info, StreamingContext context): base(info, context) { }
    }
}
