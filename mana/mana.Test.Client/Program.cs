using mana.Foundation.Test;
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
                var ntc = new TestClient(uid, "127.0.0.1", 8088);
                clients.Add(uid, ntc);
                Console.WriteLine("new TestClient [{0}]!", uid);
                return true;
            }
            return false;
        }
    }
}
