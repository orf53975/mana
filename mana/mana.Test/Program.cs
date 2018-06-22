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
                DoTestNetClient();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }

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
