using System;

namespace mana.Foundation.Network.Client
{
    public interface NetChannel
    {
        void StartConnect(string ip, ushort port, Action<bool, Exception> callback);
        void Send(Packet p);
        void AddListener(Action<Packet> p);
        void Disconnect();
    }
}