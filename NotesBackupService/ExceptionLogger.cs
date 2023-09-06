using RadinProjectNotesCommon;
using System;
using System.IO;

namespace NotesBackupService
{
    /// <summary>
    /// Log exceptions.
    /// </summary>
    public class ExceptionLogger
    {
        private Exception _ex;
        private string _appDataFolder;

        private readonly string exceptionRecordFolder = @"exceptions";

        private string ExceptionLogDirectory
        {
            get
            {
                return Path.Combine(_appDataFolder, exceptionRecordFolder);
            }
        }

        /// <summary>
        /// Log an exception to a file.
        /// </summary>
        /// <param name="ex"></param>
        public ExceptionLogger(Exception ex, string appDataFolder)
        {
            _ex = ex;
        }

        /// <summary>
        /// Save the exception info to a log file.
        /// </summary>
        public void SaveExceptionLogFile()
        {
            string logFileNameWithDate = $"exception_{DateTime.Now.ToString("yyyymmddhhmmss")}.log";
            string logFilePath = Path.Combine(ExceptionLogDirectory, logFileNameWithDate);
            Logger exceptionLogger = new Logger(logFilePath);

            // Log the exception.
            exceptionLogger.AddEntry($"Date: {DateTime.Now.ToString()}");
            exceptionLogger.AddEntry($"Message: {_ex.Message}");
            exceptionLogger.AddEntry($"Source: {_ex.Source}");
            exceptionLogger.AddEntry($"Inner exception: {_ex.InnerException}");
            exceptionLogger.AddEntry($"Stacktrace: {_ex.StackTrace}");

            exceptionLogger.SaveLogFile(autoCreateFolder: true);
        }
    }
}
