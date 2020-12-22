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
using System.Threading.Tasks;
using System.Threading;
using Sproto;

namespace Game
{
	public struct NetEventElement
	{
		public Action<ushort, ushort, byte[]> callBack;
		public ushort protoID;
		public ushort RPCID;
		public byte[] message;

		public NetEventElement(Action<ushort, ushort, byte[]> callBack, ushort protoID, ushort RPCID, byte[] message)
		{
			this.callBack = callBack;
			this.protoID = protoID;
			this.RPCID = RPCID;
			this.message = message;
		}
	}

	public enum NetStateEvent
	{
		ConnectSucess = 1,          //连接成功
		ConnectFailed = 2,          //连接失败
		Exception = 3,              //异常断开
		Disconnect = 4,             //客户端或服务端正常断开
		ReConnectSucess = 5,        //重连成功
		ReConnectFailed = 6,        //重连失败
	}

	public class TcpManager
	{
		public static bool LogMessageInConsole
		{
			get
			{
				return GameInstaller.Instance.GameSettings.LogDebug;
			}
		}
		public static long ServerTime = 0;
		public static long HeartTime = 0;
		public static long NetworkDelayInMs = 10000;// 最短的一次网络延迟 ms
		public static bool Heart = false;
		private static readonly object m_lockObject = new object();
		private static Queue<NetEventElement> mEvents = new Queue<NetEventElement>();
		private static TcpClient client = null;
		private static NetworkStream outStream = null;
		private static MemoryStream memStream;
		private static BinaryReader reader;
		private const int MAX_READ = 8192;
		// private int session = 0;
		// private int maxSession = int.MaxValue/2;
		private static byte[] byteBuffer = new byte[MAX_READ];
		// public static bool loggedIn = false;
		public const int MinLuaNetSessionID = System.Int32.MaxValue / 2;
		public const int MaxLuaNetSessionID = System.Int32.MaxValue;
		public static Action<ushort, ushort, byte[]> OnConnectEventCallBack = null;
		public static Action<ushort, ushort, byte[]> OnReceiveMsgCallBack = null;

		private static Dictionary<ushort, Action<byte[]>> m_ListenerMap = new Dictionary<ushort, Action<byte[]>>();
		/// <summary>
		/// 单次连接超时
		/// </summary>
		public static int timeoutMiliSecond = 5000;
		/// <summary>
		/// 最多重连次数
		/// </summary>
		public static int maxReconnectTime = 0;
		/// <summary>
		/// 当前重连次数
		/// </summary>
		private static int reconnectTime = 0;

		private static CancellationTokenSource tokenSource;

		public static long TimeStamp;

		public static async Task Init()
		{
			memStream = new MemoryStream();
			reader = new BinaryReader(memStream);
			Heart = false;
		}

		private static void AddEvent(Action<ushort, ushort, byte[]> _event, ushort protoID, ushort RPCID, byte[] data)
		{
			mEvents.Enqueue(new NetEventElement(_event, protoID, RPCID, data));
		}

		private static void StartHeart()
		{
			if (!Heart)
			{
				Func<System.Threading.Tasks.Task> func = async () =>
				{
					try
					{
						while (Heart)
						{
							SendBytes(1003, null); // 发送心跳包
							HeartTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
							await System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(5f));
						}
					}
					catch (System.Exception)
					{
						return;
					}
				};
				Heart = true;
				//ProtoManager.AddListener(ProtoCSharpType.HeartRes, ReceiveHeartMessage);
				func();
			}
		}

		private static void StopHeart()
		{
			Heart = false;
			//ProtoManager.RemoveListener(ProtoCSharpType.HeartRes, ReceiveHeartMessage);
		}

//		private static void ReceiveHeartMessage(IProtoDataStruct message)
//		{
//			HeartRes _message = message as HeartRes;
//			long newDelayInMs = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
//			newDelayInMs -= HeartTime; // 网络延迟(毫秒)
//									   // 网络延迟小于100毫秒 且服务器时间与本地时钟的偏差大于500毫秒, 矫正时钟 || 获得更小延迟的时间戳(更接近服务器时间), 矫正时钟
//			if ((newDelayInMs < 100 && Mathf.Abs(_message.serverTime - ServerTime) > 500) || newDelayInMs < NetworkDelayInMs)
//			{
//				NetworkDelayInMs = newDelayInMs;
//				ServerTime = _message.serverTime;
//			}
//#if UNITY_EDITOR
//			Debug.LogWarning($"收到心跳:{_message.serverTime}");
//#endif
//		}

