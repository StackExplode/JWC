using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWCCommunicationLib
{
    public interface IDataCommunicator
    {
        CommunicationType ComType { get; }
        Guid CustomTypeUID { get; }
    }
    public interface IDataReceiver : IDataCommunicator
    {

        string RecID { get; set; }
        void SetControlState(CommunicationType type,object obj);
    }

    public interface IDataSender : IDataCommunicator
    {
        string SendID { get; set; }
        event Action<object,CommunicationType,object> OnControlStateChanged;
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
