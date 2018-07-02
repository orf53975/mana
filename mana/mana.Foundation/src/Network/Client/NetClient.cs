using System;
using System.Net;

namespace mana.Foundation.Network.Client
{
    public abstract class NetClient :  IDisposable
    {
        private readonly NetResponseDispatcher responseDispatcher = new NetResponseDispatcher();

        private readonly NetPushDispatcher pushDispatcher = new NetPushDispatcher();

        public NetClient()
        {
            this.AddPushListener<Protocol>("Connector.Protocol", (protocol) =>
            {
                Protocol.Instance.Push(protocol);
            });
        }

        public abstract EndPoint RemoteEndPoint { get; }

        public abstract bool Connected { get; }

        public abstract void Connect(IPEndPoint ipep, Action<bool> callback);

        public abstract void Disconnect();

        public abstract void SendPacket(Packet p);

        public abstract void Update(int deltaTimeMs);

        public void Connect(string ip, ushort port, Action<bool> callback)
        {
            this.Connect(new IPEndPoint(IPAddress.Parse(ip), port), callback);
        }

        protected void OnPacketRecived(Packet p)
        {
            switch(p.msgType)
            {
                case Packet.MessageType.RESPONSE:
                    if (!responseDispatcher.Dispatch(p))
                    {
                        Logger.Warning("no handler! [{0}]", p.msgRoute);
                    }
                    break;
                case Packet.MessageType.PUSH:
                    if (!pushDispatcher.Dispatch(p))
                    {
                        Logger.Warning("no handler! [{0}]", p.msgRoute);
                    }
                    break;
                default:
                    Logger.Error("error type! [{0}]", p.msgType);
                    break;
            }
        }

        public void SendPingPacket()
        {
            this.Notify<Heartbeat>("Connector.Ping", (hb) =>
            {
                hb.timestamp = TimeUtil.GetCurrentTime();
            });
        }

        public void OnNetError()
        {
            Logger.Error("OnNetError");
            this.Disconnect();
        }

        public void OnHeartbeatTimeout()
        {
            Logger.Error("OnHeartbeatTimeout");
            this.Disconnect();
        }


        #region <<About Request>>

        private int requestIdGenIndex = 0;
        public int GenRequestId()
        {
            return ++requestIdGenIndex;
        }

