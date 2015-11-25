using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Archiver
{
    public static class CLog
    {
        private static TextWriter LogOut = Console.Out;
        private static CultureInfo _logCultureInfo = CultureInfo.InvariantCulture;

        public static void Init(TextWriter outputWriter)
        {
            CArguments.ThrowIfArgumentNull(outputWriter, "outputWriter");
            LogOut = outputWriter;
        }

        public static void Message(String message, params object[] args)
        {
            Message(ELogMessageTypes.Info, message, args);
        }

        public static void Error(String message, params object[] args)
        {
            Message(ELogMessageTypes.Error, message, args);
        }

        public static void Exception(Exception e, params object[] args)
        {
            Exception(e, String.Empty, args);
        }

        public static void Exception(Exception e, String message, params object[] args)
        {
            //Write main message
            if(!String.IsNullOrEmpty(message))
                Message(ELogMessageTypes.Exception, message, args);

            //Write exception type and message
            WriteLog(ELogMessageTypes.Exception, $"{e.GetType()}. {e.Message}");

            //Write stack trace
            PrintStackTrace(ELogMessageTypes.Exception, e.StackTrace);
        }

        public static void Warning(String message, params object[] args)
        {
            Message(ELogMessageTypes.Warning, message, args);
        }

        public static void Message(ELogMessageTypes messageType, String message, params object[] args)
        {
            CArguments.ThrowIfArgumentNull(message, "message");
            WriteLog(messageType, message, args);
        }

        public static void Flush()
        {
            LogOut.Flush();
        }

        private static void WriteLog(ELogMessageTypes messageType, string message, params object[] args)
        {
            String msg = FormatLogString(messageType, message, args);

            //lock (m_lock)
                LogOut.WriteLine(msg);
        }

        private static String FormatLogString(ELogMessageTypes messageType, string message, params object[] args)
        {
            return $"[{GetFormattedDateTimeNow()}]\t" +
                   $"{Thread.CurrentThread.ManagedThreadId}\t" +
                   $"{messageType, -15}\t" +
                   $"{message}. {FormatArgs(args)}";
        }

        private static String FormatArgs(params object[] args)
        {
            if (args == null || args.Length == 0)
                return String.Empty;

            StringBuilder builder = new StringBuilder();
            builder.Append("Params: ");

            var inArgs = args ?? new object[0];
            foreach (object arg in inArgs)
            {
                builder.Append(Convert.ToString(arg));
                builder.Append(", ");
            }
            builder.Remove(builder.Length - 2, 2);

            return builder.ToString();
        }

        private static String GetFormattedDateTimeNow()
        {
            return DateTime.UtcNow.ToString(_logCultureInfo);
        }

        private static void PrintStackTrace(ELogMessageTypes messageType, String stackTrace)
        {
            CArguments.ThrowIfArgumentNull(stackTrace, "stackTrace");
            foreach (string stackString in stackTrace.Split('\n'))
                Message(messageType, stackString.TrimEnd('\r'));
        }
    }
}
