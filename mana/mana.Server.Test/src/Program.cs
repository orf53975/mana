using mana.Foundation.Test;
using mana.Foundation;
using System.Diagnostics;
using System.Threading;

namespace mana.Server.Test
{
    class Program 
    {
        static void Main(string[] args)
        {
            var cr = new ConsoleRunning();
            MainServer.StartNew(Config.AppSetting);
            cr.StartUp(OnInputed);
            DoTest();
        }

        static bool OnInputed(string cmd)
        {
            return false;
        }


        static void DoTest()
        {
            var t1 = new Thread(() => ObjectCache.Put<AccountInfo>(new AccountInfo()));
            var t2 = new Thread(() => ObjectCache.Put<AccountInfo>(new AccountInfo()));
            var t3 = new Thread(() => ObjectCache.Put<AccountInfo>(new AccountInfo()));
            var t4 = new Thread(() => ObjectCache.Put<AccountInfo>(new AccountInfo()));
            var t5 = new Thread(() => ObjectCache.Put<AccountInfo>(new AccountInfo()));
            var t6 = new Thread(() => ObjectCache.Put<AccountInfo>(new AccountInfo()));
            var t7 = new Thread(() => ObjectCache.Put<AccountInfo>(new AccountInfo()));
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
        }
    }
}
