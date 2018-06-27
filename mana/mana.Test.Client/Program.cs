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
                    mTestClient.SendPingPacket();
                }
                return;
            }
            Console.WriteLine("invalid command:{0}", cmd);
        }

        #endregion

        private static AbstractNetClient mTestClient = null;

        public static AbstractNetClient StartNetClient()
        {
            var channel = new CustomNetClient();
            channel.StartThread();
            return channel;
        }
    }
}
