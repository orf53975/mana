using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    internal class NetResponseDispatcher
    {
        private readonly Dictionary<int, INetRecivedCallBack> handlerDic = new Dictionary<int, INetRecivedCallBack>();

        public void Register<T>(int responseId, Action<T> handler)
            where T : class, DataObject, new()
        {
            var nrcb = new NetRecivedCallBack<T>(handler);
            handlerDic.Add(responseId, nrcb);
        }

        public void Register(int responseId, Proto proto, Action<DDNode> handler)
        {
            var nrcb = new NetRecivedCallBack(proto.s2cdt, handler);
            handlerDic.Add(responseId, nrcb);
        }

        public bool Dispatch(Packet p)
        {
            INetRecivedCallBack nrcb = null;
            if (handlerDic.TryGetValue(p.msgRequestId, out nrcb))
            {
                nrcb.Invoke(p);
                handlerDic.Remove(p.msgRequestId);
            }
            else
            {
                Logger.Warning("process response failed! [{0}]", p);
            }
            return false;
        }
    }
}