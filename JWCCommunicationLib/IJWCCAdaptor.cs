using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JWCControlLib;
using System.Reflection;

namespace JWCCommunicationLib
{
    public abstract class AJWCCAdaptor:IPropGWAble
    {
        public abstract void AppendSender(IDataSender ctrl);
        public abstract void AppendReceiver(IDataReceiver ctrl);
        public abstract void SetCommunicator(AJWCComunicator com);

        public virtual void Initialization()
        {

        }

        public abstract AJWCComunicator Communicator { get; set; }

        public void SetProp(PropertyInfo pi, object val)
        {
            if (Attribute.IsDefined(pi, typeof(RedirectGSAttribute)))
            {
                object[] attrs = pi.GetCustomAttributes(typeof(RedirectGSAttribute), true);
                RedirectGSAttribute attr = (RedirectGSAttribute)attrs[0];
                var meth = this.GetType().GetMethod(attr.Fun, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                meth.Invoke(this, new object[] { true, val });
            }
            else
                pi.SetValue(this, val, null);
        }

        public object GetProp(PropertyInfo pi)
        {
            if (Attribute.IsDefined(pi, typeof(RedirectGSAttribute)))
            {
                object[] attrs = pi.GetCustomAttributes(typeof(RedirectGSAttribute), true);
                RedirectGSAttribute attr = (RedirectGSAttribute)attrs[0];
                var meth = this.GetType().GetMethod(attr.Fun, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                object val = meth.Invoke(this, new object[] { false, null });
                return val;
            }
            else
                return pi.GetValue(this, null);
        }

        public JControlOutputData OutputProperty()
        {
            JControlOutputData rst = new JControlOutputData();
            PropertyInfo[] pis = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (PropertyInfo pi in pis)
            {
                if (Attribute.IsDefined(pi, typeof(OutputableAttribute)))
                {
                    rst.Add(pi.Name, GetProp(pi));
                }
            }
            return rst;
        }

        public void InputProperty(JControlOutputData dic)
        {
            PropertyInfo[] pis = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (PropertyInfo pi in pis)
            {
                if (Attribute.IsDefined(pi, typeof(OutputableAttribute)))
                {
                    object val = null;
                    if (dic.ContainsKey(pi.Name))
                    {
                        val = dic[pi.Name];
                        SetProp(pi, val);
                    }
                }
            }
        }

    }
}
