using System;

namespace Logging
{
    public abstract class Loggable
    {
        private string _logger;

        public Loggable(string logger)
        {
            this._logger = logger;
            LoggerManager.Instance.RegisterLogger(this._logger);
        }

        /// <summary>
        /// Log Event
        /// </summary>
        /// <param name="logEvent"></param>
        protected void Log(string logEvent)
        {
            try
            {
                LoggerManager.Instance.Log(_logger, logEvent);
            }
            catch 
            {
                LoggerManager.Instance.LoggingError(_logger, logEvent);
            }
        }

        /// <summary>
        /// Log Error Event
        /// </summary>
        /// <param name="logEvent"></param>
        protected void LogException(string logEvent)
        {
            try
            {
                LoggerManager.Instance.LogError(_logger, logEvent);
            }
            catch 
            {
                LoggerManager.Instance.LoggingError(_logger, logEvent);
            }
        }

        /// <summary>
        /// Log Exception
        /// </summary>
        /// <param name="logEvent"></param>
        protected void LogError(Exception logExc)
        {
            try
            {
                LoggerManager.Instance.LogException(_logger, logExc);
            }
            catch 
            {
                LoggerManager.Instance.LoggingError(_logger, logExc.ToString());
            }
        }
    }
}
