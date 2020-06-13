using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace JWCCommunicationLib
{
    public static class JWCCommunicatorFactory
    {
        private static  ConcurrentDictionary<string, Type> AllComs = new ConcurrentDictionary<string, Type>();
        private static  ConcurrentDictionary<string, Type> AllAdapters = new ConcurrentDictionary<string, Type>();

        public static void LoadLibs(string path)
        {
            string[] allfs = Directory.GetFiles(path, "*.dll");
            foreach (string fname in allfs)
            {
                Assembly ass = Assembly.LoadFile(fname);
                Type[] tps = ass.GetTypes();
                foreach (Type tp in tps)
                {
                    if (tp.IsSubclassOf(typeof(AJWCComunicator)))
                    {
                        string name = tp.FullName;
                        if (!AllComs.ContainsKey(name))
                            AllComs.TryAdd(name, tp);
                    }
                    else if (tp.IsSubclassOf(typeof(AJWCCAdaptor)))
                    {
                        string name = tp.FullName;
                        if (!AllAdapters.ContainsKey(name))
                            AllAdapters.TryAdd(name, tp);
                    }
                }
            }
        }

        public static ConcurrentDictionary<string, Type> GetAllComs()
        {
            return AllComs;
        }

        public static ConcurrentDictionary<string, Type> GetAllAdapters()
        {
            return AllAdapters;
        }

        public static AJWCComunicator CreateCommunicator(string fullname)
        {
            AJWCComunicator rt = null;
            Type tp;
            if (AllComs.TryGetValue(fullname, out tp))
            {
                rt = (AJWCComunicator)System.Activator.CreateInstance(tp);
            }
            return rt;
        }

        public static AJWCCAdaptor CreateAdapter(string fullname)
        {
            AJWCCAdaptor rt = null;
            Type tp;
            if (AllAdapters.TryGetValue(fullname, out tp))
            {
                rt = (AJWCCAdaptor)System.Activator.CreateInstance(tp);
            }
            return rt;
        }
    }
}