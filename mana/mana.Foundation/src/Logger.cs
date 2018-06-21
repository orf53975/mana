using System;

namespace mana.Foundation
{
    public static class Logger
    {
        private static Action<string> OnPrint;
        public static void SetPrintHandler(Action<string> handler)
        {
            OnPrint = handler;
        }

        private static Action<string> OnWarning;
        public static void SetWarningHandler(Action<string> handler)
        {
            OnWarning = handler;
        }

        private static Action<string> OnError;
        public static void SetErrorHandler(Action<string> handler)
        {
            OnError = handler;
        }

        private static Action<Exception> OnException;
        public static void SetExceptionHandler(Action<Exception> handler)
        {
            OnException = handler;
        }

        public static void Print(string str, params object[] args)
        {
            if (OnPrint == null || str == null)
            {
                return;
            }
            if (args.Length > 0)
            {
                str = string.Format(str, args);
            }
            OnPrint.Invoke(str);
        }

        public static void Warning(string str, params object[] args)
        {
            if (OnWarning == null || str == null)
            {
                return;
            }
            if (args.Length > 0)
            {
                str = string.Format(str, args);
            }
            OnWarning.Invoke(str);
        }

        public static void Error(string str, params object[] args)
        {
            if (OnError == null || str == null)
            {
                return;
            }
            if (args.Length > 0)
            {
                str = string.Format(str, args);
            }
            OnError.Invoke(str);
        }

        public static void Exception(Exception e)
        {
            if (OnException != null) OnException.Invoke(e);
        }
    }
}