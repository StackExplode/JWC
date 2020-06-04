using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace JWCControlLib
{
    public class OpenFilePropDialog:PropDialog
    {
        public OpenFilePropDialog()
        {
            IsCustomDialog = false;
            ComDia = new OpenFileDialog();
        }

        public override System.Windows.Forms.DialogResult ShowDialog(out object rt_value, object[] para)
        {
            rt_value = null;
            OpenFileDialog op = (OpenFileDialog)ComDia;
            op.Filter = para[0].ToString();
            op.Multiselect = para[1] as bool? ?? false;
            DialogResult dr = op.ShowDialog();
            if(dr == DialogResult.OK)
            {
                if (op.Multiselect)
                    rt_value = op.FileNames;
                else
                    rt_value = op.FileName;
            }
            return dr;
        }
    }
}
