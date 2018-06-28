using mana.Foundation;
using mana.Foundation.Network.Client;
using System;
using System.Diagnostics;
using System.Threading;

namespace mana.Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new CustomTrace());
            Packet.ChangePoolCapacity(1024);
            Program.InitLogger();
            Program.StartCommandConsoleThread();
        }

        #region InitLogger

        static void InitLogger()
        {
            Logger.SetPrintHandler((str) =>
            {
                Trace.TraceInformation(str);
            });

            Logger.SetWarningHandler((str) =>
            {
                Trace.TraceWarning(str);
            });

            Logger.SetErrorHandler((str) =>
            {
                Trace.TraceError(str);
            });

            Logger.SetExceptionHandler((e) =>
            {
                Trace.TraceError(e.ToString());
            });

        }

        #endregion

        #region <<CommandConsoleThread>>
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
                    if (!string.IsNullOrEmpty(cmd))
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
        #endregion

        #region <<OnCommond>>

        static TestClient mTestClient = null;
        static void OnCommond(string cmd)
        {
            switch (cmd)
            {
                case "nc":
                    if (mTestClient == null)
                    {
                        mTestClient = new TestClient(Environment.TickCount , "127.0.0.1", 8088);
                    }
                    else
                    {
                        Console.WriteLine("TestClient had already existed!");
                    }
                    return;
                case "ping":
                    if(mTestClient != null)
                    {
                        mTestClient.mNetChannel.SendPingPacket();
                        return;
                    }
                    break;
            }
            Console.WriteLine("invalid command:{0}", cmd);
        }
        #endregion
    }
}
