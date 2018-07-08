using System;
using System.Collections.Generic;

namespace mana.Foundation.Network.Client
{
    internal class NetPacketDispatcher
    {
        readonly Dictionary<string, List<Action<Packet>>> pushHandlerDic = new Dictionary<string, List<Action<Packet>>>();

        private List<Action<Packet>> TryGetHandlerList(string route)
        {
            List<Action<Packet>> hl;
            if (pushHandlerDic.TryGetValue(route, out hl))
            {
                return hl;
            }
            return null;
        }

        internal void Register(string route, Action<Packet> handler)
        {
            var hl = TryGetHandlerList(route);
            if (hl == null)
            {
                hl = new List<Action<Packet>>();
                pushHandlerDic.Add(route, hl);
            }
            if (hl.Contains(handler))
            {
                Logger.Error("Register Duplicate!");
            }
            else
            {
                hl.Add(handler);
            }
        }

        internal void Unregister(string route, Action<Packet> handler)
        {
            var hl = TryGetHandlerList(route);
            if (hl != null)
            {
                if (!hl.Remove(handler))
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