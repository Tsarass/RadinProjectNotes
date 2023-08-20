using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace NotesBackupService
{
    public class FileBackup
    {
        static readonly string baseDirectory = @"\\nas-radin-lp\DATEN\notes";
        static readonly string backupDirectory = @"\\nas-radin-gr\FSERVER\Z_NotesBackup";

        static BackupFileInfo backupFileInfo = new BackupFileInfo();

        public FileBackup()
        {
            Logger.ResetLogFile();

            //if backup directory doesnt exist, create it
            DirectoryInfo di = new DirectoryInfo(backupDirectory);
            if (!di.Exists)
            {
                di.Create();
            }
            di.Attributes |= FileAttributes.Hidden;
        }

        public void Start()
        {
            Logger.AddEntry($"Starting backup at {DateTime.Now.ToString()}");

            backupFileInfo.LoadFromDisk();

            List<string> filesToBackup = ListAllFilesUnderDirectory(baseDirectory);

            Logger.AddEntry($"Found {filesToBackup.Count} files to backup");
            CopyAllFiles(filesToBackup);

            Logger.AddEntry(@"Checking for obsolete files in backup directory..");
            DeleteObsoleteFiles(filesToBackup);

            Logger.AddEntry($"Backup finished at {DateTime.Now.ToString()}");
            Logger.SaveLogFile();

            backupFileInfo.SaveToDisk();
        }

        /// <summary>
        /// Deletes any files in the backup directory not found in the base file list.
        /// </summary>
        /// <param name="filesToBackup"></param>
        private void DeleteObsoleteFiles(List<string> filesToBackup)
        {
            backupFileInfo.DeleteObsoleteFiles();
        }

        /// <summary>
        /// Returns a list of all files that need to be backed up.
        /// </summary>
        /// <returns>list of complete filepaths</returns>
        private List<string> ListAllFilesUnderDirectory(string root)
        {
            List<string> filesToBackup = new List<string>();

            LoopChildDirectories(root, ref filesToBackup);

            return filesToBackup;
        }

        /// <summary>
        /// Recursive function that loops through all child directories within a parent directory and adds all files contained to the list.
        /// </summary>
        /// <param name="parentDirectory"></param>
        /// <param name="files"></param>
        private void LoopChildDirectories(string parentDirectory, ref List<string> files)
        {
            //first add the files inside this directory
            files.AddRange(Directory.GetFiles(parentDirectory));
            string[] childDirectories = Directory.GetDirectories(parentDirectory);

            foreach (var directory in childDirectories)
            {
                LoopChildDirectories(directory, ref files);
            }
        }

        /// <summary>
        /// Starts copying the backup files.
        /// </summary>
        /// <param name="filesToCopy"></param>
        private void CopyAllFiles(List<string> filesToCopy)
        {

            foreach (var file in filesToCopy)
            {
                Thread.Sleep(100);
                CopyFile(file);
            }

        }

        private void CopyFile(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            string fileName = fi.Name;
            string directory = fi.DirectoryName;
            string targetDirectory = Path.Combine(backupDirectory, DirectoryPartAfterBase(directory, baseDirectory));
            string targetFilePath = Path.Combine(targetDirectory, fileName);


            if (File.Exists(filePath))
            {
                Directory.CreateDirectory(targetDirectory);

                if (File.Exists(targetFilePath))
                {
                    if (FileNeedsBackup(filePath, targetFilePath))
                    {
                        //file exists already, create a revision entry
                        backupFileInfo.AddFileRevision(targetFilePath);
                    }
                    else
                    {
                        backupFileInfo.SkipFile(targetFilePath);
                        Logger.AddEntry($"Skipping file {filePath}");
                        return;
                    }

                }
                else
                {
                    //file looks new, add it to the file info list
                    backupFileInfo.AddFile(targetFilePath);
                }

                Logger.AddEntry($"Copying file {filePath}");
                try
                {
                    fi.CopyTo(targetFilePath, true);
                }
                catch (Exception ex)
                {
                    Logger.AddEntry($"Error: {ex.Message.ToString()}");
                }

            }
        }

        /// <summary>
        /// Checks if a file needs to be backed up.
        /// </summary>
        /// <param name="baseFile"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        private bool FileNeedsBackup(string baseFile, string targetFile)
        {
            FileInfo baseInfo = new FileInfo(baseFile);
            FileInfo targetInfo = new FileInfo(targetFile);

            if (baseInfo.Length != targetInfo.Length)
            {
                return true;
            }

            if (baseInfo.LastWriteTimeUtc != targetInfo.LastWriteTimeUtc)
            {
                return true;
            }

            return false;
        }

        private string RemoveWhatFromStringStart(string s, string what)
        {
            if (s.StartsWith(what))
                return s.Substring(what.Length);
            else
                return s;
        }

        /// <summary>
        /// Returns the part of a directory's path after a base path.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        private string DirectoryPartAfterBase(string fullPath, string basePath)
        {
            string directoryPartAfterBase = RemoveWhatFromStringStart(fullPath, basePath);
            return RemoveWhatFromStringStart(directoryPartAfterBase, @"\");
        }
    }
}
