using mana.Foundation;
using System;

namespace mana.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Start Test!");
                Program.InitLogger();
                DoTestNetClient();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }

        #region InitLogger

        static void InitLogger()
        {
            Logger.SetPrintHandler((str) =>
            {
                var saveColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(str);
                Console.ForegroundColor = saveColor;
            });

            Logger.SetWarningHandler((str) =>
            {
                var saveColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(str);
                Console.ForegroundColor = saveColor;
            });

            Logger.SetErrorHandler((str) =>
            {
                var saveColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(str);
                Console.ForegroundColor = saveColor;
            });

            Logger.SetExceptionHandler((e) =>
            {
                var saveColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ForegroundColor = saveColor;
            });

        }

        #endregion

        #region NetClient

        public static void DoTestNetClient()
        {

            var nc = new NetClient(new TestNetChannel());
            //Profiler.BeginSample("-------------------------");
            nc.AddPushListener("battle.sync", (dn) => { });
            //Profiler.EndSample();
        }

        #endregion
    }
}
