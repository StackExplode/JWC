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
using JWCCommunicationLib;
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

        JControlOutputData Communicator_Data = null;
        JControlOutputData Adapter_Data = null;
        string ComFname, AdaFname;

        //class ZMR
        //{
        //    public ZMR(string s, int v)
        //    {
        //        Name = s;
        //        Content = v;
        //    }
        //    public string Name { get; set; }
        //    public int Content { get; set; }
        //}

       

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

            Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            this.Title += "(" + ver.ToString(3) + ")";

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
            JWCCommunicationLib.JWCCommunicatorFactory.LoadLibs(AppDomain.CurrentDomain.BaseDirectory + "\\Communicators");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Communicator_Data = null;
            Adapter_Data = null;
            fm_new fm = new fm_new();
            fm.OnConfirmNew += (w, h) => { stage.ClearAll(w, h); };
            fm.Owner = this;
            fm.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBoxResult dr = MessageBox.Show("确认删除控件吗？", "询问", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dr != MessageBoxResult.Yes)
                return;
            stage.RemoveSelecting();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            stage.ZoomInOut(0.25);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            stage.ZoomInOut(-0.25);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            stage.ZoomReset();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button_Click_2(null, null);
          
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            stage.UnSelectControl();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            stage.CopySelecting();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            stage.PasteSelecting();
        }
        fm_addctrl fm_add;
        private void Button_Click_add(object sender, RoutedEventArgs e)
        {
            if (fm_add == null)
            {
                fm_add = new fm_addctrl();
                fm_add.Owner = this;
                fm_add.ShowActivated = true;
                fm_add.Closed += (ss, ee) => { fm_add = null; };
                fm_add.OnConfirmAdd += (jc) => {
                    jc.Margin = new Thickness(scrollv1.HorizontalOffset / scale1.ScaleX, scrollv1.VerticalOffset/scale1.ScaleY, 0, 0);
                    stage.AddControl(jc); 
                };
            }
            else
                fm_add.Focus();
                
            fm_add.Show();
            
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog sv = new System.Windows.Forms.SaveFileDialog();
            sv.Filter = "JWC工程文件|*.jwc";
            System.Windows.Forms.DialogResult dr = sv.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
                return;

            SLHelper sl = new SLHelper(stage, Communicator_Data, Adapter_Data);
            sl.ComFname = ComFname;
            sl.AdaFname = AdaFname;
            sl.SaveFile(sv.FileName);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
            op.Filter = "JWC工程文件|*.jwc";
            System.Windows.Forms.DialogResult dr = op.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
                return;

            SLHelper sl = new SLHelper(stage, null, null);
            sl.LoadFile(op.FileName);
            Communicator_Data = sl.commu;
            Adapter_Data = sl.adapter;
            ComFname = sl.ComFname;
            AdaFname = sl.AdaFname;
        }

        private void Btn_ComWiz_Clicked(object sender, RoutedEventArgs e)
        {
            fm_comsetting fm = new fm_comsetting();
            fm.Owner = this;
            fm.SetData(Communicator_Data, Adapter_Data,ComFname,AdaFname);
            fm.OnFinished += fm_OnFinished;
            fm.ShowDialog();
        }

        void fm_OnFinished(JControlOutputData comdata, JControlOutputData adadata,string f1,string f2)
        {
            Communicator_Data = comdata;
            Adapter_Data = adadata;
            ComFname = f1;
            AdaFname = f2;
        }

        private void Btn_PageSet_Click(object sender, RoutedEventArgs e)
        {
            fm_gridsetting fm = new fm_gridsetting(stage);
            fm.Owner = this;
            fm.ShowDialog();
        }

        private void Zoomout_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button_Click_3(null, null);
        }

        private void Zoomrst_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button_Click_4(null, null);
        }

        private void Dele_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button_Click_1(null, null);
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button_Click_6(null, null);
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button_Click_7(null, null);
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button_Click_add(null, null);
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            fm_about fm = new fm_about();
            fm.Owner = this;
            fm.ShowDialog();
        }


    }
}
