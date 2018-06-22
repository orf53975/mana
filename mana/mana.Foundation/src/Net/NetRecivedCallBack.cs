using System;

namespace mana.Foundation
{
    public interface INetRecivedCallBack
    {
        void Invoke(Packet p);
    }

    public struct NetRecivedCallBack : INetRecivedCallBack
    {
        readonly Action<DDNode> callback;

        readonly string nodeTmplName;

        public NetRecivedCallBack(string tmpl, Action<DDNode> cb)
        {
            this.nodeTmplName = tmpl;
            this.callback = cb;
        }

        public void Invoke(Packet p)
        {
            callback(p.TryGetDataNode(nodeTmplName));
        }
    }

    public struct NetRecivedCallBack<T> : INetRecivedCallBack
        where T : class, DataObject, new()
    {
        readonly Action<T> callback;

        public NetRecivedCallBack(Action<T> cb)
        {
            this.callback = cb;
        }

        public void Invoke(Packet p)
        {
            callback(p.TryGet<T>());
        }
    }
}
