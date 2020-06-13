using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using JWCCommunicationLib;
using JWCControlLib;

namespace NanjingComs
{
    [JWCCommDesc("TCP Server通信器","仅供测试的TCP服务器类型通信器，监听本机设置的端口，从机主动连接并交换数据")]
    public class NJTCPServerCommunicator:AJWCComunicator
    {

        public override event Action<object, object> OnDataReceived;

        protected int _Port;
        protected TcpListener _Listener;
        private bool _Running = false;

        [PropDiscribe(CreatorPropType.Text,"监听端口","作为服务器监听的本地端口，其是一个1~65535的整数")]
        [Outputable]
        public int Port {
            get
            {
                return _Port;
            }
            set
            {
                if (value < 1 || value > 65535)
                    throw new Exception("设置端口号必须介于1~65535");
                _Port = value;
            }
        }

        [PropDiscribe(CreatorPropType.Text, "最大连接数", "允许同时连接的最大客户端数量")]
        [Outputable]
        public int MaxClient { get; set; }

        [PropDiscribe(CreatorPropType.Boolean, "接收后关闭", "是否在接收到客户端数据后立即主动关闭客户端")]
        [Outputable]
        public bool CloseAfterRec { get; set; }

        [PropDiscribe(CreatorPropType.Boolean, "发送后关闭", "是否在发送数据后立即主动关闭与客户端的连接")]
        [Outputable]
        public bool CloseAfterSend { get; set; }

        public NJTCPServerCommunicator()
        {
            CloseAfterSend = CloseAfterRec = false;
            MaxClient = 100;
            Port = 6666;
        }

        public override void Initialization()
        {
            //_Port = (int)pars[0];
            _Listener = new TcpListener(IPAddress.Any,_Port);
            
        }

        internal class RecState
        {
            public NetworkStream Stream;
            public TcpClient Client;
            public byte[] Buffer;
        }

        //internal class SendState
        //{
        //    public TcpListener Client;
        //    public NetworkStream Stream;
        //    public byte[] Buffer;
        //}

        private void HandleAsyncConnection(IAsyncResult res)
        {
            if (_Listener == null || _Listener.Server ==null || _Listener.Server.IsBound == false)
                return;
            _Listener.BeginAcceptTcpClient(HandleAsyncConnection, _Listener);
            TcpClient client = _Listener.EndAcceptTcpClient(res);
            client.ReceiveTimeout = 10000;
            byte[] buff = new byte[client.Client.ReceiveBufferSize];
            NetworkStream ns = client.GetStream();
            ns.ReadTimeout = 10000;
            RecState state = new RecState { Client = client, Buffer = buff, Stream = ns };
            ns.BeginRead(buff, 0, buff.Length, HandleClientAsyncRec, state);

        }

        private void HandleClientAsyncRec(IAsyncResult res)
        {
            RecState state = (RecState)res.AsyncState;
            TcpClient client = state.Client;
            byte[] oldbuff = state.Buffer;
            NetworkStream ns = state.Stream;

            if (client == null || !client.Connected)
                return;

            int b2r;
            try
            {
                b2r = ns.EndRead(res);
            }
            catch(System.IO.IOException)
            {
                return;
            }
            
            if(client.Available > 0)
            {
                throw new Exception("Data too long!");
            }
            else
            {
                byte[] rt = new byte[b2r];
                Array.Copy(oldbuff, rt, b2r);
                if(CloseAfterRec)
                {
                    ns.Close();
                    client.Close();
                }
                else
                {
                    ns.BeginRead(state.Buffer, 0, state.Buffer.Length, HandleClientAsyncRec, state);
                }
                if (OnDataReceived != null)
                    OnDataReceived(client, rt);
            }
        }

        public override void Start()
        {
            if (_Running)
                return;
            _Listener.Start(MaxClient);
            _Listener.BeginAcceptTcpClient(HandleAsyncConnection, _Listener);
            _Running = true;
        }

        public override void Stop(bool abort=false)
        {
            if (_Running == false)
                return;
            if(_Listener.Server.IsBound)
                _Listener.Stop();
            _Running = false;
        }

        public override void SendDataTo(object client,object data)
        {
            _Listener.Server.SendTimeout = 10000;
            byte[] buff = (byte[])data;
            TcpClient cl = (TcpClient)client;
            if (!cl.Connected)
                return;
            NetworkStream ns = cl.GetStream();
            ns.BeginWrite(buff, 0, buff.Length, HandleSendDataEnd, cl);
        }

        private void HandleSendDataEnd(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            NetworkStream ns = client.GetStream();
            ns.EndWrite(ar);
            if(CloseAfterSend)
            {
                ns.Close();
                client.Close();
            }
        }



        public override bool ComAvaliable
        {
            get 
            {
                return true;
            }
        }
    }
}
