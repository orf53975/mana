using mana.Foundation.Test;

namespace mana.Server.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            var cr = new ConsoleRunning();
            GameServer.StartNew(Config.AppSetting);
            cr.StartUp(OnInputed);
        }

        static bool OnInputed(string cmd)
        {
            return false;
        }
    }
}
