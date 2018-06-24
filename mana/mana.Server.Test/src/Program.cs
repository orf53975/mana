using mana.Foundation;
using mana.Server.Test.Config;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace mana.Server.Test
{
    class Program
    {

        static void Main(string[] args)
        {
            Trace.Listeners.Add(new CustomTrace());

            var setting = ConfigManager.AppSetting;
            // -- 1
            LoadBattleSystemPlugins();
            // -- 2
            var sev = new MainServer(setting.connMax, setting.connBuffSize);
            var ipAddr = string.IsNullOrEmpty(setting.host) ? IPAddress.Any : IPAddress.Parse(setting.host);
            sev.Start(new IPEndPoint(ipAddr, setting.port));
            // -- 3
            StartCommandConsoleThread();
        }

        static void LoadBattleSystemPlugins()
        {
            var setting = ConfigManager.AppSetting;
            foreach (var s in setting.plugins)
            {
                var fp = ConfigManager.GetFullPath(s);
                var err = Utils.LoadDll(AppDomain.CurrentDomain, fp);
                if (err == null)
                {
                    Trace.TraceInformation("dll loaded > " + s);
                }
                else
                {
                    Trace.TraceError(err);
                }
            }
        }

        static bool isCommandConsoleRunning = false;
        static void StartCommandConsoleThread()
        {
            isCommandConsoleRunning = true;
            new Thread(() =>
            {
                Trace.TraceInformation("----- Start Console Command -----");
                while (isCommandConsoleRunning)
                {
                    var cmd = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(cmd))
                    {
                        try
                        {
                            OnCommond(cmd);
                        }
                        catch (Exception e)
                        {
                            Trace.TraceError(e.ToString());
                        }
                    }
                    Thread.Sleep(100);
                }
            }).Start();
        }

        // 控制台逻辑
        static void OnCommond(string cmd)
        {
            if (cmd == "exit" || cmd == "quit")
            {
                isCommandConsoleRunning = false;
                return;
            }
            //if (cmd == "attack")
            //{
            //    var ab = BattleSystem.Units.Abilities.AbilityFactory.Creat(0x2001, 100);
            //    Trace.TraceError(ab.name);
            //    return;
            //}
            //if (cmd == "buff")
            //{
            //    var bf = BattleSystem.Units.Buffs.BuffFactory.Creat(0x0001, 1);
            //    Trace.TraceError(bf.name);
            //    return;
            //}
        }
    }
}
