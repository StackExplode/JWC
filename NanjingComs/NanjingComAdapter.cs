using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JWCCommunicationLib;
using JWCControlLib;
using System.Collections.Concurrent;

namespace NanjingComs
{
    [JWCAdapterDesc("南京通信适配器","用于南京厕所项目的通信适配器，配合蹲位等控件使用")]
    public class NanjingComAdapter:AJWCCAdaptor
    {

        ConcurrentDictionary<string,IDataSender> _Senders = new ConcurrentDictionary<string,IDataSender>();
        ConcurrentDictionary<string, IDataReceiver> _Receivers = new ConcurrentDictionary<string, IDataReceiver>();

        public override void AppendSender(IDataSender ctrl)
        {
            _Senders[ctrl.SendID] = ctrl;
            ctrl.OnRegisterChanged += ctrl_OnControlStateChanged;
        }

        void ctrl_OnControlStateChanged(object sender,string dest,string key, object val)
        {
            throw new NotImplementedException("南京项目不需要外发数据");
        }

        public override void AppendReceiver(IDataReceiver ctrl)
        {
            _Receivers[ctrl.RecID] = ctrl;
        }

        public override AJWCComunicator Communicator
        {
            get;
            set;
        }


        public override void SetCommunicator(AJWCComunicator com)
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
            IDataReceiver jc = null;
            string id = ConvertID(buff[0]);
            bool zonzai = _Receivers.TryGetValue(id, out jc);
            if (zonzai == false || jc == null)
                return;
            string regid = buff[1].ToString();
            if (jc.GetRegisterType(regid) == RegisterType.Boolean)
            {
                ((JWCControl)jc).CrossThreadTask(
                    () =>
                    {
                        jc.SetRegister(regid, buff[2] == 0 ? false : true);
                    });
            }
                
        
                
        }
    }
}
