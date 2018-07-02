using System;
using System.Diagnostics;
using System.Threading;

namespace mana.Foundation.Test
{
    public class ConsoleRunning
    {
        public bool IsRunning { get; private set; }

        public Func<string, bool> OnInputed;

        public ConsoleRunning()
        {
            this.IsRunning = true;
            Trace.Listeners.Add(new ConsoleTrace());
            this.InitLogger();
        }

        public void StartUp(Func<string, bool> inputCallback = null)
        {
            this.OnInputed = inputCallback;
            this.StartInputThread();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        void InitLogger()
        {
            Logger.SetPrintHandler((str) => Trace.TraceInformation(str));
            Logger.SetWarningHandler((str) => Trace.TraceWarning(str));
            Logger.SetErrorHandler((str) => Trace.TraceError(str));
            Logger.SetExceptionHandler((ex) => Trace.TraceError(ex.ToString()));
        }

        void StartInputThread() => new Thread(() =>
        {
            Trace.TraceInformation("-- Start Console --");
            while (IsRunning)
            {
                try
                {
                    var cmd = Console.ReadLine().Trim();
                    if (cmd == "exit" || cmd == "quit")
                    {
                        IsRunning = false;
                    }
                    else if (cmd == "clear")
                    {
                        Console.Clear();
                    }
                    else if (OnInputed != null && !OnInputed(cmd))
                    {
                        Console.WriteLine("Unrecognized Command[{0}]!", cmd);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }).Start();
    }
}