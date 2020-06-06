using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWCCommunicationLib
{
    public interface IDataReceiver
    {

        string RecID { get; set; }
        void SetControlState(object obj);
    }

    public interface IDataSender
    {
        string SendID { get; set; }
        event Action<object,object> OnControlStateChanged;
    }

  
}
