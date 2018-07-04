using mana.Foundation.Test;
using mana.Foundation;
using System;
using System.Collections.Generic;

namespace mana.Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var cr = new ConsoleRunning();
            cr.StartUp(OnInputed);
        }

        static readonly Dictionary<int, TestClient> clients = new Dictionary<int, TestClient>();

        static int uidGen = 0;

        static bool OnInputed(string cmd)
        {
            if(cmd.StartsWith("nc"))
            {
                var uid = uidGen ++;
                var ntc = new TestClient(uid, "127.0.0.1", 8081);
                clients.Add(uid, ntc);
                Console.WriteLine("new TestClient [{0}]!", uid);
                ntc.StartConnect();
                return true;
            }
            if (cmd.StartsWith("ping"))
            {
                var uid = uidGen++;
                foreach (var kv in clients)
                {
                    kv.Value.Channel.SendPingPacket();
                }
                return true;
            }
            if (cmd.StartsWith("test"))
            {
                var uid = uidGen++;
                foreach (var kv in clients)
                {
                    kv.Value.DoTest();
                }
                return true;
            }
            return false;
        }
    }
}
