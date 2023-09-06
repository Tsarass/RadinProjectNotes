using System;
using System.IO;

namespace RadinProjectNotesCommon
{
    /// <summary>
    /// Common file paths for Project Notes.
    /// </summary>
    public static class Filepaths
    {
        /// <summary>
        /// Get the app data folder for Project Notes. If the directory does not exist, create it.
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataFolder()
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
