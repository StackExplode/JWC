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
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        JWCControl _Selecting_Ctrl = null;
        public MainWindow()
        {
            InitializeComponent();
            grid_main.MouseDown += (a, b) => { if (_Selecting_Ctrl != null)_Selecting_Ctrl.LoseFocus(); _Selecting_Ctrl = null; };
            
        }



        private bool Mousedown,ImgSelected;
        private double CurX = 0;
        private double CurY = 0;
        private void JWCCMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            image1.Source = Properties.Resources.pixiv21596.ToBitMapSource();
            JWCControlFactory.LoadLibs(AppDomain.CurrentDomain.BaseDirectory + "\\Controls");
            
        }

        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(ImgSelected)
            {
                Point pt = Mouse.GetPosition(sender as FrameworkElement);
                CurX = pt.X;
                CurY = pt.Y;
                Mousedown = true;
                ((FrameworkElement)sender).Cursor = Cursors.SizeAll;
            }
            else
            {
                Border bd = new Border();
                
                bd.BorderBrush = new SolidColorBrush(Colors.Red);
                bd.BorderThickness = new Thickness(4);
                bd.Margin = image1.Margin;
                bd.Width = image1.Width;
                bd.Height = image1.Height;
                grid_main.Children.Remove(image1);
                bd.Child = image1;
                image1.Margin = new Thickness(0, 0, 0, 0);

                grid_main.Children.Add(bd);
                ImgSelected = true;
            }
           
            
        }

        private void image1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mousedown)
            {
                // 获取当前屏幕的光标坐标
                //Point pTemp = new Point(Cursor.Position.X, Cursor.Position.Y);
                // 转换成工作区坐标
                //pTemp = this.PointToClient(pTemp);
                Point pTemp = Mouse.GetPosition(grid_main);
                // 定位事件源的 Location
                FrameworkElement control = sender as FrameworkElement;
                control.Margin = new Thickness(pTemp.X - CurX, pTemp.Y - CurY, 0, 0);

                
            }
        }

        private void image1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mousedown = false;
            if (sender is FrameworkElement)
            {
                ((FrameworkElement)sender).Cursor = Cursors.Arrow;
            }
        }

        private void JWCCMainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            Point pTemp = Mouse.GetPosition(this);
            //lbl1.Content = pTemp.ToString();
        }

        private void grid2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //ImgSelected = false;
        }

        byte[] tempdata;
        XXControl tempxx;
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            //setline1 = ((SolidColorBrush)((Rectangle)(sender as RadioButton).Content).Fill).Color;//要获得所选方块的颜色，需要将SolidColorBrush取Color
            if((sender as RadioButton).Tag.ToString() == "1")
            {
                XXControl xx = new XXControl();
                xx.Parent = grid_main;
                xx.Margin = new Thickness(0, 0, 0, 0);
                xx.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                xx.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                xx.ShowPic = true;
                xx.Width = 350;
                xx.Height = 350;
                xx.IsEditMode = true;
                xx.ZIndex = 6;
                xx.OnGotFocus += xx_OnGotFocus;
                xx.Show();
            }
            else if((sender as RadioButton).Tag.ToString() == "2")
            {
                XXControl xx = new XXControl();
                xx.Parent = grid_main;
                xx.Margin = new Thickness(0, 0, 0, 0);
                xx.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                xx.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                xx.ShowPic = true;
                xx.Width = 150;
                xx.Height = 150;
                xx.IsEditMode = true;
                xx.ZIndex = 15;
                xx.OnGotFocus += xx_OnGotFocus;
                xx.Show();
                tempxx = xx;
            }
            else if((sender as RadioButton).Tag.ToString() == "3")
            {
                XXControl xx = new XXControl();
                xx.ShowPic = false;
                xx.ZIndex = 33;
                xx.ForeColor = Colors.Pink;
                JControlOutputData dic = xx.OutputProperty();
                MessageBox.Show(dic["ForeColor"].ToString());

                JWCSerializer<JControlOutputData> ss = new JWCSerializer<JControlOutputData>();
                tempdata = ss.Serialize(dic);
               
            }
            else if ((sender as RadioButton).Tag.ToString() == "4")
            {
                JWCSerializer<JControlOutputData> ss = new JWCSerializer<JControlOutputData>();
                JControlOutputData oo = ss.Deserialize(tempdata);
                XXControl xx = new XXControl();
                xx.InputProperty(oo);
                MessageBox.Show(xx.ForeColor.ToString());
                xx.Parent = grid_main;
                xx.Margin = new Thickness(0, 0, 0, 0);
                xx.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                xx.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                xx.Width = 150;
                xx.Height = 150;
                xx.IsEditMode = true;
                xx.OnGotFocus += xx_OnGotFocus;
                xx.Show();
            }
            else if ((sender as RadioButton).Tag.ToString() == "5")
            {

                JWCControl xx = JWCControlFactory.CreateInstance("NanjingControls.DunWei");
                xx.Parent = grid_main;
                xx.Margin = new Thickness(0, 0, 0, 0);
                xx.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                xx.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                //xx.ShowPic = true;
                xx.Width = 350;
                xx.Height = 350;
                xx.IsEditMode = true;
                xx.ZIndex = 6;
                xx.OnGotFocus += xx_OnGotFocus;
                xx.Show();
            }
            else if ((sender as RadioButton).Tag.ToString() == "6")
            {
                

            }
        }

      

        void xx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                MessageBox.Show("Double Clicked!");
            }
        }

        void xx_OnGotFocus(object obj)
        {
            if (_Selecting_Ctrl != null)
                _Selecting_Ctrl.LoseFocus();
            _Selecting_Ctrl = obj as JWCControl;
        }



      

        bool Scr_Draging = false;
        private double Drag_Start_X = 0;
        private double Drag_Start_Y = 0;
        private double Drag_Offset_X = 0;
        private double Drag_Offset_Y = 0;
        private void scrollv1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!Scr_Draging)
            {
                Point pt = Mouse.GetPosition(sender as FrameworkElement);
                Drag_Start_X = pt.X;
                Drag_Start_Y = pt.Y;
                Drag_Offset_X = (sender as ScrollViewer).HorizontalOffset;
                Drag_Offset_Y = (sender as ScrollViewer).VerticalOffset;
                Scr_Draging = true;
                (sender as ScrollViewer).CaptureMouse();
                ((FrameworkElement)sender).Cursor = Cursors.SizeAll;
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

        private void scrollv1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(Keyboard.Modifiers == ModifierKeys.Control)
            {
                Point pt = Mouse.GetPosition(grid_main);
                scale1.CenterX = pt.X;
                scale1.CenterY = pt.Y;
                double delta = e.Delta / 3000.0;
                double sx = scale1.ScaleX + delta;
                double sy = scale1.ScaleY + delta;
                if (sx < 0.1)
                    sx = 0.1;
                if (sx > 3)
                    sx = 3;
                if (sy < 0.1)
                    sy = 0.1;
                if (sy > 3)
                    sy = 3;
                scale1.ScaleX = sx;
                scale1.ScaleY = sy;
                e.Handled = true;
            }
            
            
        }


        private void scrollv1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
                MessageBox.Show("P Press!");
        }
    }
}
