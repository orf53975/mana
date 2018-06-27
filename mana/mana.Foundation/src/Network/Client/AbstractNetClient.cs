﻿using System;

namespace mana.Foundation.Network.Client
{
    public abstract class AbstractNetClient
    {
        private readonly NetResponseDispatcher responseDispatcher = new NetResponseDispatcher();

        private readonly NetPushDispatcher pushDispatcher = new NetPushDispatcher();

        public AbstractNetClient() { }

        public abstract void Connect(string ip, ushort port, Action<bool, Exception> callback);

        public abstract void Disconnect();

        public abstract void SendPacket(Packet p);

        protected virtual void OnNetError()
        {
            //TODO
        }

        public void SendPingPacket()
        {
            this.Notify<Heartbeat>("Connector.Ping", (hb) =>
            {
                hb.timestamp = TimeUtil.GetCurrentTime();
            });
        }

        protected void OnRecivedPacket(Packet p)
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
            p.ReleaseToPool();
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

    }
}