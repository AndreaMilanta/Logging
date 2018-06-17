using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class LoggerTool : Loggable
    {
        public  LoggerTool(string logger) : base(logger)
        {
        }

        // public override
        public new void Log(string logEvent)
        {
            base.Log(logEvent);
        }

        public new void LogError(string logEvent)
        {
            base.LogError(logEvent);
        }
        public new void LogException(Exception logExc)
        {
            base.LogException(logExc);
        }
    }
}
