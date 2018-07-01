using mana.Foundation.Test;
using System.Net;

namespace mana.Server.Battle
{
    class Program : ConsoleProgram
    {
        static void Main(string[] args)
        {
            Instance.Start(args);
        }

        public static readonly Program Instance = new Program();

        public BattleServer Server { get; private set; }

        protected override void OnStarted(params string[] args)
        {
            var setting = Config.AppSetting;

            Server = new BattleServer(setting);

            var ipAddr = string.IsNullOrEmpty(setting.host) ? IPAddress.Any : IPAddress.Parse(setting.host);

            Server.Start(new IPEndPoint(ipAddr, setting.port));
        }

        protected override bool OnInputed(string cmd)
        {
            return base.OnInputed(cmd);
        }
    }
}
