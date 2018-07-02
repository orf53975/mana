using mana.Foundation;
using mana.Foundation.Network.Sever;
using mana.Foundation.Test;
using System;

namespace mana.Server.Battle
{
    class Program
    {
        static void Main(string[] args)
        {
            TypeUtil.ParseArgs<ServerSetting>(args, opts => Config.AppSetting.UpdateWith(opts));
            Console.Title = string.Format("BattleServer-{0}:{1}", Config.AppSetting.host, Config.AppSetting.port);
            var cr = new ConsoleRunning();
            BattleServer.StartNew(Config.AppSetting);
            cr.StartUp(OnInputed);
        }

        static bool OnInputed(string cmd)
        {
            return false;
        }
    }
}
