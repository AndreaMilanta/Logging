using System;

using Logging.Exceptions;

namespace Logging
{
    public abstract class Loggable
    {
        private string _logger;

        protected Loggable(string logger)
        {
            this._logger = logger.ToLower();
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
            catch (Exception e)
            {
                LoggerManager.Instance.LoggingError(_logger, logEvent, e);
            }
        }

        /// <summary>
        /// Log Error Event
        /// </summary>
        /// <param name="logEvent"></param>
        protected void LogError(string logEvent)
        {
            try
            {
                LoggerManager.Instance.LogError(_logger, logEvent);
            }
            catch (Exception e)
            {
                LoggerManager.Instance.LoggingError(_logger, logEvent, e);
            }
        }

        /// <summary>
        /// Log Exception
        /// </summary>
        /// <param name="logEvent"></param>
        protected void LogException(Exception logExc)
        {
            try
            {
                LoggerManager.Instance.LogException(_logger, logExc);
            }
            catch (Exception e)
            {
                LoggerManager.Instance.LoggingError(_logger, logExc.ToString(), e);
            }
        }
    }
}
