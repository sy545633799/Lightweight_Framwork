// ========================================================
// des：网络管理类
// author: https://github.com/liuhaopen/UnityMMO
// time：2020-06-26 11:19:41
// version：1.0
// ========================================================

using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using XLua;


namespace Game
{
    using NetEventType = KeyValuePair<Action<byte[]>, byte[]>;

    public enum NetPackageType
    {
        BaseLine = 1,       //基于行的解析
        BaseHead = 2,       //头两字节为包大小
    }

    public class DisType
    {
        public const int ConnectFailed = 1;  //连不上
        public const int Exception = 2;     //数据解析错误
        public const int Disconnect = 3;    //客户端或服务端正常断开
    }

    public class TcpManager
    {
        private static readonly object m_lockObject = new object();
        private static Queue<NetEventType> mEvents = new Queue<NetEventType>();
        private static TcpClient client = null;
        private static NetworkStream outStream = null;
        private static MemoryStream memStream;
        private static BinaryReader reader;
        private static NetPackageType curPackageType = NetPackageType.BaseLine;
        private const int MAX_READ = 8192;
        // private int session = 0;
        // private int maxSession = int.MaxValue/2;
        private static byte[] byteBuffer = new byte[MAX_READ];
        private static byte[] connectFailedTag = new byte[1] { DisType.ConnectFailed };
        private static byte[] exceptionTag = new byte[1] { DisType.Exception };
        private static byte[] disconnectTag = new byte[1] { DisType.Disconnect };
        // public static bool loggedIn = false;
        public const int MinLuaNetSessionID = System.Int32.MaxValue / 2;
        public const int MaxLuaNetSessionID = System.Int32.MaxValue;
        public static Action<byte[]> onConnectCallBack = null;
        public static Action<byte[]> onDisConnectCallBack = null;
        public static Action<byte[]> onReceiveLineCallBack = null;
        public static Action<byte[]> onReceiveMsgCallBack = null;
        public static Action<byte[]> OnConnectCallBack
        {
            get
            {
                return onConnectCallBack;
            }
            set
            {
                onConnectCallBack = value;
            }
        }
        public static Action<byte[]> OnDisConnectCallBack
        {
            get
            {
                return onDisConnectCallBack;
            }
            set
            {
                onDisConnectCallBack = value;
            }
        }
        public static Action<byte[]> OnReceiveLineCallBack
        {
            get
            {
                return onReceiveLineCallBack;
            }
            set
            {
                onReceiveLineCallBack = value;
            }
        }
        public static Action<byte[]> OnReceiveMsgCallBack
        {
            get
            {
                return onReceiveMsgCallBack;
            }
            set
            {
                onReceiveMsgCallBack = value;
            }
        }

        public static void Init()
        {
            memStream = new MemoryStream();
            reader = new BinaryReader(memStream);
        }

        public static void AddEvent(Action<byte[]> _event, byte[] data)
        {
            lock (m_lockObject)
            {
                mEvents.Enqueue(new NetEventType(_event, data));
            }
        }

        public static void Update()
        {
            if (mEvents.Count > 0)
            {
                while (mEvents.Count > 0)
                {
                    NetEventType _event = mEvents.Dequeue();
                    _event.Key(_event.Value);
                }
            }
        }

        public static void Connect(string host, int port, NetPackageType type)
        {
            Debug.Log("host : " + host + " port:" + port.ToString() + " type:" + type.ToString());
            Close();
            curPackageType = type;
            try
            {
                IPAddress[] address = Dns.GetHostAddresses(host);
                if (address.Length == 0)
                {
                    Debug.LogError("host invalid");
                    return;
                }
                if (address[0].AddressFamily == AddressFamily.InterNetworkV6)
                {
                    client = new TcpClient(AddressFamily.InterNetworkV6);
                }
                else
                {
                    client = new TcpClient(AddressFamily.InterNetwork);
                }
                client.SendTimeout = 1000;
                client.ReceiveTimeout = 1000;
                client.NoDelay = true;
                //Debug.Log("begin connect socket");
                client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
            }
            catch (Exception e)
            {
                Close();
                OnDisconnected(DisType.ConnectFailed, connectFailedTag, "ConnectFailed");
                Debug.LogError(e.Message);
            }
        }

