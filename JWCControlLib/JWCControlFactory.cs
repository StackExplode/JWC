using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JWCControlLib;
using System.IO;
using System.Collections.Concurrent;
using System.Reflection;

namespace JWCControlLib
{
    public static class JWCControlFactory
    {
        private static volatile ConcurrentDictionary<string, Type> AllControls = new ConcurrentDictionary<string, Type>();

        public static void LoadLibs(string path)
        {
            string [] allfs = Directory.GetFiles(path,"*.dll");
            foreach (string fname in allfs)
            {
                Assembly ass = Assembly.LoadFile(fname);
                Type[] tps = ass.GetTypes();
                foreach(Type tp in tps)
                {
                    if(tp.IsSubclassOf(typeof(JWCControl)))
                    {
                        string name = tp.FullName;
                        if(!AllControls.ContainsKey(name))
                            AllControls.TryAdd(name, tp);
                    }
                }
            }
        }

        public static JWCControl CreateInstance(string fullname)
        {
            JWCControl rt = null;
            Type tp;
            if(AllControls.TryGetValue(fullname, out tp))
            {
                rt = (JWCControl)System.Activator.CreateInstance(tp);
            }
            return rt;
        }
    }
}
