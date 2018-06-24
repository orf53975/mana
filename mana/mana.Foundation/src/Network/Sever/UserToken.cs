﻿using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace mana.Foundation
{
    public sealed class UserToken
    {
        #region <<State>>
        internal const int kStateIdle = 0;
        internal const int kStateLink = 1;
        internal const int kStateFree = 2;
        #endregion

        internal const int kDefaultBufferSize = 1024;

        readonly SocketAsyncEventArgs rcvEventArg;

        readonly SocketAsyncEventArgs sndEventArg;

        readonly int SendBufferOffset;

        readonly int SendBufferLimit;

        readonly PacketRcver packetRcver;

        readonly PacketSnder packetSnder;

        readonly IOCPServer server;

        public int startTime
        {
            get;
            private set;
        }

        public int lastActiveTime
        {
            get;
            private set;
        }

        public Socket socket
        {
            get;
            private set;
        }

        public string uid
        {
            get;
            private set;
        }

        public bool isWorking
        {
            get
            {
                return state == kStateLink && binded;
            }
        }

        public bool binded
        {
            get;
            private set;
        }

        int state = kStateIdle;
        public int State
        {
            get
            {
                return state;
            }
        }

        internal UserToken(BufferManager bufferManager, IOCPServer server)
        {
            rcvEventArg = new SocketAsyncEventArgs();
            rcvEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(RcvCompleted);
            rcvEventArg.UserToken = this;
            if (bufferManager == null || bufferManager.SetBuffer(rcvEventArg))
            {
                rcvEventArg.SetBuffer(new byte[kDefaultBufferSize], 0, kDefaultBufferSize);
            }
            sndEventArg = new SocketAsyncEventArgs();
            sndEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(SndCompleted);
            sndEventArg.UserToken = this;
            if (bufferManager == null || bufferManager.SetBuffer(sndEventArg))
            {
                sndEventArg.SetBuffer(new byte[kDefaultBufferSize], 0, kDefaultBufferSize);
            }
            SendBufferOffset = sndEventArg.Offset;
            SendBufferLimit = SendBufferOffset + sndEventArg.Count;

            this.server = server;
            this.packetRcver = new PacketRcver();
            this.packetSnder = new PacketSnder();
            this.state = kStateIdle;
        }


        internal void Init(Socket socket)
        {
            if (Interlocked.CompareExchange(ref state, kStateLink, kStateIdle) != kStateIdle)
            {
                throw new Exception("state error!");
            }
            if (socket == null)
            {
                throw new NullReferenceException();
            }
            this.socket = socket;

            this.startTime = this.lastActiveTime = Environment.TickCount;
            this.binded = false;
            this.StartRcv();
        }

        internal void OnBinded()
        {
            this.binded = true;
        }

        public T GetServer<T>() where T : IOCPServer
        {
            return server as T;
        }

        void StartRcv()
        {
            var willRaiseEvent = this.socket.ReceiveAsync(rcvEventArg);
            if (!willRaiseEvent)
            {
                ProcessRcved(rcvEventArg);
            }
        }

        void RcvCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Receive)
            {
                this.ProcessRcved(e);
            }
            else
            {
                Trace.TraceError("The last operation completed on the socket was not a rcv.");
                this.Release();
            }
        }

        void SndCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Send)
            {
                this.ProcessSnded(e);
            }
            else
            {
                Trace.TraceError("The last operation completed on the socket was not a snd.");
                this.Release();
            }
        }

        void ProcessRcved(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                this.OnRcv(e.Buffer, e.Offset, e.BytesTransferred);
                if (!socket.ReceiveAsync(e))
                {
                    this.ProcessRcved(e);
                }
            }
            else
            {
                this.Release();
            }
        }

        void OnRcv(byte[] buffer, int offset, int count)
        {
            packetRcver.PushData(buffer, offset, count);

            var p = packetRcver.Build();
            if (p != null)
            {
                this.OnRecived(p);
            }
            lastActiveTime = Environment.TickCount;
        }

        void ProcessSnded(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                this.DoSending(e);
            }
            else
            {
                this.Release();
            }
        }

        public string TryBind(string id)
        {
            if (state != kStateLink)
            {
                return string.Format("UserToken.Bind Failed! state = {0} error!", state);
            }
            if (string.IsNullOrEmpty(id))
            {
                return "UserToken.Bind Failed! is is null or empty!";
            }
            if (!string.IsNullOrEmpty(uid))
            {
                return "UserToken.Bind Failed! has already binded uid!";
            }
            this.uid = id;
            server.WakeUpDeamon();
            return null;
        }

        void DoSending(SocketAsyncEventArgs e)
        {
            var offset = SendBufferOffset;
            while (offset < SendBufferLimit && packetSnder.HasSendingData)
            {
                packetSnder.WriteTo(e.Buffer, ref offset, SendBufferLimit);
            }
            var nSndBytes = offset - SendBufferOffset;
            if (nSndBytes != 0)
            {
                e.SetBuffer(SendBufferOffset, nSndBytes);
                var willRaiseEvent = socket.SendAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessSnded(e);
                }
            }
        }

        private void OnRecived(Packet p)
        {
            try
            {
                if (!MessageDispatcher.Instance.Dispatch(this, p))
                {
                    Trace.TraceWarning("unhandled : " + p.ToString());
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }

        public void Send(Packet p)
        {
            if (state != kStateLink)
            {
                Trace.TraceWarning("UserToken.Send Failed! state = {0} error!", state);
                return;
            }
            var bInSending = packetSnder.HasSendingData;
            packetSnder.Push(p);
            if (!bInSending)
            {
                DoSending(sndEventArg);
            }
        }

        public void SendResponse(string route, int responseId, ISerializable dataObj)
        {
            this.Send(Packet.CreatResponse(route, responseId, dataObj));
        }

        public void SendPush(string route, ISerializable dataObj)
        {
            this.Send(Packet.CreatPush(route, dataObj));
        }

        void CloseSocket()
        {
            var addr = socket.RemoteEndPoint.ToString();
            try
            {
                if (socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
            finally
            {
                socket.Close();
                this.server.OnSocketClosed(addr);
                socket = null;
            }
        }

        void Release()
        {
            if (Interlocked.CompareExchange(ref state, kStateFree, kStateLink) != kStateLink)
            {
                return;
            }
            CloseSocket();
            this.server.WakeUpDeamon();
            packetRcver.Clear();
            packetSnder.Clear();
        }

        internal void Reset()
        {
            if (Interlocked.Exchange(ref state, kStateIdle) != kStateFree)
            {
                Trace.TraceError("UserToken.Reset: state error!");
            }
            uid = null;
        }

        internal void CheckLinkStateTimeOut(int curTime)
        {
            if (Utils.GetTimeSpan(curTime, startTime) > server.tokenUnbindTimeOut)
            {
                Release();
            }
        }

        internal void CheckWorkStateTimeOut(int curTime)
        {
            if (Utils.GetTimeSpan(curTime, lastActiveTime) > server.tokenWorkTimeOut)
            {
                Release();
            }
        }
    }
}