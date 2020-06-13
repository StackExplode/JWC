using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using JWCCommunicationLib;
using JWCControlLib;

namespace JWCCreator
{
    public delegate void CommuWizardFinishHandler(JControlOutputData comdata,JControlOutputData adadata,string f1,string f2);
    /// <summary>
    /// fm_comsetting.xaml 的交互逻辑
    /// </summary>
    public partial class fm_comsetting : Window
    {
        static readonly string[] StepDescString = new string[]
        {
            "根据实际通信使用的通信物理层选取本项目使用的通信器。",
            "对上一步选取的通信器的具体参数进行设置",
            "针对通信协议选取通信适配器，适配器工作于逻辑层并依赖本向导选择的通信器工作，其用于协调收发的数据与控件之间的联动",
            "对上一步选取的通信协调器参数进行设置"
        };

        public event Action<int,int, WizardEventArg> OnStepChanged;
        public event CommuWizardFinishHandler OnFinished;

        private AJWCComunicator Communicator;
        private AJWCCAdaptor Adapter;

        public void SetData(JControlOutputData comdata,JControlOutputData adadata,string f1,string f2)
        {
            if (comdata != null)
            {
                Communicator = JWCCommunicatorFactory.CreateCommunicator(f1);
                Communicator.InputProperty(comdata);
            }    
            if (adadata != null)
            {
                Adapter = JWCCommunicatorFactory.CreateAdapter(f2);
                Adapter.InputProperty(adadata);
            }
                
        }

        public fm_comsetting()
        {
            InitializeComponent();
            this.Loaded += fm_comsetting_Loaded;
            OnStepChanged += fm_comsetting_OnStepChanged;
            lst_com.DisplayMemberPath = "FName";
            lst_ada.DisplayMemberPath = "FName";
            lst_com.SelectionChanged += lst_com_SelectionChanged;
            lst_ada.SelectionChanged += lst_ada_SelectionChanged;
        }

        void lst_ada_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyListItem item = (MyListItem)lst_ada.SelectedItem;
            lbl_adaname.Content = item.FName;
            lbl_adafullname.Content = item.FullName;
            txt_adadesc.Text = item.Description;
        }

        void lst_com_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyListItem item = (MyListItem)lst_com.SelectedItem;
            lbl_comname.Content = item.FName;
            lbl_comfullname.Content = item.FullName;
            txt_comdesc.Text = item.Description;
        }

        void fm_comsetting_OnStepChanged(int laststep,int nextstep, WizardEventArg e)
        {
            if(laststep == 1)
            {
                if (lst_com.SelectedItem == null)
                {
                    MessageBox.Show("你必须选择一个通信器！","错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                MyListItem item = (MyListItem)lst_com.SelectedItem;
                if (Communicator == null || Communicator.GetType().FullName != item.FullName)
                    Communicator = JWCCommunicatorFactory.CreateCommunicator(item.FullName);
  
                ComAdPropManager man = new ComAdPropManager(stk_com_prop, txt_comhelp);
                man.RefreshComPropList(Communicator);
            }
            else if(laststep == 3)
            {
                if (lst_ada.SelectedItem == null)
                {
                    MessageBox.Show("你必须选择一个适配器！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                MyListItem item = (MyListItem)lst_ada.SelectedItem;
                if (Adapter == null || Adapter.GetType().FullName != item.FullName)
                    Adapter = JWCCommunicatorFactory.CreateAdapter(item.FullName);
                ComAdPropManager man = new ComAdPropManager(stk_ada_prop, txt_adahelp);
                man.RefreshComPropList(Adapter);
            }
        }
        
        void SetStepLbls(int s)
        {
            lbl_step.Content = CurrStep.ToString() + "/" + TotalStep.ToString();
            lbl_desc.Text = StepDescString[s-1];
        }

        void fm_comsetting_Loaded(object sender, RoutedEventArgs e)
        {
            SetStepLbls(CurrStep);

            string old_comfullname = null;
            if (Communicator != null)
                old_comfullname = Communicator.GetType().FullName;
            int idx = 0,s_idx=0;
            var lst = JWCCommunicatorFactory.GetAllComs();
            foreach(var pai in lst)
            {
                MyListItem item = new MyListItem();
                item.FullName = pai.Key;
                Type tp = pai.Value;
                JWCCommDescAttribute attr = (JWCCommDescAttribute)Attribute.GetCustomAttribute(tp, typeof(JWCCommDescAttribute));
                item.FName = attr.FriendlyName;
                item.Description = attr.Description;
                lst_com.Items.Add(item);
                if (old_comfullname != null && old_comfullname.Equals(item.FullName))
                    s_idx = idx;
                idx++;
            }
            if(this.Communicator != null)
            {
                lst_com.SelectedIndex = s_idx;
            }
            idx = s_idx = 0;

            //Init adapter listbox
            if (Adapter != null)
                old_comfullname = Adapter.GetType().FullName;
            var lst2 = JWCCommunicatorFactory.GetAllAdapters();
            foreach(var pai in lst2)
            {
                MyListItem item = new MyListItem();
                item.FullName = pai.Key;
                Type tp = pai.Value;
                JWCAdapterDescAttribute attr = (JWCAdapterDescAttribute)Attribute.GetCustomAttribute(tp, typeof(JWCAdapterDescAttribute));
                item.FName = attr.FriendlyName;
                item.Description = attr.Description;
                lst_ada.Items.Add(item);
                if (old_comfullname != null && old_comfullname.Equals(item.FullName))
                    s_idx = idx;
                idx++;
            }
            if (this.Adapter != null)
            {
                lst_ada.SelectedIndex = s_idx;
            }
        }
        const int TotalStep = 4;
        int CurrStep = 1;


        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            WizardEventArg eee = new WizardEventArg();
            if(!btn_next.Content.Equals("完成"))
            {
                if (OnStepChanged != null)
                    OnStepChanged(CurrStep,CurrStep+1,eee);
                if (eee.Cancel)
                    return;
                CurrStep++;
                if (CurrStep <= TotalStep)
                {
                    btn_prev.IsEnabled = true;
                    tab1.SelectedIndex = CurrStep - 1;
                    SetStepLbls(CurrStep);
                    if(CurrStep == TotalStep)
                        btn_next.Content = "完成";
                }
            }
            else
            {
                JControlOutputData data1 = Communicator == null ? null : Communicator.OutputProperty();
                JControlOutputData data2 = Adapter == null ? null : Adapter.OutputProperty();
                string f1 = Communicator == null ? null : Communicator.GetType().FullName;
                string f2 = Adapter == null ? null : Adapter.GetType().FullName;
                if (OnFinished != null)
                    OnFinished(data1,data2,f1,f2);
                this.Close();
            }
           

        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_prev_Click(object sender, RoutedEventArgs e)
        {
            WizardEventArg eee = new WizardEventArg();
            if (OnStepChanged != null)
                OnStepChanged(CurrStep,CurrStep-1,eee);
            if (eee.Cancel)
                return;
            CurrStep--;
            if (CurrStep >= 1)
            {
                btn_next.Content = "下一步>";
                tab1.SelectedIndex = CurrStep - 1;
                SetStepLbls(CurrStep);
                if(CurrStep == 1)
                    btn_prev.IsEnabled = false;
            }
            
        }
    }

    class MyListItem
    {
        public string FName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
    }

    public class WizardEventArg
    {
        public bool Cancel { get; set; }
    }
}
