using System;
using System.Diagnostics;
using System.Threading;

namespace mana.Foundation.Test
{
    public abstract class ConsoleProgram
    {
        public bool IsRunning { get; private set; }

        public ConsoleProgram()
        {
            this.IsRunning = true;
            Trace.Listeners.Add(new ConsoleTrace());
            this.InitLogger();
        }

        public void Start(params string[] args)
        {
            this.StartInputThread();
            this.OnStarted(args);
        }

        protected virtual void OnStarted(params string[] args) { }

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
                    var command = Console.ReadLine().Trim();
                    if (!OnInputed(command))
                    {
                        Console.WriteLine("Unrecognized Command[{0}]!" , command);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }).Start();

        protected virtual bool OnInputed(string cmd)
        {
            if (cmd == "exit" || cmd == "quit")
            {
                IsRunning = false;
                return true;
            }
            if(cmd == "clear")
            {
                Console.Clear();
                return true;
            }
            return false;
        }
    }
}