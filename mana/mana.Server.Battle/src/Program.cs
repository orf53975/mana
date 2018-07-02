using mana.Foundation;
using mana.Foundation.Test;
using System;

namespace mana.Server.Battle
{
    class Options
    {
        public ushort port;
        public string host;
    }

    class Program
    {
        static void Main(string[] args)
        {
            TypeUtil.ParseArgs<Options>(args, opts =>
            {
                if (opts.port != 0)
                {
                    Config.AppSetting.port = opts.port;
                }
                if (opts.host != null)
                {
                    Config.AppSetting.host = opts.host;
                }
            },
            p =>
            {
                Console.WriteLine("unrecognized:{0}", p);
            }
            );
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
