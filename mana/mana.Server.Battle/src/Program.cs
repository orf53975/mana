﻿using mana.Foundation;
using mana.Foundation.Network.Sever;
using mana.Foundation.Test;
using System;

namespace mana.Server.Battle
{
    class Program
    {
        static void Main(string[] args)
        {
            TypeUtil.ParseArgs<ServerSetting>(args, opts => Config.SevSetting.UpdateWith(opts));
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
