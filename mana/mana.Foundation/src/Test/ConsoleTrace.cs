using System;
using System.Diagnostics;

namespace mana.Foundation.Test
{
    public sealed class ConsoleTrace : TraceListener
    {
        private void SetConsoleColor(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Information:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case TraceEventType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case TraceEventType.Critical:
                case TraceEventType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public override void Write(string message)
        {
            Console.Write(message);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            this.SetConsoleColor(eventType);
            base.TraceEvent(eventCache, source, eventType, id, format, args);
            this.SetConsoleColor(TraceEventType.Verbose);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            this.SetConsoleColor(eventType);
            base.TraceEvent(eventCache, source, eventType, id, message);
            this.SetConsoleColor(TraceEventType.Verbose);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            this.SetConsoleColor(eventType);
            base.TraceEvent(eventCache, source, eventType, id);
            this.SetConsoleColor(TraceEventType.Verbose);
        }
    }
}