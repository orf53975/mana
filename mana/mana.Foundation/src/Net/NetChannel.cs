using System;

namespace mana.Foundation
{
    public interface NetChannel
    {
        void StartConnect(string ip, ushort port, Action<bool, Exception> callback);
        void Send(Packet p);
        void AddListener(Action<Packet> p);
    }
}