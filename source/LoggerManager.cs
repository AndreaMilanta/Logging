using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Timers;

using Logging.Exceptions;
using Logging.Internal;

namespace Logging
{
    public class LoggerManager
    {
        private static LoggerManager _instance = null;

        public string FolderPath { get; private set; }
        public string Culture { get; private set; }
        public string DateFormat { get; private set; }
        public string NameFormat { get; private set; }
        public int TimerDt{ get; private set; }
        public bool ToConsole { get; private set; }
        public bool Enabled { get; private set; }
        public List<string> Logs { get => logDict.Keys.ToList(); }
        public static LoggerManager Instance { get => _instance == null ? throw new LogManagerAlreadyInitiliazedException() : _instance; }

        private Timer timer;

        private Dictionary<string, Logger> logDict;

        private LoggerManager(string folderPath, bool enabled ,bool toConsole, int dt, string nameFormate, string dateFormat, string culture)
        {
            //Set instance as singleton
            _instance = this;

            //Init culture
            CultureInfo.CurrentCulture = new CultureInfo(LogConstants.CULTURE);

            //Set properties
            this.FolderPath = folderPath;
            this.Culture = culture;
            this.DateFormat = dateFormat;
            this.NameFormat = nameFormate;
            this.TimerDt = dt;
            this.ToConsole = toConsole;
            this.Enabled = enabled;

            if (!Enabled)
                return;

            //Init folder
            if (!Directory.Exists(FolderPath.TrimEnd('\\')))
                Directory.CreateDirectory(FolderPath.TrimEnd('\\'));

            //Init Dictionary
            this.logDict = new Dictionary<string, Logger>();

            //Init LoggerLog
            string loggerLogPath = FolderPath + DateTime.Now.ToString(LogConstants.NAME_FORMAT) + LogConstants.NAME_DIVIDER + LogConstants.LOGGERLOG;
            this.logDict.Add(LogConstants.LOGGERLOG, new Logger(loggerLogPath, DateFormat));

            //Start Timer
            timer = new Timer(dt * 1000);
            timer.Elapsed += this.LogToFile;
            timer.Start();
        }

        public static LoggerManager SetupLoggerManager(string folderPath, bool enabled = true, bool toConsole = LogConstants.TO_CONSOLE,
                                                       int timerDt = LogConstants.TIMER_DT_s, string nameFormat = LogConstants.NAME_FORMAT,
                                                       string dateFormat = LogConstants.DATE_FORMAT, string culture = LogConstants.CULTURE)
        {
            if (_instance != null)
                throw new LogManagerAlreadyInitiliazedException();
            return new LoggerManager(folderPath, enabled, toConsole, timerDt, nameFormat, dateFormat, culture);
        }

        public void RegisterLogger(string logName, string dateFormat = null)
        {
            if (!Enabled)
                return;
            if (logDict.ContainsKey(logName))
                return;
            else
            {
                string fileName = FolderPath + DateTime.Now.ToString(NameFormat) + LogConstants.NAME_DIVIDER + logName + LogConstants.EXTENSION;
                logDict.Add(logName, new Logger(fileName, dateFormat ?? DateFormat));
            }
        }

        private void LogToFile(Object source, ElapsedEventArgs e)
        {
            foreach (Logger log in logDict.Values)
                log.ToFile();
        }

        public void Log(string log, string logEvent)
        {
            if (!Enabled)
                return;
            try
            {
                if (this.ToConsole)
                    Console.WriteLine(logEvent);
                logDict[log].Log(logEvent);
            }
            catch (KeyNotFoundException)
            {
                throw new LoggerNotFoundException(log);
            }
        }

        public void LogError(string log, string excEvent)
        {
            if (!Enabled)
                return;
            try
            {
                logDict[log].Log(excEvent);
                if (this.ToConsole)
                    Console.WriteLine(excEvent);
            }
            catch (KeyNotFoundException)
            {
                throw new LoggerNotFoundException(log);
            }
        }

        public void LogException(string log, Exception ex)
        {
            if (!Enabled)
                return;
            try
            {
                logDict[log].Log(ex.ToString());
                if (this.ToConsole)
                    Console.WriteLine(ex.ToString());
            }
            catch (KeyNotFoundException)
            {
                throw new LoggerNotFoundException(log);
            }
        }

        public void LoggingError(string orig, string logError, Exception e)
        {
            if (!Enabled)
                return;
            string logEntry = orig + " - " + logError + ". Error:  " + e.ToString();
            Log(LogConstants.LOGGERLOG, logEntry);
        }

        public void Close()
        {
            if (!Enabled)
                return;
            timer.Stop();
            foreach(Logger l in logDict.Values)
            {
                l.Log("Logger closed");
                l.Close();
            }
            logDict.Clear();
        }

        public void Close(string log)
        {
            try
            {
                logDict[log].Log("Logger closed");
                logDict[log].Close();
                logDict.Remove(log);
            }
            catch { }
        }
    }
}
