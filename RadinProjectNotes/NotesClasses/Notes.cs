using RetryOnExceptionHelper;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace RadinProjectNotes
{
    public partial class Notes
    {
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

        public static void TryLoadNotesDatabaseInMemory(ProjectFolder projectFolder)
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
        private static void LoadNotesDatabaseInMemory(ProjectFolder projectFolder)
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

        public static void SaveNewNoteToProjectNoteDatabase(ProjectFolder currentProject, ProjectNote newNote)
        {
            //first open the file from server
            //required to have simultaneous multi-user access to same project notes
            TryLoadNotesDatabaseInMemory(currentProject);

            currentNoteData.noteData.Add(newNote);

            TrySaveNotesDatabase(currentProject);
        }

        public static void TrySaveNotesDatabase(ProjectFolder currentProject)
        {
            var maxRetryAttempts = 30;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () =>
            {
                SaveNotesDatabase(currentNoteData, currentProject);
            });

        }

        private static void SaveNotesDatabase(Versioning.SaveStructureV1 noteDataToSave, ProjectFolder projectFolder)
        {
            //create save structure 
            Versioning.SaveStructureV1 save = new Versioning.SaveStructureV1(1, noteDataToSave.noteData);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            string dbFilePath = ServerConnection.GetDatabaseFilepathForProject(projectFolder);
            string backupFile = dbFilePath + ".bak";

            // Encryption
            try
            {
                if (File.Exists(dbFilePath))
                {
                    // Back up file.
                    File.Copy(dbFilePath, backupFile, overwrite: true);
                    File.Delete(dbFilePath);
                }
                
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
                if (File.Exists(backupFile))
                {
                    // Restore backup file.
                    File.Copy(backupFile, dbFilePath, overwrite: true);
                    File.Delete(backupFile);
                }

                //MessageBox.Show(this, e.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine($"Exception caught while accessing file to SaveNotesDatabase", e);
                throw;
            }

            //Delete backup file.
            File.Delete(backupFile);
        }

        public static Versioning.SaveStructureV1 LoadDatabaseFile(ProjectFolder projectFolder)
        {
            Versioning.SaveStructureV1 data = new Versioning.SaveStructureV1();

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            string dbFilePath = ServerConnection.GetDatabaseFilepathForProject(projectFolder);

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
    }
}
