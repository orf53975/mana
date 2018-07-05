using mana.Foundation;
using mana.Foundation.Test;
using System;

namespace mana.Server.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            TypeUtil.ParseArgsUpdateObject(Config.ServerSetting, args, (argv) =>
            {
                Console.WriteLine("parse[{0}] failed!", argv);
            });
            var cr = new ConsoleRunning();
            GameServer.StartNew(Config.ServerSetting);
            cr.StartUp(OnInputed);
        }

        static bool OnInputed(string cmd)
        {
            return false;
        }
    }
}