		public static void Update()
		{
			if (mEvents.Count > 0)
			{
				while (mEvents.Count > 0)
				{
					NetEventElement _event = mEvents.Dequeue();
					//if (ProtoCSharpList.IsCSharp(_event.protoID))
					//	ProtoManager.Receive(_event.protoID, _event.message);
					//else
						_event.callBack(_event.protoID, _event.RPCID, _event.message);
				}
			}
		}

		public static void FixedUpdate()
		{
			ServerTime += (long)(Time.fixedDeltaTime * 1000);
		}


		public static void ReConnect(string host, int port, int timeoutMiliSecondx, int maxReconnectTimex)
		{
			reconnectTime = 0;
			reconnectTime++;
			maxReconnectTime = maxReconnectTimex;
			Connect(host, port, timeoutMiliSecondx);
		}

		public static void Connect(string host, int port, int timeoutMiliSecondx = 5000)
		{
			Close();
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

				timeoutMiliSecond = timeoutMiliSecondx;
				var source = new CancellationTokenSource();
				tokenSource = source;
				Task.Run(async () =>
				{
					await Task.Delay(System.TimeSpan.FromMilliseconds(timeoutMiliSecond));
					if (source.IsCancellationRequested) return;
					if (client != null && client.Connected) return;
					if (reconnectTime > 0)
					{
						OnDisconnected(NetStateEvent.ReConnectFailed, "Reconnect Failed");
						if (reconnectTime < maxReconnectTime)
						{
							reconnectTime++;
							Connect(host, port);
						}
						else
							reconnectTime = 0;
					}
					else
					{
						OnDisconnected(NetStateEvent.ConnectFailed, "ConnectFailed");
					}

				}, source.Token);


				client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
			}
			catch (Exception e)
			{
				tokenSource = null;
				OnDisconnected(NetStateEvent.ConnectFailed, "ConnectFailed");
				Debug.LogError(e.Message);
			}
		}

		public static void OnConnect(IAsyncResult asr)
		{
			if (!client.Connected) return;
			if (tokenSource != null)
			{
				tokenSource.Cancel();
				tokenSource = null;
			}

			if (reconnectTime == 0)
				AddEvent(OnConnectEventCallBack, (int)NetStateEvent.ConnectSucess, 0, null);
			else
			{
				reconnectTime = 0;
				AddEvent(OnConnectEventCallBack, (int)NetStateEvent.ReConnectSucess, 0, null);
			}

			outStream = client.GetStream();
			client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);

			//StartHeart();
		}

		public static void SendBytes(ushort protoID, byte[] message, ushort rpcId = 0)
		{
			try
			{
				MemoryStream ms = null;
				int messageLength = message == null ? 2 : message.Length + 2;
				using (ms = new MemoryStream())
				{
					ms.Position = 0;
					BinaryWriter writer = new BinaryWriter(ms);
					if (rpcId > 0)
					{
						ushort msglen = BytesUtility.SwapUInt16((ushort)(messageLength + 2));
						writer.Write(msglen);
						ushort protoid = BytesUtility.SwapUInt16(protoID);
						writer.Write(protoid);
						ushort rpcid = BytesUtility.SwapUInt16(rpcId);
						writer.Write(rpcid);
					}
					else
					{
						ushort msglen = BytesUtility.SwapUInt16((ushort)messageLength);
						writer.Write(msglen);
						ushort protoid = BytesUtility.SwapUInt16(protoID);
						writer.Write(protoid);
					}

					if (message != null)
						writer.Write(message);
					writer.Flush();
					if (client != null && client.Connected)
					{
						byte[] payload = ms.ToArray();
						outStream.BeginWrite(payload, 0, payload.Length, new AsyncCallback(OnWrite), null);
					}
					else
					{
						// Debug.LogError("client.connected----->>false");
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}
		}

		public static void AddListener(ushort msgId, Action<byte[]> callback)
		{
			if (m_ListenerMap.ContainsKey(msgId))
				Debug.LogError($"覆盖了消息: {msgId}");
			m_ListenerMap[msgId] = callback;
		}

		public static void RemoveListener(ushort msgId)
		{
			if (m_ListenerMap.ContainsKey(msgId))
				m_ListenerMap[msgId] = null;
		}

		public static void OnRead(IAsyncResult asr)
		{
			int bytesRead = 0;
			try
			{
				if (client != null)
				{
					bytesRead = client.GetStream().EndRead(asr);
					if (bytesRead < 1)
					{
						//包尺寸有问题，断线处理
						Debug.Log("net manager read empty!");
						OnDisconnected(NetStateEvent.Disconnect, "bytesRead < 1");

						return;
					}
					//分析数据包内容，抛给逻辑层
					OnReceive(byteBuffer, bytesRead);
					//分析完，再次监听服务器发过来的新消息
					Array.Clear(byteBuffer, 0, byteBuffer.Length);   //清空数组
					client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
				}
				else
				{
					//这种情况一般是客户端主动断开
				}

			}
			catch (Exception ex)
			{
				OnDisconnected(NetStateEvent.Exception, ex.Message);
			}
		}

		public static void OnDisconnected(NetStateEvent netState, string msg)
		{
			if (client == null) return;
			Close();   //关掉客户端链接
			AddEvent(OnConnectEventCallBack, (ushort)netState, (ushort)reconnectTime, null);
			Debug.Log("networkmanager on disconnect!" + msg + " trace:" + new System.Diagnostics.StackTrace().ToString());
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

		public static void OnReceive(byte[] bytes, int length)
		{
			
			memStream.Seek(0, SeekOrigin.End);
			memStream.Write(bytes, 0, length);
			memStream.Seek(0, SeekOrigin.Begin);
			while (RemainingBytes() >= 4)
			{
				ushort messageLen = reader.ReadUInt16();
				messageLen = (ushort)(BytesUtility.SwapUInt16(messageLen) - 2);
				ushort protoId = reader.ReadUInt16();
				protoId = BytesUtility.SwapUInt16(protoId);
				ushort rpcId = 0;
				if (protoId > 20000)
				{
					rpcId = reader.ReadUInt16();
					rpcId = BytesUtility.SwapUInt16(rpcId);
					messageLen = (ushort)(messageLen - 2);
				}
				if (RemainingBytes() >= messageLen)
				{
					byte[] bytes1 = reader.ReadBytes(messageLen);
					Action<byte[]> callback;
					if (m_ListenerMap.TryGetValue(protoId, out callback))
						callback.Invoke(bytes1);
					else
						OnReceivedMessage(protoId, rpcId, bytes1);
					//Debug.LogError($"{RemainingBytes()}：{messageLen}");
				}
				else
				{
					memStream.Position = memStream.Position;
					break;
				}
			}
			byte[] leftover = reader.ReadBytes((int)RemainingBytes());
			memStream.SetLength(0);
			memStream.Write(leftover, 0, leftover.Length);
		}

		public static long RemainingBytes()
		{
			return memStream.Length - memStream.Position;
		}

		public static void OnReceivedMessage(ushort protoID, ushort RPCID, byte[] cmd_byte)
		{
			AddEvent(OnReceiveMsgCallBack, protoID, RPCID, cmd_byte);
		}

		public static void StopReconnect()
		{
			if (tokenSource != null)
			{
				tokenSource.Cancel();
				tokenSource = null;
				reconnectTime = 0;
			}
			Close();
		}

		public static void Close()
		{
			if (client != null)
			{
				StopHeart();
				try
				{
					var tcp = client;
					client = null;
					if (tcp.Client != null && tcp.Connected)
						tcp.GetStream().Close();
					tcp.Close();
				}
				catch (Exception ex)
				{
					Debug.LogError("关闭报错：" + ex.ToString());
				}
			}
		}

		public static void Dispose()
		{
			OnConnectEventCallBack = null;
			OnReceiveMsgCallBack = null;
			if (client != null)
			{
				if (client.Connected) client.Close();
				client = null;
			}
			// loggedIn = false;
			reader.Close();
			memStream.Close();
			StopHeart();
			Debug.Log("~NetworkManager was destroy");
		}
	}
}