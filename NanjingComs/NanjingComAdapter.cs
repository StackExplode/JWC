using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JWCCommunicationLib;
using JWCControlLib;
using System.Collections.Concurrent;

namespace NanjingComs
{
    public class NanjingComAdapter:IJWCCAdaptor
    {

        ConcurrentDictionary<string,IDataSender> _Senders = new ConcurrentDictionary<string,IDataSender>();
        ConcurrentDictionary<string, IDataReceiver> _Receivers = new ConcurrentDictionary<string, IDataReceiver>();

        public void AppendSender(IDataSender ctrl)
        {
            _Senders[ctrl.SendID] = ctrl;
            ctrl.OnControlStateChanged += ctrl_OnControlStateChanged;
        }

        void ctrl_OnControlStateChanged(object sender,CommunicationType type, object val)
        {
            throw new NotImplementedException("南京项目不需要外发数据");
        }

        public void AppendReceiver(IDataReceiver ctrl)
        {
            _Receivers[ctrl.RecID] = ctrl;
        }

        public AJWCComunicator Communicator
        {
            get;
            set;
        }


        public void SetCommunicator(AJWCComunicator com)
        {
            Communicator = com;
            com.OnDataReceived += com_OnDataReceived;
        }

        private string ConvertID(byte id)
        {
            return id.ToString();
        }

        void com_OnDataReceived(object arg1, object arg2)
        {
            byte[] buff = (byte[])arg2;
            string id = ConvertID(buff[0]);
            var jc = _Receivers[id];
            if (jc == null)
                return;
            if (jc.ComType == CommunicationType.Boolean)
            {
                ((JWCControl)jc).CrossThreadTask(
                        () =>
                        {
                            jc.SetControlState(CommunicationType.Boolean, buff[1] == 0 ? false : true);
                        }
                    );
            }
                
        }
    }
}
