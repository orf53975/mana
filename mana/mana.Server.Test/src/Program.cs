using mana.Foundation.Test;

namespace mana.Server.Test
{
    class Program 
    {
        static void Main(string[] args)
        {
            var cr = new ConsoleRunning();
            MainServer.StartNew(Config.AppSetting);
            cr.StartUp(OnInputed);
        }

        static bool OnInputed(string cmd)
        {
            return false;
        }
    }
}
