using System;

namespace WPFUserInterface.Helpers
{
    public class Logger
    {
        public string LogFile { get; set; }

        public Logger(string logFile)
        {
            LogFile = logFile;
        }


        // these should check if the output path is null...it really should never be...but end users are silly 
        internal void Error(string message)
        {
            Console.WriteLine($"[{DateTime.Now}][ERROR]: {message}");
        }

        internal void Warning(string message)
        {
            Console.WriteLine($"[{DateTime.Now}][WARN]: {message}");
        }
    }
}
