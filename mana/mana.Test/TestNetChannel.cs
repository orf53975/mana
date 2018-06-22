using System;
using mana.Foundation;

namespace mana.Test
{
    class TestNetChannel : NetChannel
    {
        public void AddListener(Action<Packet> p)
        {

        }

        public void Send(Packet p)
        {

        }

        public void StartConnect(string ip, ushort port, Action<bool, Exception> callback)
        {

        }
    }
}