        public static void OnConnect(IAsyncResult asr)
        {
            //Debug.Log("on connect : " + client.Connected.ToString());
            if (!client.Connected)
            {
                Close();
                OnDisconnected(DisType.ConnectFailed, connectFailedTag, "ConnectFailed");
                return;
            }
            outStream = client.GetStream();
            if (curPackageType == NetPackageType.BaseLine)
            {
                client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnReadLine), null);
            }
            else
            {
                client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
            }
            //Debug.Log("onConnectCallBack : " + (onConnectCallBack != null).ToString());
            AddEvent(onConnectCallBack, null);
        }

		public static void SendBytes(byte[] message)
		{
			MemoryStream ms = null;
			using (ms = new MemoryStream())
			{
				ms.Position = 0;
				BinaryWriter writer = new BinaryWriter(ms);
				if (curPackageType == NetPackageType.BaseHead)
				{
                    UInt16 msglen = BytesUtility.SwapUInt16((UInt16)(message.Length));
					writer.Write(msglen);
				}
				writer.Write(message);
				writer.Flush();
				if (client != null && client.Connected)
				{
					byte[] payload = ms.ToArray();
					outStream.BeginWrite(payload, 0, payload.Length, new AsyncCallback(OnWrite), null);
				}
				else
				{
					Debug.LogError("client.connected----->>false");
				}
			}
		}

        public static void SendBytesWithoutSize(byte[] message)
        {
            if (client != null && client.Connected)
            {
                outStream.BeginWrite(message, 0, message.Length, new AsyncCallback(OnWrite), null);
            }
            else
                Debug.LogError("SendBytesWithoutSize failed:connected----->>false");
        }

        public static void OnRead(IAsyncResult asr)
        {
            int bytesRead = 0;
            try
            {
                lock (client.GetStream())
                {         //读取字节流到缓冲区
                    bytesRead = client.GetStream().EndRead(asr);
                }
                if (bytesRead < 1)
                {   
                    //包尺寸有问题，断线处理
                    Debug.Log("net manager read empty!");
                    OnDisconnected(DisType.Disconnect, disconnectTag, "bytesRead < 1");
                    return;
                }
                OnReceive(byteBuffer, bytesRead);   //分析数据包内容，抛给逻辑层
                lock (client.GetStream())
                {         //分析完，再次监听服务器发过来的新消息
                    Array.Clear(byteBuffer, 0, byteBuffer.Length);   //清空数组
                    client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
                }
            }
            catch (Exception ex)
            {
                OnDisconnected(DisType.Exception, exceptionTag, ex.Message);
            }
        }

        public static void OnReadLine(IAsyncResult asr)
        {
            int bytesRead = 0;
            try
            {
                lock (client.GetStream())
                {
                    //读取字节流到缓冲区
                    bytesRead = client.GetStream().EndRead(asr);
                }
                if (bytesRead < 1)
                {
                    //包尺寸有问题，断线处理
                    Debug.Log("net manager read empty!");
                    OnDisconnected(DisType.Disconnect, disconnectTag, "bytesRead < 1");
                    return;
                }
                OnReceiveLine(byteBuffer, bytesRead);   //分析数据包内容，抛给逻辑层
                lock (client.GetStream())
                {   //分析完，再次监听服务器发过来的新消息
                    Array.Clear(byteBuffer, 0, byteBuffer.Length);   //清空数组
                    client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnReadLine), null);
                }
            }
            catch (Exception ex)
            {
                //PrintBytes();
                Debug.Log("net manager GetStream exeption!");
                OnDisconnected(DisType.Exception, exceptionTag, ex.Message);
            }
        }

        public static void OnDisconnected(int dis, byte[] bytes, string msg)
        {
            Close();   //关掉客户端链接
            Debug.Log("networkmanager on disconnect!" + msg + " trace:" + new System.Diagnostics.StackTrace().ToString());
            AddEvent(onDisConnectCallBack, bytes);
        }

        public static void PrintBytes()
        {
            string returnStr = string.Empty;
            for (int i = 0; i < byteBuffer.Length; i++)
            {
                returnStr += byteBuffer[i].ToString("X2");
            }
            Debug.LogError(returnStr);
        }

        public static void OnWrite(IAsyncResult r)
        {
            try
            {
                outStream.EndWrite(r);
            }
            catch (Exception ex)
            {
                Debug.LogError("OnWrite--->>>" + ex.Message);
            }
        }

        public static void OnReceiveLine(byte[] bytes, int length)
        {
            int line_start_index = 0;
            for (int i = 0; i < length; i++)
            {
                if (bytes[i] == (int)'\n')
                {
                    int can_read_len = i - line_start_index;
                    if (can_read_len > 0)
                    {
                        long msg_len = memStream.Length + can_read_len;
                        memStream.Seek(0, SeekOrigin.End);
                        memStream.Write(bytes, line_start_index, can_read_len);
                        memStream.Seek(0, SeekOrigin.Begin);
                        OnReceivedMessageLine(reader.ReadBytes((int)msg_len));
                        memStream.SetLength(0);
                    }
                    line_start_index = i + 1;
                }
            }
            int left_len = length - line_start_index;
            if (left_len > 0)
            {
                memStream.Seek(0, SeekOrigin.End);
                memStream.Write(bytes, line_start_index, left_len);
            }
        }

        public static void OnReceive(byte[] bytes, int length)
        {
            memStream.Seek(0, SeekOrigin.End);
            memStream.Write(bytes, 0, length);
            memStream.Seek(0, SeekOrigin.Begin);
            //Debug.Log("on receive RemainingBytes len : " + RemainingBytes().ToString()+ "  length len:" + length.ToString());
            while (RemainingBytes() > 2)
            {
                ushort messageLen = reader.ReadUInt16();
                messageLen = BytesUtility.SwapUInt16((UInt16)(messageLen));
                if (RemainingBytes() >= messageLen)
                {
                    OnReceivedMessage(reader.ReadBytes((int)messageLen));
                }
                else
                {
                    memStream.Position = memStream.Position - 2;
                    break;
                }
            }
            //Create a new stream with any leftover bytes
            byte[] leftover = reader.ReadBytes((int)RemainingBytes());
            memStream.SetLength(0);     //Clear
            memStream.Write(leftover, 0, leftover.Length);
        }

        public static long RemainingBytes()
        {
            return memStream.Length - memStream.Position;
        }

        public static void OnReceivedMessageLine(byte[] cmd_byte)
        {
            AddEvent(onReceiveLineCallBack, cmd_byte);
        }

        public static void OnReceivedMessage(byte[] cmd_byte)
        {
            AddEvent(onReceiveMsgCallBack, cmd_byte);
        }

        public static void Close()
        {
            if (client != null)
            {
                if (client.Connected)
                    client.Close();
                client = null;
            }
            // loggedIn = false;
        }

        public static void Destroy()
        {
            onConnectCallBack = null;
            onDisConnectCallBack = null;
            onReceiveLineCallBack = null;
            onReceiveMsgCallBack = null;
            if (client != null)
            {
                if (client.Connected) client.Close();
                client = null;
            }
            // loggedIn = false;
            reader.Close();
            memStream.Close();
            Debug.Log("~NetworkManager was destroy");
        }
    }
}