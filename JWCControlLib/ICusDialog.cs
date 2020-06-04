using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JWCControlLib
{
    public interface ICusDialog
    {
        DialogResult ShowDialog();
        object DialogValue { get;  }
    }
}
