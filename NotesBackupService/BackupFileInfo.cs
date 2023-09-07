using ProtoBuf;
using RadinProjectNotesCommon;
using RadinProjectNotesCommon.EncryptedDatabaseSerializer;
using System;
using System.Collections.Generic;
using System.IO;

namespace NotesBackupService
{
    [ProtoContract]
    public partial class BackupFileInfo
    {
        [ProtoIgnore]
        public static readonly string fileInfoFilename = @"backup_file_info.dat";
        [ProtoIgnore]
        private static Logger _logger;

        [ProtoMember(1)]
        private List<BackupFile> _files = new List<BackupFile>();
        [ProtoIgnore]
        private string _appDataFolder;
        [ProtoIgnore]
        private int _maxRevisions;

        public BackupFileInfo()
        {
            //parameterless constructor for protobuf
        }

        public BackupFileInfo(string appDataFolder, int maxRevisions, Logger logger)
        {
            _appDataFolder = appDataFolder;
            _logger = logger;
            _maxRevisions = maxRevisions;
        }

        [ProtoIgnore]
        private string BackupFileInfoFilePath
        {
            get
            {
                return Path.Combine(_appDataFolder, fileInfoFilename);
            }
        }

        public void LoadFromDisk()
        {
            EncryptedDatabaseProtobufSerializer<BackupFileInfo> encryptedDbSerializer =
                new EncryptedDatabaseProtobufSerializer<BackupFileInfo>(BackupFileInfoFilePath);

            try
            {
                BackupFileInfo loadedInfo = encryptedDbSerializer.TryLoadDatabase();
                _files = loadedInfo._files;
            }
            catch (CouldNotLoadDatabase)
            {
                throw;
            }
            catch (DatabaseFileNotFound)
            {
                // If the due items file does not exist, create an empty database.
                _files = new List<BackupFile>();
            }
        }

        public void SaveToDisk()
        {
            EncryptedDatabaseProtobufSerializer<BackupFileInfo> encryptedDbSerializer =
                new EncryptedDatabaseProtobufSerializer<BackupFileInfo>(BackupFileInfoFilePath, hideDatabaseFile: false);

            try
            {
                encryptedDbSerializer.TrySaveDatabase(this);
            }
            catch (CouldNotSaveDatabase)
            {
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
                _files.Add(new BackupFile(filePath, _maxRevisions));
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
