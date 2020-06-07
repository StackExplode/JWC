using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JWCControlLib;

namespace JWCCommunicationLib
{
    public interface IJWCCAdaptor
    {
        void AppendSender(IDataSender ctrl);
        void AppendReceiver(IDataReceiver ctrl);
        void SetCommunicator(AJWCComunicator com);

        AJWCComunicator Communicator { get; set; }
    }
}
