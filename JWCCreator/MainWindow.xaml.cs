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
        Stage stage;
        PropManager pm;

        class ZMR
        {
            public ZMR(string s, int v)
            {
                Name = s;
                Content = v;
            }
            public string Name { get; set; }
            public int Content { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            stage = new Stage(grid_main, scrollv1, scale1, cmb_allc);
            stage.OnSelectionChanged += stage_SelectionChanged;
            stage.OnSelectedCtrlMoved += (s, x, y) => { pm.RefreshMargin(x, y); };
            stage.OnSelectedCtrlResized += (s, x, y) => { pm.RefreshSize(x, y); };
            pm = new PropManager(cmb_allc,pan_prop,txt_help);

            

            cmb_allc.IsEditable = false;
            cmb_allc.IsReadOnly = false;
            //ObservableCollection<JWCControl> temp = new ObservableCollection<JWCControl>();
            //cmb_allc.ItemsSource = temp;
           
            //var xx = new JWCControl();
            //xx.Name = "aaa";
            //xx.Content = new ZMR("111",101);
            //temp.Add(xx);
            //xx = new JWCControl();
            //xx.Name = "bbb";
            //xx.Content = new ZMR("222", 102);
            //temp.Add(xx);
            //xx = new JWCControl();
            //xx.Name = "ccc";
            //xx.Content = new ZMR("333", 103);
            //temp.Add(xx);
            

        }

         void stage_SelectionChanged(bool arg1, object arg2)
        {
            if(arg1)
            {
                JWCControl jc = arg2 as JWCControl;
                if (object.ReferenceEquals(jc, null))
                    return;
                pm.RefreshCurrName(jc);
                pm.RefreshPropList(jc);
            }
            else
            {
                pm.RefreshStageProp(stage);
            }
        }

        private void JWCCMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            JWCControlFactory.LoadLibs(AppDomain.CurrentDomain.BaseDirectory + "\\Controls");
            
        }


        byte[] tempdata;
        XXControl tempxx;
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            //setline1 = ((SolidColorBrush)((Rectangle)(sender as RadioButton).Content).Fill).Color;//要获得所选方块的颜色，需要将SolidColorBrush取Color
            if((sender as RadioButton).Tag.ToString() == "1")
            {
                XXControl xx = new XXControl();
                xx.Margin = new Thickness(0, 0, 0, 0);
                xx.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                xx.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                xx.ShowPic = true;
                xx.Width = 350;
                xx.Height = 350;
                xx.IsEditMode = true;
                xx.ZIndex = 6;
                xx.Name = "WC1";
                stage.AddControl(xx);
            }
            else if((sender as RadioButton).Tag.ToString() == "2")
            {
                XXControl xx = new XXControl();
                xx.Margin = new Thickness(0, 0, 0, 0);
                xx.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                xx.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                xx.ShowPic = true;
                xx.Width = 150;
                xx.Height = 150;
                xx.IsEditMode = true;
                xx.ZIndex = 15;

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
                xx.Margin = new Thickness(0, 0, 0, 0);
                xx.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                xx.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                xx.Width = 150;
                xx.Height = 150;
                xx.IsEditMode = true;
                stage.AddControl(xx);
            }
            else if ((sender as RadioButton).Tag.ToString() == "5")
            {

                JWCControl xx = JWCControlFactory.CreateInstance("NanjingControls.DunWei");
                xx.Parent = grid_main;
                xx.Margin = new Thickness(0, 0, 0, 0);
                xx.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                xx.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                //xx.ShowPic = true;
                //xx.Width = 350;
                //xx.Height = 350;
                xx.IsEditMode = true;
                xx.ZIndex = 6;
                stage.AddControl(xx);
            }
            else if ((sender as RadioButton).Tag.ToString() == "6")
            {

            }
        }

       

        private void scrollv1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
                MessageBox.Show("P Press!");
        }

        private void cmb_allc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JWCControl ctrl = cmb_allc.SelectedItem as JWCControl;
            if (ctrl == null)
                return;
            if (ctrl == stage.SelectedControl)
                return;
            ctrl.GetFocus();
        }
    }
}
