using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace JWCControlLib
{
    class ThumbAdder
    {
        private Grid _MainGrid;
        private UserControl _Ctrl;
        private Thumb _TmbMove = null;
        private Thumb _TmbSize_ES = null;
        private Thumb _TmbSize_EN = null;
        private Thumb _TmbSize_WS = null;
        private Thumb _TmbSize_WN = null;
        private Thumb _TmbSize_E = null;
        private Thumb _TmbSize_S = null;
        private Thumb _TmbSize_W = null;
        private Thumb _TmbSize_N = null;

        public Size MinSize { get; set; }
        public Size ResizerSize { get; set; }
        public double ResizerThickness { get; set; }

        public ThumbAdder(UserControl ctrl, Grid grd)
        {
            _MainGrid = grd;
            _Ctrl = ctrl;
            MinSize = new Size(20, 20);
            ResizerSize = new Size(8, 8);
            ResizerThickness = 3;
        }

        const string red_tmplate = "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType=\"Thumb\" >" +
                             "<Rectangle Fill=\"#FFFF0000\" />" +
                             "</ControlTemplate>";

        private void InitMove()
        {
            if (_TmbMove == null)
                _TmbMove = new Thumb();
            else
                return;
            //tmb.Height = tmb.Width = 10;
            _TmbMove.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            _TmbMove.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            //tmb.Margin = new Thickness(0, 0, -10, -10);
            _TmbMove.Cursor = Cursors.SizeAll;

            //_TmbMove.BorderBrush = new SolidColorBrush(Colors.Red);
            //_TmbMove.BorderThickness = new Thickness(2);
            
            string tmplate = "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType=\"Thumb\" >" +
                             "<Rectangle Fill=\"#5A0000FF\" />" +
                             "</ControlTemplate>";


            _TmbMove.Template = (ControlTemplate)XamlReader.Parse(tmplate);
            _TmbMove.DragDelta += (s, e) =>
            {
                //if (this.Width + e.HorizontalChange > 10)
                //    this.Width += e.HorizontalChange;
                //if (this.Height + e.VerticalChange > 10)
                //    this.Height += e.VerticalChange;

                double x = _Ctrl.Margin.Left + e.HorizontalChange;
                double y = _Ctrl.Margin.Top + e.VerticalChange;
                _Ctrl.Margin = new Thickness(x, y, 0, 0);
            };

            
        }

        private void InitSize_ES()
        {
            if (_TmbSize_ES != null)
                return;

            _TmbSize_ES = new Thumb();
            _TmbSize_ES.Height = ResizerSize.Height;
            _TmbSize_ES.Width = ResizerSize.Width;
            _TmbSize_ES.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            _TmbSize_ES.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            _TmbSize_ES.Margin = new Thickness(0, 0, -ResizerSize.Width, -ResizerSize.Height);
            _TmbSize_ES.Cursor = Cursors.SizeNWSE;
            _TmbSize_ES.DragDelta += (s, e) =>
            {
                if (_Ctrl.Width + e.HorizontalChange > MinSize.Width)
                    _Ctrl.Width += e.HorizontalChange;
                if (_Ctrl.Height + e.VerticalChange > MinSize.Height)
                    _Ctrl.Height += e.VerticalChange;
            };
        }

        private void InitSize_S()
        {
            if (_TmbSize_S != null)
                return;

            _TmbSize_S = new Thumb();
            _TmbSize_S.Height = ResizerThickness;
            _TmbSize_S.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            _TmbSize_S.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            _TmbSize_S.Margin = new Thickness(0, 0, 0, -ResizerThickness);
            _TmbSize_S.Cursor = Cursors.SizeNS;
            _TmbSize_S.Template = (ControlTemplate)XamlReader.Parse(red_tmplate);
            _TmbSize_S.DragDelta += (s, e) =>
            {
                if (_Ctrl.Height + e.VerticalChange > MinSize.Height)
                    _Ctrl.Height += e.VerticalChange;
            };
        }

        private void InitSize_E()
        {
            if (_TmbSize_E != null)
                return;

            _TmbSize_E = new Thumb();
            _TmbSize_E.Width = ResizerThickness;
            _TmbSize_E.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            _TmbSize_E.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            _TmbSize_E.Margin = new Thickness(0, 0, -ResizerThickness, 0);
            _TmbSize_E.Cursor = Cursors.SizeWE;
            _TmbSize_E.Template = (ControlTemplate)XamlReader.Parse(red_tmplate);
            _TmbSize_E.DragDelta += (s, e) =>
            {
                if (_Ctrl.Width + e.HorizontalChange > MinSize.Width)
                    _Ctrl.Width += e.HorizontalChange;
            };
        }

        private void InitSize_N()
        {
            if (_TmbSize_N != null)
                return;

            _TmbSize_N = new Thumb();
            _TmbSize_N.Height = ResizerThickness;
            _TmbSize_N.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            _TmbSize_N.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            _TmbSize_N.Margin = new Thickness(0, -ResizerThickness, 0, 0);
            _TmbSize_N.Cursor = Cursors.SizeNS;
            _TmbSize_N.Template = (ControlTemplate)XamlReader.Parse(red_tmplate);
            _TmbSize_N.DragDelta += (s, e) =>
            {
                if (_Ctrl.Height - e.VerticalChange > MinSize.Height)
                {
                    _Ctrl.Height -= e.VerticalChange;
                    Thickness curr = _Ctrl.Margin;
                    _Ctrl.Margin = new Thickness(curr.Left , curr.Top + e.VerticalChange, curr.Right, curr.Bottom);
                }
                   
            };
        }

        private void InitSize_W()
        {
            if (_TmbSize_W != null)
                return;

            _TmbSize_W = new Thumb();
            _TmbSize_W.Width = ResizerThickness;
            _TmbSize_W.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            _TmbSize_W.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            _TmbSize_W.Margin = new Thickness(-ResizerThickness, 0, 0, 0);
            _TmbSize_W.Cursor = Cursors.SizeWE;
            _TmbSize_W.Template = (ControlTemplate)XamlReader.Parse(red_tmplate);
            _TmbSize_W.DragDelta += (s, e) =>
            {
                if (_Ctrl.Width - e.HorizontalChange > MinSize.Width)
                {
                    _Ctrl.Width -= e.HorizontalChange;
                    Thickness curr = _Ctrl.Margin;
                    _Ctrl.Margin = new Thickness(curr.Left + e.HorizontalChange, curr.Top, curr.Right, curr.Bottom);
                    
                }
                    
            };
        }

        private void InitSize_EN()
        {
            if (_TmbSize_EN != null)
                return;

            _TmbSize_EN = new Thumb();
            _TmbSize_EN.Height = ResizerSize.Height;
            _TmbSize_EN.Width = ResizerSize.Width;
            _TmbSize_EN.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            _TmbSize_EN.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            _TmbSize_EN.Margin = new Thickness(0,-ResizerSize.Width, -ResizerSize.Height,0);
            _TmbSize_EN.Cursor = Cursors.SizeNESW;
            _TmbSize_EN.DragDelta += (s, e) =>
            {
                if (_Ctrl.Width + e.HorizontalChange > MinSize.Width)
                    _Ctrl.Width += e.HorizontalChange;
                if (_Ctrl.Height - e.VerticalChange > MinSize.Height)
                {
                    _Ctrl.Height -= e.VerticalChange;
                    Thickness curr = _Ctrl.Margin;
                    _Ctrl.Margin = new Thickness(curr.Left, curr.Top + e.VerticalChange, curr.Right, curr.Bottom);
                }
            };
        }

        private void InitSize_WS()
        {
            if (_TmbSize_WS != null)
                return;

            _TmbSize_WS = new Thumb();
            _TmbSize_WS.Height = ResizerSize.Height;
            _TmbSize_WS.Width = ResizerSize.Width;
            _TmbSize_WS.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            _TmbSize_WS.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            _TmbSize_WS.Margin = new Thickness(-ResizerSize.Width,0, 0,-ResizerSize.Height);
            _TmbSize_WS.Cursor = Cursors.SizeNESW;
            _TmbSize_WS.DragDelta += (s, e) =>
            {
                if (_Ctrl.Width - e.HorizontalChange > MinSize.Width)
                {
                    _Ctrl.Width -= e.HorizontalChange;
                    Thickness curr = _Ctrl.Margin;
                    _Ctrl.Margin = new Thickness(curr.Left + e.HorizontalChange, curr.Top, curr.Right, curr.Bottom);
                }
                if (_Ctrl.Height + e.VerticalChange > MinSize.Height)
                    _Ctrl.Height += e.VerticalChange;
            };
        }

        private void InitSize_WN()
        {
            if (_TmbSize_WN != null)
                return;

            _TmbSize_WN = new Thumb();
            _TmbSize_WN.Height = ResizerSize.Height;
            _TmbSize_WN.Width = ResizerSize.Width;
            _TmbSize_WN.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            _TmbSize_WN.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            _TmbSize_WN.Margin = new Thickness(-ResizerSize.Width, -ResizerSize.Height,0,0);
            _TmbSize_WN.Cursor = Cursors.SizeNWSE;
            _TmbSize_WN.DragDelta += (s, e) =>
            {
                if (_Ctrl.Width - e.HorizontalChange > MinSize.Width)
                {
                    _Ctrl.Width -= e.HorizontalChange;
                    Thickness curr = _Ctrl.Margin;
                    _Ctrl.Margin = new Thickness(curr.Left + e.HorizontalChange, curr.Top, curr.Right, curr.Bottom);
                }
                if (_Ctrl.Height - e.VerticalChange > MinSize.Height)
                {
                    _Ctrl.Height -= e.VerticalChange;
                    Thickness curr = _Ctrl.Margin;
                    _Ctrl.Margin = new Thickness(curr.Left, curr.Top + e.VerticalChange, curr.Right, curr.Bottom);
                }
            };
        }

        public void InitThumbs(bool move=true,bool resize=true)
        {
            if(move)
            {
                InitMove();
            }
            if (resize)
            {
                InitSize_S();
                InitSize_N();
                InitSize_E();
                InitSize_W();
                InitSize_ES();
                InitSize_EN();
                InitSize_WS();
                InitSize_WN();
            }
        }

        public void AppendMoveThumb()
        {
            _MainGrid.Children.Add(_TmbMove);
            _MainGrid.Children.Add(_TmbSize_ES);
            _MainGrid.Children.Add(_TmbSize_EN);
            _MainGrid.Children.Add(_TmbSize_WS);
            _MainGrid.Children.Add(_TmbSize_WN);
            _MainGrid.Children.Add(_TmbSize_S);
            _MainGrid.Children.Add(_TmbSize_N);
            _MainGrid.Children.Add(_TmbSize_E);
            _MainGrid.Children.Add(_TmbSize_W);
        }

        public void RemoveMoveThumb()
        {
            _MainGrid.Children.Remove(_TmbMove);
            _MainGrid.Children.Remove(_TmbSize_ES);
            _MainGrid.Children.Remove(_TmbSize_EN);
            _MainGrid.Children.Remove(_TmbSize_WS);
            _MainGrid.Children.Remove(_TmbSize_WN);
            _MainGrid.Children.Remove(_TmbSize_S);
            _MainGrid.Children.Remove(_TmbSize_N);
            _MainGrid.Children.Remove(_TmbSize_E);
            _MainGrid.Children.Remove(_TmbSize_W);
        }
    }
}
