using mana.Foundation;
using mana.Foundation.Network.Client;
using System;

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

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

    }
}
