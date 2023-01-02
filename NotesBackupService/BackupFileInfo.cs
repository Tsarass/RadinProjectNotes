using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NotesBackupService
{
    [Serializable]
    public partial class BackupFileInfo
    {
        public static readonly string fileInfoFilename = @"backup_file_info.dat";

        List<BackupFile> files = new List<BackupFile>();

        private string BackupFileInfoFilePath
        {
            get
            {
                return Path.Combine(Logger.AppdataFolder(), fileInfoFilename);
            }
        }

        public BackupFileInfo()
        {
            
        }

        public void LoadFromDisk()
        {
            string filePath = Path.Combine(Logger.AppdataFolder(),fileInfoFilename);
            if (File.Exists(filePath))
            {
                try
                {
                    using (var fs = new FileStream(BackupFileInfoFilePath, FileMode.Open, FileAccess.Read))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        // This is where you deserialize the class
                        BackupFileInfo data = (BackupFileInfo)formatter.Deserialize(fs);
                        this.files = data.files;
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
                files.Add(new BackupFile(filePath));
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
            foreach (var file in files)
            {
                if (file.filePath == filePath)
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
            for (int i = files.Count - 1; i >= 0; i--)
            {
                BackupFile file = files[i];
                //find the time elapsed since last file update
                TimeSpan timeElapsed = DateTime.Now.Subtract(file.timeLastUpdated);
                if (timeElapsed.TotalDays > 30) //remove obsolete files after 30 days
                {
                    file.Delete();
                    files.RemoveAt(i);
                }
            }
        }
    }
}
