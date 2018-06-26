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
            Program.InitLogger();
            var setting = ConfigMgr.AppSetting;
            // -- 1
            LoadPlugins();
            // -- 2
            var sev = new MainServer(setting.connMax, setting.connBuffSize);
            var ipAddr = string.IsNullOrEmpty(setting.host) ? IPAddress.Any : IPAddress.Parse(setting.host);
            sev.Start(new IPEndPoint(ipAddr, setting.port));
            // -- 3
            StartCommandConsoleThread();
        }

        static void LoadPlugins()
        {
            var setting = ConfigMgr.AppSetting;
            foreach (var s in setting.plugins)
            {
                var fp = ConfigMgr.GetFullPath(s);
                var err = TypeUtil.LoadDll(AppDomain.CurrentDomain, fp);
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

        #region <<CommandConsole>>

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

        #endregion
    }
}
