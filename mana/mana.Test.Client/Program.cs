using mana.Foundation.Test;
using System;
using System.Collections.Generic;

namespace mana.Test.Client
{
    class Program : ConsoleProgram
    {
        #region <<STATIC MAIN>>
        static void Main(string[] args)
        {
            new Program().Start();
        }
        #endregion

        private readonly Dictionary<int, TestClient> clients = new Dictionary<int, TestClient>();

        private int uidGen = 0;

        protected override bool OnInputed(string cmd)
        {
            if (base.OnInputed(cmd))
            {
                return true;
            }
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
