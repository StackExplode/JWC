using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace JWCControlLib
{
    public abstract class JWCControl : System.Windows.Controls.UserControl
    {
        internal enum MouseState { Up, MoveDown, ResizeDown }

        private MouseState Mousedown;
        private bool _mouseforresize = false;
        private ThumbAdder TMA;
        private double CurX = 0;
        private double CurY = 0;
        protected bool _selecting = false;
        private Border _selector { get; set; }
        protected Grid _maingrd { get; set; }
        public new Panel Parent { set; get; }
        public bool IsEditMode { get; set; }

        public event Action<object, bool, DragCompletedEventArgs> OnMovedOrResized;

        public new event Action<object> OnGotFocus;

        public new event Action<object> OnLostFocus;

        [PropDiscribe(CreatorPropType.Text, "Name", "控件的名称，其不能以数字开头且不包含特殊符号，一般它是唯一的")]
        [Outputable]
        public new string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }


        

        public string PropDispName
        {
            get
            {
                string fn = this.GetType().FullName;
                string name = string.IsNullOrEmpty(this.Name) ? "[无名称]" : this.Name;
                return name + "(" + fn + ")";
            }
        }

        //[PropDiscribe(CreatorPropType.EnumDropDown, "通信标识类型", "控制通信时使用哪种唯一标识", true)]
        //[Outputable]
        //public ComuIDType ComType { get; set; }

        //[PropDiscribe(CreatorPropType.Text, "ID", "在参与通信时的唯一标识符，其只能由数字组成")]
        //[Outputable]
        //public int ID { get; set; }

        public JWCControl()
        {
            IsEditMode = false;
           
            //this.DataContext = this;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.PreviewMouseLeftButtonDown += JWCControl_MouseLeftButtonDown;
        }

        [Obsolete]
        protected void BindSelector(Border bd)
        {
            _selector = bd;
            //this.PreviewMouseLeftButtonDown += JWCControl_MouseLeftButtonDown;
            //this.PreviewMouseLeftButtonUp += JWCControl_MouseRightButtonUp;
            //this.PreviewMouseMove += JWCControl_MouseMove;
        }

        protected void BindMainGrid(Grid grd)
        {
            //((FrameworkElement)Content).DataContext = this;
            _maingrd = grd;
            TMA = new ThumbAdder(this, grd);
            TMA.InitThumbs();
            TMA.OnMoveFinished += (s, e) =>
                {
                    if (OnMovedOrResized != null)
                        OnMovedOrResized(this, false, e);
                };
            TMA.OnResizeFinished += (s, e) =>
            {
                if (OnMovedOrResized != null)
                    OnMovedOrResized(this, true, e);
                if (OnMovedOrResized != null)
                    OnMovedOrResized(this, false, e);
            };
        }

        public void SetProp(PropertyInfo pi, object val)
        {
            if (Attribute.IsDefined(pi, typeof(RedirectGSAttribute)))
            {
                object[] attrs = pi.GetCustomAttributes(typeof(RedirectGSAttribute), true);
                RedirectGSAttribute attr = (RedirectGSAttribute)attrs[0];
                var meth = this.GetType().GetMethod(attr.Fun, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                meth.Invoke(this, new object[] { true, val });
            }
            else
                pi.SetValue(this, val, null);
        }

        public object GetProp(PropertyInfo pi)
        {
            if (Attribute.IsDefined(pi, typeof(RedirectGSAttribute)))
            {
                object[] attrs = pi.GetCustomAttributes(typeof(RedirectGSAttribute), true);
                RedirectGSAttribute attr = (RedirectGSAttribute)attrs[0];
                var meth = this.GetType().GetMethod(attr.Fun, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                object val = meth.Invoke(this, new object[] { false, null });
                return val;
            }
            else
                return pi.GetValue(this, null);
        }

        public JControlOutputData OutputProperty()
        {
            JControlOutputData rst = new JControlOutputData();
            PropertyInfo[] pis = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in pis)
            {
                if (Attribute.IsDefined(pi, typeof(OutputableAttribute)))
                {
                    rst.Add(pi.Name, GetProp(pi));
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
                    if (dic.ContainsKey(pi.Name))
                    {
                        val = dic[pi.Name];
                        SetProp(pi, val);
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

        [PropDiscribe(CreatorPropType.Multi,"位置","设置控件的位置，一般只有左和上有效")]
        [SubProp(CreatorPropType.Text, "左", "控件到左边的距离", 0)]
        [SubProp(CreatorPropType.Text, "上", "控件到上边的距离", 1)]
        [SubProp(CreatorPropType.Text, "右", "控件到右边的距离，系统优先考虑左边距离", 2)]
        [SubProp(CreatorPropType.Text, "下", "控件到下边的距离，系统优先考虑上边距离", 3)]
        [RedirectGS("GSR_Margin")]
        public new Thickness Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; }
        }

        protected object[] GSR_Margin(bool isset,object[] val)
        {
            JWCControl me = this;
 
            if (isset)
            {
                double left = (int)Convert.ToDouble(val[0]);
                double top = (int)Convert.ToDouble(val[1]);
                double right = (int)Convert.ToDouble(val[2]);
                double bottom = (int)Convert.ToDouble(val[3]);
                me.Margin = new Thickness(left, top, right, bottom);
                return null;
            }
            else
            {
                return new object[] { (int)me.Margin.Left, (int)me.Margin.Top, (int)me.Margin.Right, (int)me.Margin.Bottom };
            }
        }

        [PropDiscribe(CreatorPropType.Multi, "尺寸", "设置控件的尺寸，一般只有左和上有效")]
        [SubProp(CreatorPropType.Text, "宽度", "控件的宽度", 0)]
        [SubProp(CreatorPropType.Text, "高度", "控件的高度", 1)]
        [RedirectGS("GSR_Size")]
        public Size Size
        {
            get { return new Size(base.Width, base.Height); }
            set
            {
                base.Width = value.Width;
                base.Height = value.Height;
            }
        }

        protected object[] GSR_Size(bool isset,object[] val)
        {
            if(isset)
            {
                int w = (int)Convert.ToDouble(val[0]);
                int h = (int)Convert.ToDouble(val[1]);
                this.Size = new Size(w, h);
                return null;
            }
            else
            {
                return new object[] { (int)this.Width, (int)this.Height };
            }
        }

        [PropDiscribe(CreatorPropType.Text, "层叠位置", "设置其z轴层叠的高度")]
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
            //foreach (Delegate d in OnMoved.GetInvocationList())
            //{
            //    OnMoved -= (Action<object>)d;
            //}
        }

        public void GetFocus()
        {
            //Border bd = _selector;
            //_selector.Background = new SolidColorBrush(Color.FromArgb(90, 0, 0, 255));
            //bd.BorderBrush = new SolidColorBrush(Colors.Red);
            //bd.BorderThickness = new Thickness(2);

            TMA.AppendMoveThumb();

            _selecting = true;

            if (this.OnGotFocus != null)
                this.OnGotFocus(this);
        }

        public void LoseFocus()
        {
            //_selector.Background = null;
            //_selector.BorderBrush = null;

            TMA.RemoveMoveThumb();

            _selecting = false;
            if (this.OnLostFocus != null)
                this.OnLostFocus(this);
        }

        [Obsolete("不应该再使用show了！", true)]
        public void Show()
        {
            Parent.Children.Add(this);
        }

        protected virtual void JWCControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsEditMode || !_selecting)
                return;

            e.Handled = true;

            if (Mousedown == MouseState.MoveDown)
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
            else if (Mousedown == MouseState.ResizeDown)
            {
                this.CaptureMouse();
                Point pTemp = Mouse.GetPosition(Parent);
                double diff_x = pTemp.X - CurX;
                double diff_y = pTemp.Y - CurY;
                if (this.Width + diff_x > 10)
                    this.Width += diff_x;
                if (this.Height + diff_y > 10)
                    this.Height += diff_y;
            }
            else
            {
                //this.CaptureMouse();
                Point pTemp = e.GetPosition(this);
                if (pTemp.X > 0 && pTemp.X < 4 && pTemp.Y > 0 && pTemp.Y < 4)
                {
                    _mouseforresize = true;
                    ((FrameworkElement)this).Cursor = Cursors.SizeNWSE;
                }
                else
                    _mouseforresize = false;
                //this.ReleaseMouseCapture();
            }
        }

        protected virtual void JWCControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsEditMode)
                return;
            e.Handled = true;
            if (Mousedown != MouseState.Up)
            {
                Mousedown = MouseState.Up;
                if (sender is FrameworkElement)
                {
                    ((FrameworkElement)this).Cursor = Cursors.Arrow;
                    this.ReleaseMouseCapture();
                    //if (OnMoved != null)
                    //    OnMoved(this);
                }
            }
        }

        protected virtual void JWCControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsEditMode)
                return;

            if (!_selecting)
            {
                GetFocus();
                e.Handled = true;
                return;
            }

            //if(_selecting)
            //{
            //    Point pt = Mouse.GetPosition((FrameworkElement)this);
            //    CurX = pt.X;
            //    CurY = pt.Y;
            //    if (_mouseforresize)
            //    {
            //        Mousedown = MouseState.ResizeDown;

            //    }
            //    else
            //    {
            //        this.Cursor = Cursors.SizeAll;
            //        Mousedown = MouseState.MoveDown;
            //    }

            //}
        }

        public static PropDiscribeAttribute GetJWCPropDetail(PropertyInfo pi)
        {
            if (Attribute.IsDefined(pi, typeof(PropDiscribeAttribute)))
            {
                return pi.GetCustomAttributes(typeof(PropDiscribeAttribute), true)[0] as PropDiscribeAttribute;
            }
            return null;
        }

        public static PropDiscribeAttribute GetJWCPropDetail(string name)
        {
            PropertyInfo pi = typeof(JWCControl).GetProperty("name");
            if (Attribute.IsDefined(pi, typeof(PropDiscribeAttribute)))
            {
                return pi.GetCustomAttributes(typeof(PropDiscribeAttribute), true)[0] as PropDiscribeAttribute;
            }
            return null;
        }
    }

    [Serializable]
    public class JControlOutputData : Hashtable
    {
        public JControlOutputData()
            : base()
        {
        }

        protected JControlOutputData(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}