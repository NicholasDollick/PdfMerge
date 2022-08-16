using System;
using System.IO;

namespace WPFUserInterface.Helpers
{
    public class Logger
    {
        public string LogFile { get; set; }

        public Logger(string logFIle)
        {
            LogFile = logFIle;
            File.Create(LogFile);
        }


        // these should check if the output path is null...it really should never be...but end users are silly 
        internal void Error(string message)
        {
            File.AppendAllText(LogFile, $"[{DateTime.Now}][ERROR]: {message}\n");
        }

        internal void Warning(string message)
        {
            File.AppendAllText(LogFile, $"[{DateTime.Now}][WARN]: {message}");
        }
    }
}
