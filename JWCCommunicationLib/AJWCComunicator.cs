using JWCControlLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JWCCommunicationLib
{
    public abstract class AJWCComunicator:IDisposable,IPropGWAble
    {
        public bool IsRunning { get; protected set; }
        public abstract event Action<object, object> OnDataReceived;
        public abstract void Initialization();
        public abstract void Start();
        public abstract void Stop(bool abort);
        public abstract void SendDataTo(object client,object data);
        public virtual void SendBroadCast(object data)
        {

        }
        public abstract bool ComAvaliable { get; }
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

        public virtual void Dispose()
        {
            this.Stop(true);
        }
}
}
