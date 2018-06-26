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
            Program.InitLogger();
            Program.StartCommandConsoleThread();
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

        static void OnCommond(string cmd)
        {
            if (cmd == "exit" || cmd == "quit")
            {
                isCommandConsoleRunning = false;
                return;
            }
            if (cmd == "nc")
            {
                if (mTestClient == null)
                {
                    mTestClient = StartNetClient();
                    mTestClient.AddPushListener<Protocol>("Connector.Protocol", (protocol) =>
                    {
                        Protocol.Instance.Push(protocol);
                        Logger.Print(Protocol.Instance.ToFormatString(""));
                    });
                    mTestClient.AddPushListener<Heartbeat>("Connector.Pong", (protocol) =>
                    {
                        Logger.Print(protocol.ToFormatString(""));
                    });
                    mTestClient.Connect("127.0.0.1", 8088, null);
                }
                else
                {
                    Console.WriteLine("TestClient had already existed!");
                }
                return;
            }
            if (cmd == "ping")
            {
                if (mTestClient != null)
                {
                    mTestClient.Notify<Heartbeat>("Connector.Ping", (hb) =>
                    {
                        TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
                        hb.timestamp = (int)ts.TotalSeconds;
                    });
                }
                return;
            }
            Console.WriteLine("invalid command:{0}", cmd);
        }

        #endregion

        private static NetClient mTestClient = null;
        public static NetClient StartNetClient()
        {
            var channel = new CustomNetChannel();
            channel.StartThread();
            return new NetClient(channel);
        }

    }
}
