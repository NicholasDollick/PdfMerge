using System;

namespace WPFUserInterface.Helpers
{
    public class Logger
    {
        public Logger()
        {
            // this would open the file to append to
        }


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
