using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.IO;

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
            bool rela = para[2] as bool? ?? false;

            string basefpath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
            if (!basefpath.EndsWith("\\"))
                basefpath += "\\";
            if(rela)
            {
                op.InitialDirectory = basefpath;
            }
          

            DialogResult dr = op.ShowDialog();
            if(dr == DialogResult.OK)
            {
                if(rela)
                {
                    if(!op.Multiselect)
                    {
                        if (op.FileName.StartsWith(basefpath))
                        {
                            rt_value = op.FileName.Substring(basefpath.Length);
                            rt_value = ".\\" + rt_value;
                        }
                        else
                            rt_value = op.FileName;
                    }
                    else
                    {
                        string[] rts = new string[op.FileNames.Length];
                        for (int i = 0; i < rts.Length; i++)
                        {
                            if (op.FileNames[i].StartsWith(basefpath))
                            {
                                rts[i] = op.FileNames[i].Substring(basefpath.Length);
                                rts[i] = "./" + rts[i];
                            }
                            else
                                rts[i] = op.FileNames[i];
                        }
                        rt_value = rts;
                    }
                }
                else
                {
                    if (op.Multiselect)
                        rt_value = op.FileNames;
                    else
                        rt_value = op.FileName;
                } 
            }
            return dr;
        }
    }
}
