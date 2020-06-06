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
    class Stage : IDisposable
    {
        private Grid grid_main;
        private JWCControl _Selecting_Ctrl = null;
        private ScrollViewer _Scroller = null;
        private ScaleTransform _Scaller = null;
        private Cursor grab_cur;
        private ComboBox _CombAll = null;
        private ObservableCollection<JWCControl> AllCtrls = new ObservableCollection<JWCControl>();

        public event Action<bool,object> OnSelectionChanged;
        public event Action<object, double, double> OnSelectedCtrlMoved;
        public event Action<object, double, double> OnSelectedCtrlResized;
        public JWCControl SelectedControl { get { return _Selecting_Ctrl; } }

        public Stage(Grid grd,ScrollViewer sv,ScaleTransform sc,ComboBox cbx)
        {
            _Scaller = sc;
            _Scroller = sv;
            grid_main = grd;
            _CombAll = cbx;
            _CombAll.ItemsSource = AllCtrls;
            grab_cur = new Cursor(new System.IO.MemoryStream(Properties.Resources.Grab));
            Initialization();
        }

        private void Initialization()
        {
            grid_main.MouseDown += (a, b) => {
                UnSelectControl();
            };
            _Scroller.PreviewMouseRightButtonDown += scrollv1_MouseRightButtonDown;
            _Scroller.PreviewMouseRightButtonUp += scrollv1_MouseRightButtonUp;
            _Scroller.PreviewMouseMove += scrollv1_MouseMove;
            _Scroller.ManipulationBoundaryFeedback += scrollv1_ManipulationBoundaryFeedback;
            _Scroller.PreviewMouseWheel += scrollv1_PreviewMouseWheel;
        }

        public void ClearAll(int w,int h)
        {
            if (_Selecting_Ctrl != null)
                _Selecting_Ctrl.LoseFocus();
            UnSelectControl();
            grid_main.Children.Clear();
            AllCtrls.Clear();
            grid_main.Width = w;
            grid_main.Height = h;
            grid_main.Background = new SolidColorBrush(Colors.White);
            ZoomReset();
        }

        private void UnSelectControl()
        {
            if (_Selecting_Ctrl != null)
                _Selecting_Ctrl.LoseFocus();
            _Selecting_Ctrl = null;
            if (OnSelectionChanged != null)
                OnSelectionChanged(false, grid_main);
        }

        public void RemoveSelecting()
        {
            if (_Selecting_Ctrl != null)
                RemoveCtonrol(_Selecting_Ctrl);
            UnSelectControl();
        }

        public void AddControl(JWCControl ctrl)
        {
            ctrl.Parent = grid_main;
            ctrl.OnGotFocus += xx_OnGotFocus;
            grid_main.Children.Add(ctrl);
            AllCtrls.Add(ctrl);
        }

        public void RemoveCtonrol(JWCControl ctrl)
        {
            grid_main.Children.Remove(ctrl);
            AllCtrls.Remove(ctrl);
        }


        void xx_OnGotFocus(object obj)
        {
            if (_Selecting_Ctrl != null)
            {
                _Selecting_Ctrl.OnMovedOrResized -= _Selecting_Ctrl_OnMovedOrResized;
                _Selecting_Ctrl.LoseFocus();
            }
                
            _Selecting_Ctrl = obj as JWCControl;
            _Selecting_Ctrl.OnMovedOrResized += _Selecting_Ctrl_OnMovedOrResized;
            if (OnSelectionChanged != null)
                OnSelectionChanged(true,obj);
        }

        void _Selecting_Ctrl_OnMovedOrResized(object arg1, bool rz, System.Windows.Controls.Primitives.DragCompletedEventArgs arg2)
        {
            if(!rz && OnSelectedCtrlMoved != null)
            {
                OnSelectedCtrlMoved(arg1, _Selecting_Ctrl.Margin.Left, _Selecting_Ctrl.Margin.Top);
            }
            if (rz && OnSelectedCtrlResized != null)
            {
                OnSelectedCtrlResized(arg1, _Selecting_Ctrl.Width, _Selecting_Ctrl.Height);
            }
        }

        bool Scr_Draging = false;
        private double Drag_Start_X = 0;
        private double Drag_Start_Y = 0;
        private double Drag_Offset_X = 0;
        private double Drag_Offset_Y = 0;

        private void scrollv1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Scr_Draging)
            {
                Point pt = Mouse.GetPosition(sender as FrameworkElement);
                Drag_Start_X = pt.X;
                Drag_Start_Y = pt.Y;
                Drag_Offset_X = (sender as ScrollViewer).HorizontalOffset;
                Drag_Offset_Y = (sender as ScrollViewer).VerticalOffset;
                Scr_Draging = true;
                (sender as ScrollViewer).CaptureMouse();
                ((FrameworkElement)sender).Cursor = grab_cur;
            }

        }

        private void scrollv1_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Scr_Draging = false;
            if (sender is FrameworkElement)
            {
                ((FrameworkElement)sender).Cursor = Cursors.Arrow;
                ((ScrollViewer)sender).ReleaseMouseCapture();
            }
        }

        private void scrollv1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Scr_Draging)
            {
                // 获取当前屏幕的光标坐标
                //Point pTemp = new Point(Cursor.Position.X, Cursor.Position.Y);
                // 转换成工作区坐标
                //pTemp = this.PointToClient(pTemp);
                Point pTemp = Mouse.GetPosition((ScrollViewer)sender);
                // 定位事件源的 Location
                ScrollViewer ctrl = sender as ScrollViewer;
                double diffOffsetY = pTemp.Y - Drag_Start_Y;
                double diffOffsetX = pTemp.X - Drag_Start_X;
                ctrl.ScrollToVerticalOffset(Drag_Offset_Y - diffOffsetY);
                ctrl.ScrollToHorizontalOffset(Drag_Offset_X - diffOffsetX);

            }
        }

        private void scrollv1_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        public void ZoomInOut(double delta)
        {
            Point pt = Mouse.GetPosition(grid_main);
            _Scaller.CenterX = pt.X;
            _Scaller.CenterY = pt.Y;
            
            double sx = _Scaller.ScaleX + delta;
            double sy = _Scaller.ScaleY + delta;
            if (sx < 0.1)
                sx = 0.1;
            if (sx > 3)
                sx = 3;
            if (sy < 0.1)
                sy = 0.1;
            if (sy > 3)
                sy = 3;
            _Scaller.ScaleX = sx;
            _Scaller.ScaleY = sy;
        }

        public void Zoom(double x,double y)
        {
            Point pt = Mouse.GetPosition(grid_main);
            _Scaller.CenterX = pt.X;
            _Scaller.CenterY = pt.Y;
            _Scaller.ScaleX = x;
            _Scaller.ScaleY = y;
        }

        public void Zoom(double z)
        {
            Zoom(z, z);
        }

        public void Zoom(double x,double y,double dx,double dy)
        {
            _Scaller.CenterX = x;
            _Scaller.CenterY = y;
            _Scaller.ScaleX = dx;
            _Scaller.ScaleY = dy;
        }

        public void ZoomReset()
        {
            _Scaller.ScaleX = 1;
            _Scaller.ScaleY = 1;
        }

        private void scrollv1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                double delta = e.Delta / 3000.0;
                ZoomInOut(delta);
                e.Handled = true;
            }


        }

        public void Dispose()
        {
            grab_cur.Dispose();
        }
    }
}
