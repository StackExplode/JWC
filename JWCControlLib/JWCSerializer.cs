using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JWCControlLib
{
    public class JWCSerializer<T>
    {
        public byte[] Serialize(T obj)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);

            byte[] data = ms.ToArray();

            ms.Close();

            return data;
        }

        public void Serialize(T obj, string fname)
        {
            FileStream fs = new FileStream(fname, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }

        public T Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);

            stream.Close();

            return (T)obj;
        }

        public T Deserialize(string fname)
        {
            FileStream stream = new FileStream(fname,FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);

            stream.Close();

            return (T)obj;
        }

        public byte[] ContractSerialize(T obj)
        {
            MemoryStream ms = new MemoryStream();
            DataContractSerializer ss = new DataContractSerializer(typeof(T));
            ss.WriteObject(ms, obj);
            byte[] data = ms.ToArray();

            ms.Close();

            return data;
        }

        public T ContractDeSerialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            DataContractSerializer ss = new DataContractSerializer(typeof(T));
            object obj = ss.ReadObject(stream);

            stream.Close();

            return (T)obj;
        }
    }

    [Serializable]
    public class  JWCSaveFile
    {
        public List<JControlOutputData> AllControls;
        public JControlOutputData Communicator;
        public JControlOutputData ComAdapter;
        public string ComName;
        public string AdaName;
        public object[] Parameters;
        public int Height;
        public int Width;
        public string BackGroundPic;
        public bool BgUsePic;
        public byte[] BackColor;
        public Version Version;
      
    }

   
}
