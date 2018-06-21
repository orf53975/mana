using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public sealed class NetClient
    {
        public delegate void NetRecived<out T>(DataObject dat);

        #region <<Class ResponseDispatcher>>

        internal class ResponseDispatcher
        {
            private readonly Dictionary<int, Delegate> handlerDic = new Dictionary<int, Delegate>();

            private readonly Dictionary<int, string> responsesTypes = new Dictionary<int, string>();

            public void Register<T>(int responseId, Action<T> handler)
                where T : class, DataObject, new()
            {
                handlerDic.Add(responseId, handler);
            }

            public bool Dispatch(Packet p)
            {
                //NetRecived<DataObject> handler = null;
                //if (responsesHandlerDic.TryGetValue(p.msgRequestId, out handler))
                //{
                //    //handler.DynamicInvoke(p.Decode<>());
                //    responsesHandlerDic.Remove(p.msgRequestId);
                //    //responsesTypes.Remove();
                //}
                //else
                //{
                //    Logger.Warning("process response failed! [{0}]", p);
                //}
                return false;
            }
        }
        #endregion

        #region <<Class PushDispatcher>>

        internal class PushDispatcher
        {
            private readonly Dictionary<string, List<NetRecived<DataObject>>> pushHandlerDic = new Dictionary<string, List<NetRecived<DataObject>>>();

            private List<NetRecived<DataObject>> TryGetHandlerList(string route)
            {
                //List<NetRecived<DataObject>> hl = null;
                //if (pushHandlerDic.TryGetValue(route, out hl))
                //{
                //    return hl;
                //}
                //return null;
                return null;
            }

            internal void Register<T>(string route, Action<T> handler)
                where T : class, DataObject, new()
            {
                //var hl = TryGetHandlerList(route);
                //if (hl == null)
                //{
                //    hl = new List<NetRecived<DataObject>>();
                //    pushHandlerDic.Add(route, hl);
                //}
                //var ha = (Action<DataObject>)handler;
                //if (hl.Contains(ha))
                //{
                //    Logger.Error("Register Duplicate!");
                //}
                //else
                //{
                //    hl.Add(ha);
                //}
            }

            internal void Unregister<T>(string route, Action<T> handler)
                where T : class, DataObject, new()
            {
                //var hl = TryGetHandlerList(route);
                //if (hl == null)
                //{
                //    Logger.Error("handler remove failed, Not found!");
                //}

                //if (!hl.Remove(ha))
                //{
                //    Logger.Error("handler remove failed, Not found!");
                //}
            }

            internal bool Dispatch(Packet p)
            {
                //var hl = TryGetHandlerList(p.msgRoute);
                //var proto = Protocol.Instance.GetProto(p.msgRoute);
                //if (hl == null || hl.Count == 0)
                //{
                //    return false;
                //}
                //for (int i = 0; i < hl.Count; i++)
                //{
                //    hl[i].Invoke(p.TryGetDataObject(proto.dldt));
                //}
                //return true;

                return false;
            }
        }
        #endregion

        #region <<Static Functions>>

        private static void CheckProtoError(string route, ProtoType pt, string uldt, string dldt)
        {
            var code = Protocol.Instance.GetRouteCode(route);
            if (code != 0)
            {
                var proto = Protocol.Instance.GetProto(code);
                if (proto.type != pt)
                {
                    Logger.Error("proto type not match! [{0}->{1}]", proto.type, pt);
                }
                if (proto.uldt != uldt)
                {
                    Logger.Error("proto uldt type not match! [{0}]", uldt);
                }
                if (proto.dldt != dldt)
                {
                    Logger.Error("proto uldt type not match! [{0}]", dldt);
                }
            }
            else
            {
                Logger.Error("route not existed! [{0}]", route);
            }
        }
        #endregion

        private readonly ResponseDispatcher responseDispatcher = new ResponseDispatcher();

        private readonly PushDispatcher pushDispatcher = new PushDispatcher();

        private NetChannel channel = null;

        public bool EnableCheckError = false;

        public NetClient(NetChannel c)
        {
            this.channel = c;
            this.channel.AddListener(OnRecived);
        }

        public void Connect(string ip, ushort port, Action<bool, Exception> callback)
        {
            this.channel.StartConnect(ip, port, callback);

        }

        private int requestIdGenIndex = 0;
        public int GenRequestId()
        {
            return ++requestIdGenIndex;
        }

        public void Request<TREQ, TRSP>(string route, Action<TREQ> reqSetter, Action<TRSP> rspHandler)
            where TREQ : class, DataObject, Cacheable, new()
            where TRSP : class, DataObject, Cacheable, new()
        {
            if (EnableCheckError)
            {
                CheckProtoError(route, ProtoType.REQRSP, typeof(TREQ).FullName, typeof(TRSP).FullName);
            }
            var d = ObjectCache.Get<TREQ>(reqSetter);
            var requestId = this.GenRequestId();
            var p = Packet.CreatRequest(route, requestId, d);
            this.channel.Send(p);
            this.responseDispatcher.Register(requestId, rspHandler);

        }

        public void Request<TRSP>(string route, Action<TRSP> rspHandler)
            where TRSP : class, DataObject, Cacheable, new()
        {
            if (EnableCheckError)
            {
                CheckProtoError(route, ProtoType.REQRSP, null, typeof(TRSP).FullName);
            }
            var requestId = this.GenRequestId();
            var p = Packet.CreatRequest(route, requestId, null);
            this.channel.Send(p);
            this.responseDispatcher.Register(requestId, rspHandler);
        }


        public void Request(string route, Action<DataNode> reqSetter, Action<DataNode> rspHandler)
        {
            var d = ObjectCache.Get<DataNode>(reqSetter);
            if (EnableCheckError)
            {
                CheckProtoError(route, ProtoType.REQRSP, d.Tmpl.name, null);
            }
            var requestId = this.GenRequestId();
            var p = Packet.CreatRequest(route, requestId, d);
            this.channel.Send(p);
            this.responseDispatcher.Register(requestId, rspHandler);
        }

        public void Request(string route, Action<DataNode> rspHandler)
        {
            var requestId = this.GenRequestId();
            var p = Packet.CreatRequest(route, requestId, null);
            this.channel.Send(p);
            this.responseDispatcher.Register(requestId, rspHandler);
        }

        public void Notify<T>(string route, Action<T> notifySetter)
            where T : class, DataObject, Cacheable, new()
        {
            if (EnableCheckError)
            {
                CheckProtoError(route, ProtoType.NOTIFY, typeof(T).FullName, null);
            }
            var d = ObjectCache.Get<T>(notifySetter);
            var p = Packet.CreatNotify(route, d);
            this.channel.Send(p);
        }

        public void Notify(string route, Action<DataNode> notifySetter)
        {
            var d = ObjectCache.Get<DataNode>(notifySetter);
            if (EnableCheckError)
            {
                CheckProtoError(route, ProtoType.NOTIFY, d.Tmpl.name, null);
            }
            var p = Packet.CreatNotify(route, d);
            this.channel.Send(p);
        }

        public void Notify(string route)
        {
            var p = Packet.CreatNotify(route, null);
            this.channel.Send(p);
        }

        public void AddPushListener<T>(string route, Action<T> handler)
            where T : class, DataObject, Cacheable, new()
        {
            if (EnableCheckError)
            {
                CheckProtoError(route, ProtoType.PUSH, typeof(T).FullName, null);
            }
            pushDispatcher.Register(route, handler);
        }

        public void AddPushListener(string route, Action<DataNode> handler)
        {
            pushDispatcher.Register(route, handler);
        }

        public void RemovePushListener<T>(string route, Action<T> handler)
             where T : class, DataObject, Cacheable, new()
        {
            pushDispatcher.Unregister(route, handler);
        }

        public void RemovePushListener(string route, Action<DataNode> handler)
        {
            pushDispatcher.Unregister(route, handler);
        }

        private void OnRecived(Packet p)
        {
            if (p.msgType == Packet.MessageType.RESPONSE)
            {
                if (responseDispatcher.Dispatch(p))
                {
                    Logger.Warning("no handler!", p);
                }
                return;
            }
            if (p.msgType == Packet.MessageType.PUSH)
            {
                if (pushDispatcher.Dispatch(p))
                {
                    Logger.Warning("no handler!", p);
                }
                return;
            }
            Logger.Error("error type[{0}]!", p.msgType);
        }
    }
}