        public bool Request<TREQ, TRSP>(string route, Action<TREQ> reqSetter, Action<TRSP> rspHandler)
            where TREQ : class, DataObject, new()
            where TRSP : class, DataObject, new()
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Request)
            {
                Logger.Error("proto type not match! [{0}->REQRSP]", proto.ptype);
                return false;
            }
            var uldt = typeof(TREQ).FullName;
            if (proto.c2sdt != uldt)
            {
                Logger.Error("proto uldt not match! [{0}->{1}]", proto.c2sdt, uldt);
                return false;
            }
            var dldt = typeof(TRSP).FullName;
            if (proto.s2cdt != dldt)
            {
                Logger.Error("proto dldt not match! [{0}->{1}]", proto.s2cdt, dldt);
                return false;
            }
            var requestId = this.GenRequestId();
            var p = Packet.CreatRequest(route, requestId, reqSetter);
            this.SendPacket(p);
            this.responseDispatcher.Register(requestId, rspHandler);
            return true;
        }

        public bool Request<TRSP>(string route, Action<TRSP> rspHandler)
            where TRSP : class, DataObject, new()
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Request)
            {
                Logger.Error("proto type not match! [{0}->REQRSP]", proto.ptype);
                return false;
            }
            if (proto.c2sdt != null)
            {
                Logger.Error("proto uldt not match! [{0}->null]", proto.c2sdt);
                return false;
            }
            var dldt = typeof(TRSP).FullName;
            if (proto.s2cdt != dldt)
            {
                Logger.Error("proto dldt not match! [{0}->{1}]", proto.s2cdt, dldt);
                return false;
            }
            var requestId = this.GenRequestId();
            var p = Packet.CreatRequest(route, requestId, null);
            this.SendPacket(p);
            this.responseDispatcher.Register(requestId, rspHandler);
            return true;
        }

        public bool Request(string route, Action<DDNode> reqSetter, Action<DDNode> rspHandler)
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Request)
            {
                Logger.Error("proto type not match! [{0}->REQRSP]", proto.ptype);
                return false;
            }
            var d = ObjectCache.Get(reqSetter);
            if (proto.c2sdt != d.Tmpl.fullName)
            {
                Logger.Error("proto uldt not match! [{0}->{1}]", proto.c2sdt, d.Tmpl.fullName);
                return false;
            }
            var requestId = this.GenRequestId();
            var p = Packet.CreatRequest(route, requestId, d);
            this.SendPacket(p);
            this.responseDispatcher.Register(requestId, proto, rspHandler);
            return true;
        }

        public bool Request(string route, Action<DDNode> rspHandler)
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Request)
            {
                Logger.Error("proto type not match! [{0}->REQRSP]", proto.ptype);
                return false;
            }
            if (proto.c2sdt != null)
            {
                Logger.Error("proto uldt not match! [{0}->null]", proto.c2sdt);
                return false;
            }
            var requestId = this.GenRequestId();
            var p = Packet.CreatRequest(route, requestId, null);
            this.SendPacket(p);
            this.responseDispatcher.Register(requestId, proto, rspHandler);
            return true;
        }

        #endregion

        #region <<About Notify>>

        public bool Notify<T>(string route, Action<T> notifySetter)
            where T : class, DataObject, new()
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Notify)
            {
                Logger.Error("proto type not match! [{0}->Notify]", proto.ptype);
                return false;
            }
            var uldt = typeof(T).FullName;
            if (proto.c2sdt != uldt)
            {
                Logger.Error("proto uldt not match! [{0}->{1}]", proto.c2sdt, uldt);
                return false;
            }
            var p = Packet.CreatNotify(route, notifySetter);
            this.SendPacket(p);
            return true;
        }

        public bool Notify(string route, Action<DDNode> notifySetter)
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Notify)
            {
                Logger.Error("proto type not match! [{0}->Notify]", proto.ptype);
                return false;
            }
            var d = ObjectCache.Get(notifySetter);
            if (proto.c2sdt != d.Tmpl.fullName)
            {
                Logger.Error("proto uldt type not match! [{0}->{1}]", proto.c2sdt, d.Tmpl.fullName);
                return false;
            }
            var p = Packet.CreatNotify(route, d);
            this.SendPacket(p);
            return true;
        }

        public bool Notify(string route)
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Notify)
            {
                Logger.Error("proto type not match! [{0}->Notify]", proto.ptype);
                return false;
            }
            var p = Packet.CreatNotify(route, null);
            this.SendPacket(p);
            return true;
        }

        #endregion

        #region <<About Push>>

        public bool AddPushListener<T>(string route, Action<T> handler)
            where T : class, DataObject, new()
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Push)
            {
                Logger.Error("proto type not match! [{0}->Push]", proto.ptype);
                return false;
            }
            var dldt = typeof(T).FullName;
            if (proto.s2cdt != dldt)
            {
                Logger.Error("proto dldt not match! [{0}->{1}]", proto.s2cdt, dldt);
                return false;
            }
            pushDispatcher.Register(route, handler);
            return true;
        }

        public bool AddPushListener(string route, Action<DDNode> handler)
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Push)
            {
                Logger.Error("proto type not match! [{0}->Push]", proto.ptype);
                return false;
            }
            pushDispatcher.Register(proto, handler);
            return true;
        }

        public bool RemovePushListener<T>(string route, Action<T> handler)
             where T : class, DataObject, new()
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Push)
            {
                Logger.Error("proto type not match! [{0}->Push]", proto.ptype);
                return false;
            }
            pushDispatcher.Unregister(route, handler);
            return true;
        }

        public bool RemovePushListener(string route, Action<DDNode> handler)
        {
            var proto = Protocol.Instance.GetProto(route);
            if (proto == null)
            {
                Logger.Error("route[{0}] error!", route);
                return false;
            }
            if (proto.ptype != ProtoType.Push)
            {
                Logger.Error("proto type not match! [{0}->Push]", proto.ptype);
                return false;
            }
            pushDispatcher.Unregister(proto, handler);
            return true;
        }
        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // 要检测冗余调用

        protected bool IsDisposed
        {
            get
            {
                return disposedValue;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~NetClient() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}