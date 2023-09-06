using System.Collections.Generic;
using System.IO;

namespace RadinProjectNotesCommon
{
    /// <summary>
    /// Add log messages and save to a log file.
    /// </summary>
    public class Logger
    {
        private string _logFilePath;
        private List<string> _logEntries = new List<string>();

        /// <summary>
        /// Create a new logger to add messages and save to a log file.
        /// </summary>
        /// <param name="logFilePath">The full file path to the log file.</param>
        public Logger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        /// <summary>
        /// Add an entry to the log.
        /// </summary>
        /// <param name="logEntry"></param>
        public void AddEntry(string logEntry)
        {
            _logEntries.Add(logEntry);
        }

        /// <summary>
        /// Clear the log file.
        /// </summary>
        public void ClearLogFile()
        {
            _logEntries.Clear();
        }

        /// <summary>
        /// Save the log file. Can automatically create the folder.
        /// </summary>
        /// <param name="autoCreateFolder">Create the folder if it does not exist.</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public void SaveLogFile(bool autoCreateFolder = true)
        {
            // Check if the directory exists.
            string directory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(directory))
            {
                if (autoCreateFolder)
                {
                    Directory.CreateDirectory(directory);
                }
                else
                {
                    throw new DirectoryNotFoundException();
                }
            }

            using (var file = File.Open(_logFilePath, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(file))
            {
                foreach (var entry in _logEntries)
                {
                    writer.WriteLine(entry);
                }
            }
        }

    }
}
