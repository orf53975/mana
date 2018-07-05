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
            TypeUtil.ParseArgsUpdateObject(Config.SevSetting, args, (argv) =>
            {
                Console.WriteLine("parse[{0}] failed!", argv);
            });
            Console.Title = string.Format("BattleServer-{0}", Config.SevSetting.port);
            var cr = new ConsoleRunning();
            BattleServer.StartNew(Config.SevSetting);
            cr.StartUp(OnInputed);
        }

        static bool OnInputed(string cmd)
        {
            return false;
        }
    }
}
