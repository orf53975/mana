using System;
using System.Net;
using System.Threading;

namespace mana.Foundation.Network.Server
{
    public abstract class UserToken
    {
        #region <<State>>
        internal const int kStateIdle = 0;
        internal const int kStateLink = 1;
        internal const int kStateFree = 2;
        #endregion

        public bool EnablePrintPacketInfo;

        protected readonly PacketRcver packetRcver;

        protected readonly PacketSnder packetSnder;

        public readonly AbstractServer server;

        public abstract EndPoint Address
        {
            get;
        }

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

        public object UserData
        {
            get;
            set;
        }

        protected UserToken(AbstractServer server)
        {
            this.server = server;
            packetRcver = new PacketRcver();
            packetSnder = new PacketSnder();
            this.state = kStateIdle;
        }

        public T GetServer<T>() where T : AbstractServer
        {
            return server as T;
        }

        #region <<rcv>>

        public void OnRecived(byte[] buffer, int offset, int count)
        {
            packetRcver.PushData(buffer, offset, count);
            var p = packetRcver.Build();
            while (p != null)
            {
                this.OnRecivedPacket(p);
                p.Release();
                p = packetRcver.Build();
            }
            lastActiveTime = Environment.TickCount;
        }

        private void OnRecivedPacket(Packet p)
        {
            try
            {
                if (EnablePrintPacketInfo)
                {
                    Logger.Print("UserToken[{0}] recived Packet[{1}]", uid, p.msgRoute);
                }
                if (!MessageDispatcher.Instance.Dispatch(this, p))
                {
                    Logger.Warning("unhandled : {0}", p.ToString());
                }
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
        }

        #endregion

        #region <<snd>>

        protected abstract void TryInvokeSendData();

        public void Send(Packet p)
        {
            if (EnablePrintPacketInfo)
            {
                Logger.Print("UserToken[{0}] Send Packet[{1}]", uid, p.msgRoute);
            }
            if (state != kStateLink)
            {
                Logger.Warning("UserToken.Send Failed! state = {0} error!", state);
                return;
            }
            packetSnder.Push(p);
            TryInvokeSendData();
        }

        public void SendResponse<T>(string route, int responseId, T data, string attachId = null)
            where T : class, ISerializable, new()
        {
            var p = Packet.CreatResponse(route, responseId, data);
            p.SetAttach(attachId);
            this.Send(p);
            p.Release();
        }

        public void SendResponse<T>(string route, int responseId, Action<T> rspSetter, string attachId = null)
            where T : class, ISerializable, new()
        {
            var p = Packet.CreatResponse(route, responseId, rspSetter);
            p.SetAttach(attachId);
            this.Send(p);
            p.Release();
        }

        public void SendPush<T>(string route, T data, string attachId = null)
            where T : class, ISerializable, new()
        {
            var p = Packet.CreatPush(route, data);
            p.SetAttach(attachId);
            this.Send(p);
            p.Release();
        }

        public void SendPush<T>(string route, Action<T> setter, string attachId = null)
            where T : class, ISerializable, new()
        {
            var p = Packet.CreatPush(route, setter);
            p.SetAttach(attachId);
            this.Send(p);
            p.Release();
        }

        #endregion

        #region <<Check TimeOut>>

        internal void CheckLinkStateTimeOut(int curTime)
        {
            if (server.bindTimeOut > 0 && TimeUtil.GetTimeSpan(curTime, startTime) > server.bindTimeOut)
            {
                Logger.Error("UserToken Kick! [CheckLinkStateTimeOut]");
                Release();
            }
        }

        internal void CheckWorkStateTimeOut(int curTime)
        {
            if (server.pingTimeOut > 0 && TimeUtil.GetTimeSpan(curTime, lastActiveTime) > server.pingTimeOut)
            {
                Logger.Error("UserToken Kick! [CheckWorkStateTimeOut]");
                Release();
            }
        }

        #endregion


        protected void InitState()
        {
            if (Interlocked.CompareExchange(ref state, kStateLink, kStateIdle) != kStateIdle)
            {
                throw new Exception("state error!");
            }
            this.startTime = this.lastActiveTime = Environment.TickCount;
            this.binded = false;
        }

        internal string TryBind(string id)
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

        internal void OnBinded()
        {
            this.binded = true;
        }

        internal void Reset()
        {
            if (Interlocked.Exchange(ref state, kStateIdle) != kStateFree)
            {
                Logger.Error("UserToken.Reset: state error!");
            }
            uid = null;
        }

        public void Release()
        {
            if (Interlocked.CompareExchange(ref state, kStateFree, kStateLink) != kStateLink)
            {
                return;
            }
            this.CloseChannel();
            packetRcver.Clear();
            packetSnder.Clear();
            server.WakeUpDeamon();
        }

        public string RemoteEndInfo()
        {
            return Address != null ? Address.ToString() : null;
        }

        protected abstract void CloseChannel();

    }
}