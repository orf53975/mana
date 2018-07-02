using mana.Foundation.Test;

namespace mana.Server.Battle
{
    class Program
    {
        static void Main(string[] args)
        {
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
