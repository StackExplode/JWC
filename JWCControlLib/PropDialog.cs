using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace JWCControlLib
{
    public abstract class PropDialog
    {
        protected CommonDialog ComDia = null;
        protected ICusDialog CusDia = null;
        public bool IsCustomDialog { get; protected set; }


        public virtual DialogResult ShowDialog(out object rt_value,object [] para)
        {
            DialogResult dr;
            if(IsCustomDialog)
            {
                dr = CusDia.ShowDialog();
                rt_value = CusDia.DialogValue;

            }
            else
            {
                dr = ComDia.ShowDialog();
                rt_value = null;
            }
            
            return dr;
        }
    }
}
