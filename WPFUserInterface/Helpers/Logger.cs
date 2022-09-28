using System;
using System.IO;

namespace WPFUserInterface.Helpers
{
    public class Logger
    {
        private string LogFile { get; set; }

        public Logger(string logFile)
        {
            LogFile = logFile;
            File.Create(LogFile);
        }


        // these should check if the output path is null...it really should never be...but end users are silly 
        internal void Error(string message)
        {
            File.AppendAllText(LogFile, $"[{DateTime.Now}][ERROR]: {message}\n");
        }

        internal void Info(string message)
        {
            File.AppendAllText(LogFile, $"[{DateTime.Now}][INFO]: {message}\n");
        }

        internal void Warning(string message)
        {
            File.AppendAllText(LogFile, $"[{DateTime.Now}][WARN]: {message}");
        }
    }
}
