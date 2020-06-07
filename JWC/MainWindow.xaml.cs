﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace JWC
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            com.OnDataReceived += com_OnDataReceived;
            ca.SetCommunicator(com);
        }

        TcpClient cl = null;
        void com_OnDataReceived(object arg1, object arg2)
        {
            cl = (TcpClient)arg1;
        }
        NanjingComs.NJTCPServerCommunicator com = new NanjingComs.NJTCPServerCommunicator();
        NanjingComs.NanjingComAdapter ca = new NanjingComs.NanjingComAdapter();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            com.Initialization(9097);
            com.Start();
        }



        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("F1!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            com.Stop(false);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            com.SendDataTo(cl,new byte[] { 2, 3, 0xFF, 2 });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NanjingControls.DunWei dw = new NanjingControls.DunWei();
            dw.Size = new Size(160, 90);
            (dw as JWCCommunicationLib.IDataReceiver).RecID = "2";
            main_grid.Children.Add(dw);
            ca.AppendReceiver(dw);
        }
    }
}
