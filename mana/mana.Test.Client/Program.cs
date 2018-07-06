using mana.Foundation.Test;
using mana.Foundation;
using System;
using System.Collections.Generic;
using System.Threading;

namespace mana.Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var cr = new ConsoleRunning();
            cr.StartUp(OnInputed);
            new Thread(() =>
            {
                while (cr.IsRunning)
                {
                    UpdateClients(20);
                    Thread.Sleep(20);
                }
            }).Start();
        }

        static bool OnInputed(string cmd)
        {
            if(cmd.StartsWith("nc"))
            {
                var uid = uidGen ++;
                var ntc = new TestClient(uid, "127.0.0.1", 8081);
                Console.WriteLine("new TestClient [{0}]!", uid);

                lock (clients)
                {
                    ForeachClient(tc => tc.Channel.Disconnect());
                    clients.Clear();
                    clients.Add(uid, ntc);
                }

                ntc.StartConnect();
                return true;
            }
            if (cmd.StartsWith("ping"))
            {
                ForeachClient(tc => tc.Channel.SendPingPacket());
                return true;
            }
            if (cmd.StartsWith("test"))
            {
                ForeachClient(tc => tc.DoTest());
                return true;
            }
            return false;
        }

        static readonly Dictionary<int, TestClient> clients = new Dictionary<int, TestClient>();
        static int uidGen = 0;

        static void ForeachClient(Action<TestClient> action)
        {
            lock (clients)
            {
                foreach (var kv in clients)
                {
                    action(kv.Value);
                }
            }
        }

        static void UpdateClients(int deltaTimeMs)
        {
            lock (clients)
            {
                foreach (var kv in clients)
                {
                    kv.Value.Channel.Update(deltaTimeMs);
                }
            }
        }
    }
}
