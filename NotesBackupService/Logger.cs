using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesBackupService
{
    public class Logger
    {
        static readonly string logFilename = @"backup.log";

        private static string LogFilePath
        {
            get
            {
                return Path.Combine(AppdataFolder(), logFilename);
            }
        }

        private static string LogSaveFileName
        {
            get
            {
                return LogFilePath + $"_{DateTime.Now.ToString("yyyymmddhhmmss")}";
            }
        }

        private static List<string> logEntries = new List<string>();

        public static void AddEntry(string logEntry)
        {
            logEntries.Add(logEntry);
        }

        public static void ResetLogFile()
        {
            logEntries.Clear();
        }

        public static void SaveLogFile()
        {
            using (var file = File.Open(LogSaveFileName, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(file))
            {
                foreach (var entry in logEntries)
                {
                    writer.WriteLine(entry);
                }
            }
        }

        public static string AppdataFolder()
        {
            string path = @"%appdata%\NotesBackupService";
            path = Environment.ExpandEnvironmentVariables(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

    }
}
