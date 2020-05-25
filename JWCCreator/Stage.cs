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
    class Stage
    {
        private Grid grid_main;
        private JWCControl _Selecting_Ctrl = null;



        public Stage(Grid grd)
        {
            grid_main = grd;
            InitGrid();
        }

        private void InitGrid()
        {
            grid_main.MouseDown += (a, b) => { if (_Selecting_Ctrl != null)_Selecting_Ctrl.LoseFocus(); _Selecting_Ctrl = null; };
        }


    }
}
