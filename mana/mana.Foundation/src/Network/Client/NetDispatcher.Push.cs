using System;
using System.Collections.Generic;

namespace mana.Foundation.Network.Client
{
    internal class NetPushDispatcher
    {
        readonly Dictionary<string, List<INetRecivedCallBack>> pushHandlerDic = new Dictionary<string, List<INetRecivedCallBack>>();

        private List<INetRecivedCallBack> TryGetHandlerList(string route)
        {
            List<INetRecivedCallBack> hl;
            if (pushHandlerDic.TryGetValue(route, out hl))
            {
                return hl;
            }
            return null;
        }

        internal void Register<T>(string route, Action<T> handler)
            where T : class, DataObject, new()
        {
            var hl = TryGetHandlerList(route);
            if (hl == null)
            {
                hl = new List<INetRecivedCallBack>();
                pushHandlerDic.Add(route, hl);
            }
            var nrcb = new NetRecivedCallBack<T>(handler);
            if (hl.Contains(nrcb))
            {
                Logger.Error("Register Duplicate!");
            }
            else
            {
                hl.Add(nrcb);
            }
        }

        internal void Unregister<T>(string route, Action<T> handler)
            where T : class, DataObject, new()
        {
            var hl = TryGetHandlerList(route);
            if (hl != null)
            {
                var nrcb = new NetRecivedCallBack<T>(handler);
                if (!hl.Remove(nrcb))
                {
                    Logger.Error("Unregister failed! , Not Existed Callback");
                }
            }
            else
            {
                Logger.Error("Unregister failed!");
            }
        }

        public void Register(Proto proto, Action<DDNode> handler)
        {
            var hl = TryGetHandlerList(proto.route);
            if (hl == null)
            {
                hl = new List<INetRecivedCallBack>();
                pushHandlerDic.Add(proto.route, hl);
            }
            var nrcb = new NetRecivedCallBack(proto.s2cdt, handler);
            if (hl.Contains(nrcb))
            {
                Logger.Error("Register Duplicate!");
            }
            else
            {
                hl.Add(nrcb);
            }
        }

        public void Unregister(Proto proto, Action<DDNode> handler)
        {
            var hl = TryGetHandlerList(proto.route);
            if (hl != null)
            {
                var nrcb = new NetRecivedCallBack(proto.s2cdt, handler);
                if (!hl.Remove(nrcb))
                {
                    Logger.Error("Unregister failed! , Not Existed Callback");
                }
            }
            else
            {
                Logger.Error("Unregister failed!");
            }
        }

        internal bool Dispatch(Packet p)
        {
            var hl = TryGetHandlerList(p.msgRoute);
            if (hl == null || hl.Count == 0)
            {
                return false;
            }
            for (int i = 0; i < hl.Count; i++)
            {
                hl[i].Invoke(p);
            }
            return true;
        }
    }
}