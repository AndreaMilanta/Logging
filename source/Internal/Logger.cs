using System;
using System.IO;
using System.Text;

namespace Logging.Internal
{
    internal class Logger
    {
        private StreamWriter _sw;
        private StringBuilder _content;
        private string _dateFormat;

        public Logger(string logFilePath, string dateformat)
        {
            if (logFilePath.Contains(LogConstants.EXTENSION))
                _sw = new StreamWriter(logFilePath);
            else
                _sw = new StreamWriter(logFilePath + LogConstants.EXTENSION);
            _content = new StringBuilder();
            _dateFormat = dateformat;
        }

        /// <summary>
        /// Log Event
        /// </summary>
        /// <param name="logEvent"></param>
        /// <param name="dateFormat"></param>
        public void Log(string logEvent)
        {
            string logEntry = DateTime.Now.ToString(_dateFormat) + LogConstants.DIVIDER + logEvent;
            lock (this._content)
                _content.AppendLine(logEvent);
        }

        /// <summary>
        /// Log Exception
        /// </summary>
        /// <param name="excEvent"></param>
        /// <param name="dateFormat"></param>
        public void LogException(string excEvent)
        {
            string logEntry = DateTime.Now.ToString(_dateFormat) + LogConstants.DIVIDER + 
                              LogConstants.EX_HIGHLIGHT_PRE + excEvent + LogConstants.EX_HIGHLIGHT_POST;
            lock (this._content)
                _content.AppendLine(logEntry);
        }


        public void ToFile()
        {
            lock (this._content)
            {
                _sw.Write(_content.ToString());
                _sw.FlushAsync();
                _content.Clear();
            }
        }

        public void Close()
        {
            this.ToFile();
            _sw.Close();
        }
    }
}
