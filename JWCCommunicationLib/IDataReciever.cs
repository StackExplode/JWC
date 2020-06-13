using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace JWCCommunicationLib
{
    public interface IDataCommunicator
    {
        //CommunicationType ComType { get; }
        //Guid CustomTypeUID { get; set; }
        RegisterType GetRegisterType(string key);
    }
    public interface IDataReceiver : IDataCommunicator
    {

        string RecID { get; set; }
        void SetRegister(string key,object data);
        event Action<object, string> OnRequestRegister;
    }

    public interface IDataSender : IDataCommunicator
    {
        string SendID { get; set; }
        //sender,destination,key,value
        event Action<object,string,string,object> OnRegisterChanged;
        void RequestRegister(string key);
    }

    [Flags]
    public enum RegisterType:int
    {
        Undefined = 0,
        Boolean = 1,
        Byte = (1<<1),
        Integer = (1<<2),
        Double = (1<<3),
        String = (1<<4),
        DateTime = (1<<5),

        Array=(1<<31)
    }

    [Flags]
    public enum CommunicationType:int
    {
        Custom = 0,
        Boolean = (1<<0),
        Integer = (1<<1),
        Number = (1<<2),
        ASCIIString = (1<<3),
        WString = (1<<4),
        VarString = (1<<5),
        Binaray = (1<<6),
        DateTiem=(1<<7),

        BoolArray=(1<<16),
        IntArray=(1<<17),
        NumArray=(1<<18),
        AStringArray=(1<<19),
        WStringArray=(1<<20),
        VStringArray=(1<<21),
        BinArray=(1<<22),
        DateTiemArray=(1<<23)
    }
  
}
