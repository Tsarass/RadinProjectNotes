using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
    public class FilesUpdater
    {
        private string updateFromPath = "";
        private string updateToPath = "";

        /// <summary>
        /// Creates an instance of an updater for the miscellaneous files of the application.
        /// </summary>
        /// <param name="updateFromPath">the path where the installation files are found</param>
        public FilesUpdater(string updateFromPath, string updateToPath)
        {
            this.updateFromPath = updateFromPath;
            this.updateToPath = updateToPath;
            UpdateFiles();
        }

        private void UpdateFiles()
        {
            Console.WriteLine("Updating files..");
            List<string> filesToUpdate = ListAllFilesUnderDirectory(updateFromPath);

            foreach (var file in filesToUpdate)
            {
                //set up useful paths
                string filePathAfterBaseRoot = DirectoryPartAfterBase(file, updateFromPath);
                string filePathInTargetFolder = Path.Combine(updateToPath, filePathAfterBaseRoot);

                //first check if the file exists in target directory
                if (!File.Exists(filePathInTargetFolder))
                {
                    UpdateFile(file, filePathInTargetFolder);
                }
                else //file exists in target folder
                {
                    if (FileNeedsUpdate(file, filePathInTargetFolder))
                    {
                        UpdateFile(file, filePathInTargetFolder);
                    }
                }

                
            }

        }

        /// <summary>
        /// Checks if a file needs to be updated through CRC check.
        /// </summary>
        /// <returns>true if the file needs to be updated</returns>
        private bool FileNeedsUpdate(string filePathBase, string filePathTarget)
        {
            string hashBaseFile = FileSHA1(filePathBase);
            string hashTargetFile = FileSHA1(filePathTarget);

            if ((hashBaseFile == String.Empty) || (hashTargetFile == String.Empty))
            {
                return false;
            }

            if (hashBaseFile != hashTargetFile)
            {
                return true;
            }

            return false;
        }

        private string FileSHA1(string filePath)
        {
            using (FileStream sourceFileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var cryptoProvider = new SHA1CryptoServiceProvider())
            {
                string hash = BitConverter
                        .ToString(cryptoProvider.ComputeHash(sourceFileStream));

                return hash;
            }
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
            //exclude .exe files
            files.AddRange(Directory.GetFiles(parentDirectory).Where(s => !s.EndsWith(".exe")));
            string[] childDirectories = Directory.GetDirectories(parentDirectory);

            foreach (var directory in childDirectories)
            {
                LoopChildDirectories(directory, ref files);
            }
        }

        /// <summary>
        /// Updates a single file using copy with overwrite.
        /// </summary>
        /// <param name="fromPath">source file</param>
        /// <param name="toPath"> destination file</param>
        /// <returns>true if update was successfull</returns>
        public static bool UpdateFile(string fromPath, string toPath)
        {
            if (File.Exists(fromPath))
            {
                Console.WriteLine($"Updating file: {toPath}");
                FileInfo sourcefile = new FileInfo(fromPath);
                try
                {
                    CreateDirectoryForFile(toPath);
                    sourcefile.CopyTo(toPath, true);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine($"Could not update file: {toPath}");
                }
            }
            else
            {
                Console.WriteLine($"Could not locate file: {fromPath}");
            }

            return false;
        }

        private string CutStart(string s, string what)
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
            string directoryPartAfterBase = CutStart(fullPath, basePath);
            return CutStart(directoryPartAfterBase, @"\");
        }

        /// <summary>
        /// Creates the directory hosting a file.
        /// </summary>
        /// <param name="fileTargetPath"></param>
        private static void CreateDirectoryForFile(string fileTargetPath)
        {
            FileInfo fi = new FileInfo(fileTargetPath);
            string directoryPath = fi.DirectoryName;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
