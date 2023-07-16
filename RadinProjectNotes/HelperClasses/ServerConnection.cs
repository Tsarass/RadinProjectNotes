using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public partial class ServerConnection
    {
        public readonly static string serverFolder = @"\\nas-radin-lp\DATEN\notes";
        //public readonly static string serverFolder = @"C:\Users\Tsaras\Desktop\test_notes_server";
        
        private readonly static string[] possibleServerPaths = new string[] {  @"C:\Users\Tsaras\Desktop\test_notes_projects",
                                                                               @"\\nas-radin-gr\Projekte",
                                                                               @"\\nas-radin-lp\Projekte",};
        public static List<ProjectFolder> folderCache = new List<ProjectFolder>();


        public static bool Check()
        {
            DialogResult dlg;
            while(!Directory.Exists(serverFolder))
            {
                dlg = MessageBox.Show("Could not connect to server.\nTry again?", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);

                if (dlg == DialogResult.Cancel)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets all valid project folder names; after and including "projekte 2016".
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAndCacheValidProjectFolders()
        {
            string[] subDirectories = GetProjectFolderSubdirectories();
            List<string> validProjectFolders = GetValidProjectFolders(subDirectories);
            CreateFolderCache(validProjectFolders);

            List<string> projectPaths = new List<string> { };
            foreach (ProjectFolder folder in folderCache)
            {
                projectPaths.Add(folder.projectPath);
            }

            return projectPaths;
        }

        private static List<string> GetValidProjectFolders(string[] subDirectories)
        {
            List<string> searchFolders = new List<string>();

            foreach (string subDir in subDirectories)
            {
                if (subDir.Contains("projekte"))
                {
                    string folderName = new DirectoryInfo(subDir).Name;
                    string number = folderName.Substring(9, 4);
                    bool couldParse = Int32.TryParse(number, out int year);
                    if (!couldParse)
                        continue;
                    if (year >= 2016)
                    {
                        searchFolders.Add(subDir);
                    }
                }
            }

            return searchFolders;
        }

        private static string[] GetProjectFolderSubdirectories()
        {
            string[] subDirectories = new string[1];

            bool success = false;
            foreach (string possiblePath in possibleServerPaths)
            {
                if (Directory.Exists(possiblePath))
                {
                    subDirectories = Directory.GetDirectories(possiblePath);
                    success = true;
                    break;
                }
            }

            if (!success)
            {
                MessageBox.Show("Could not connect to server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return subDirectories;
        }

        private static void CreateFolderCache(List<string> searchFolders)
        {
            folderCache.Clear();

            foreach (string searchFolder in searchFolders)
            {
                if (!searchFolder.Contains("projekte")) continue;
                try
                {
                    string[] subFolders = Directory.GetDirectories(searchFolder);
                    foreach (string subFolder in subFolders)
                    {
                        ProjectFolder newFolder = new ProjectFolder
                        {
                            fullPath = subFolder,
                            projectPath = new DirectoryInfo(subFolder).Name.Replace('_', ' ')
                        };

                        folderCache.Add(newFolder);
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Debug.WriteLine("Warning: could not access search folder {0}.", searchFolder);
                    continue;
                }
            }
        }

        public static ProjectFolder GetProjectFolderFromProjectPath(string projectPath)
        {
            //compare project code
            string projectCode = projectPath.Substring(0, 9);

            foreach (ProjectFolder folder in folderCache)
            {
                string folderProjectCode = folder.projectPath.Substring(0, 9);
                if (projectCode == folderProjectCode)
                {
                    return folder;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the database file for a project with the specified extension.
        /// </summary>
        /// <remarks>Tries to find the note database file for the project in question by looking
        /// only at the project code number.</remarks>
        /// <param name="projectFolder"></param>
        /// <param name="databaseFileFilter">Filter to apply to files in directory.</param>
        /// <returns></returns>
        public static string GetDatabaseFilepathForProject(ProjectFolder projectFolder, string fileExtension = "db")
        {
            string projectCode = projectFolder.projectPath.Substring(0, 9);

            string[] filePaths = Directory.GetFiles(serverFolder, $"*.{fileExtension}",
                                         SearchOption.TopDirectoryOnly);
            foreach (var filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                if (fileName.Length < 9)
                {
                    continue;
                }

                string pathProjectCode = fileName.Substring(0, 9);
                if (pathProjectCode == projectCode)
                {
                    return Path.Combine(serverFolder, filePath);
                }
            }

            return GetDefaultProjectDatabaseFilePath(projectFolder, fileExtension);
        }

        /// <summary>
        /// Get the default project database file path.
        /// </summary>
        /// <param name="projectFolder"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string GetDefaultProjectDatabaseFilePath(ProjectFolder projectFolder, string fileExtension = "db") 
        {
            return Path.Combine(serverFolder, $"{projectFolder.projectPath}.{fileExtension}");
        }

    }
}
