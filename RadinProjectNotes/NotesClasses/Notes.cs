using ProtoBuf;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace RadinProjectNotes
{
    public partial class Notes
    {
        #region Static methods/variables
        public static readonly long maxAttachmentByteSize = 30 * 1024 * 1024;
        public static readonly int maxEditHours = 24 * 3;
        public static readonly int maxDeleteHours = 24 * 20;

        public static Versioning.SaveStructureV1 currentNoteData = new Versioning.SaveStructureV1();
        public static ProjectNote currentNote;

        public static int maxAttachmentMB
        {
            get
            {
                return (int)(maxAttachmentByteSize / 1024 / 1024);
            }
        }

        public static string GetCurrentProjectAttachmentsFolder()
        {
            string projectName = MainForm.currentProject.projectPath;
            string attachmentsFolder = Path.Combine(ServerConnection.serverFolder, projectName);
            return attachmentsFolder;
        }

        public static void TryLoadNotesDatabaseInMemory(ServerConnection.ProjectFolder projectFolder)
        {
            var maxRetryAttempts = 20;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () =>
            {
                LoadNotesDatabaseInMemory(projectFolder);
            });
        }

        /// <summary>
        /// Load the notes database and store in member variable.
        /// </summary>
        /// <param name="projectFolder"></param>
        private static void LoadNotesDatabaseInMemory(ServerConnection.ProjectFolder projectFolder)
        {
            try
            {
                currentNoteData = LoadDatabaseFile(projectFolder);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error while loading database file.", e);
                throw;
            }
        }

        public static void SaveNewNoteToProjectNoteDatabase(ServerConnection.ProjectFolder currentProject, ProjectNote newNote)
        {
            //first open the file from server
            //required to have simultaneous multi-user access to same project notes
            TryLoadNotesDatabaseInMemory(currentProject);

            Notes.currentNoteData.noteData.Add(newNote);

            TrySaveNotesDatabase(currentProject);
        }

        public static void TrySaveNotesDatabase(ServerConnection.ProjectFolder currentProject)
        {
            var maxRetryAttempts = 30;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () =>
            {
                Notes.SaveNotesDatabase(currentNoteData, currentProject.projectPath);
            });

        }

        private static void SaveNotesDatabase(Versioning.SaveStructureV1 noteDataToSave, string projectPath)
        {
            //create save structure 
            Versioning.SaveStructureV1 save = new Versioning.SaveStructureV1(1, noteDataToSave.noteData);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            string dbFilePath = Path.Combine(ServerConnection.serverFolder, projectPath + ".db");

            // Encryption
            try
            {
                using (var fs = new FileStream(dbFilePath, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(Security.desKey, Security.desIV), CryptoStreamMode.Write))
                {

                    // This is where you serialize the class
                    ProtoBuf.Serializer.Serialize(cryptoStream, save);
                }

                //hide file
                File.SetAttributes(dbFilePath, FileAttributes.Hidden);
            }
            catch (Exception e)
            {
                //MessageBox.Show(this, e.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine($"Exception caught while accessing file to SaveNotesDatabase", e);
                throw;
            }
        }

        public static Versioning.SaveStructureV1 LoadDatabaseFile(ServerConnection.ProjectFolder projectFolder)
        {
            Versioning.SaveStructureV1 data = new Versioning.SaveStructureV1();

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //check to see if database exists
            string dbFilePath = TryFindDatabaseFilepathForProject(projectFolder);
            if (dbFilePath == string.Empty)
            {
                dbFilePath = Path.Combine(ServerConnection.serverFolder, projectFolder.projectPath + ".db");
            }

            if (File.Exists(dbFilePath))
            {
                try
                {
                    // Decryption
                    Debug.WriteLine("Trying to load database file.");
                    using (var fs = new FileStream(dbFilePath, FileMode.Open, FileAccess.Read))
                    using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(Security.desKey, Security.desIV), CryptoStreamMode.Read))
                    {
                        // This is where you deserialize the class
                        data = ProtoBuf.Serializer.Deserialize<Versioning.SaveStructureV1>(cryptoStream);
                        Debug.WriteLine("Successfully loaded database file.");
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(this, e.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Debug.WriteLine("Error while loading database file..", e);
                    throw;
                }

            }
            else
            {
                //check to create file maybe..
                Debug.WriteLine("LoadDatabaseFile: File doesn't exist!");
            }

            Debug.WriteLine($"Returning loaded database file with {data.noteData.Count} notes.");
            return data;
        }

        /// <summary>
        /// Tries to find the note database file for the project in question by looking
        /// only at the project code number.
        /// </summary>
        /// <param name="projectFolder"></param>
        /// <returns></returns>
        private static string TryFindDatabaseFilepathForProject(ServerConnection.ProjectFolder projectFolder)
        {
            string projectCode = projectFolder.projectPath.Substring(0, 9);
            string dbStoragePath = ServerConnection.serverFolder;

            string[] filePaths = Directory.GetFiles(dbStoragePath, "*.db",
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
                    return Path.Combine(dbStoragePath, filePath);
                }
            }

            return String.Empty;
        }

        #endregion

        
    }
}
