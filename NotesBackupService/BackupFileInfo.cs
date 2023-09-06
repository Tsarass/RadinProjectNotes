using RadinProjectNotesCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NotesBackupService
{
    [Serializable]
    public partial class BackupFileInfo
    {
        public static readonly string fileInfoFilename = @"backup_file_info.dat";

        private List<BackupFile> _files = new List<BackupFile>();
        private string _appDataFolder;
        private Logger _logger;
        private int _maxRevisions;

        public BackupFileInfo(string appDataFolder, int maxRevisions, Logger logger)
        {
            _appDataFolder = appDataFolder;
            _logger = logger;
            _maxRevisions = maxRevisions;
        }

        private string BackupFileInfoFilePath
        {
            get
            {
                return Path.Combine(_appDataFolder, fileInfoFilename);
            }
        }

        public void LoadFromDisk()
        {
            if (File.Exists(BackupFileInfoFilePath))
            {
                try
                {
                    using (var fs = new FileStream(BackupFileInfoFilePath, FileMode.Open, FileAccess.Read))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        // This is where you deserialize the class
                        BackupFileInfo data = (BackupFileInfo)formatter.Deserialize(fs);
                        this._files = data._files;
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(this, e.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("Error while loading backup file info database", e);
                    throw;
                }
            }

        }

        public void SaveToDisk()
        {
            try
            {
                using (var fs = new FileStream(BackupFileInfoFilePath, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    
                    formatter.Serialize(fs, this);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(this, e.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Exception caught while saving backup file info database", e);
                throw;
            }
        }

        /// <summary>
        /// Add a new file to the backup file info list.
        /// </summary>
        /// <param name="fileName">the full file path relative to the backup folder</param>
        public void AddFile(string filePath)
        {
            BackupFile matchingFile = FindMatchingFile(filePath);
            if (matchingFile is null)
            {
                _files.Add(new BackupFile(filePath, _maxRevisions, _logger));
            }
            else
            {
                matchingFile.UpdateFile();
            }
        }

        public void AddFileRevision(string filePath)
        {
            BackupFile matchingFile = FindMatchingFile(filePath);
            if (matchingFile is null)
            {
                AddFile(filePath);
                return;
            }

            matchingFile.AddRevision();
        }

        private BackupFile FindMatchingFile(string filePath)
        {
            foreach (var file in _files)
            {
                if (file.FilePath == filePath)
                {
                    return file;
                }
            }
            return null;
        }

        public void SkipFile(string filePath)
        {
            BackupFile matchingFile = FindMatchingFile(filePath);
            if (matchingFile is null)
            {
                return;
            }

            matchingFile.UpdateFile();
        }

        public void DeleteObsoleteFiles()
        {
            for (int i = _files.Count - 1; i >= 0; i--)
            {
                BackupFile file = _files[i];
                //find the time elapsed since last file update
                TimeSpan timeElapsed = DateTime.Now.Subtract(file.TimeLastUpdated);
                if (timeElapsed.TotalDays > 30) //remove obsolete files after 30 days
                {
                    file.Delete();
                    _files.RemoveAt(i);
                }
            }
        }
    }
}
