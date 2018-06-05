using System;

namespace Logging.Exceptions
{
    public class LoggerNotFoundException : Exception
    {
        private string _logName = null;
        public LoggerNotFoundException(string name)
        {
            _logName = name;
        }

        public override string ToString()
        {
            return "Logger " + _logName + " has not been registered yet";
        }
    }
}
