using RadinProjectNotesCommon.DueItems;
using RadinProjectNotesCommon.EncryptedDatabaseSerializer;

namespace RadinProjectNotes.DatabaseFiles.Controllers
{
    public class DueItemsDatabaseController
    {
        private const string dueItemsFileNameExtension = @"kal";

        private static string GetDueItemsFilePath(ProjectFolder projectFolder)
        {
            return ServerConnection.GetDatabaseFilepathForProject(projectFolder, dueItemsFileNameExtension);
        }

        /// <summary>
        /// Try to load the due items database.
        /// </summary>
        /// <exception cref="CouldNotLoadDatabase"></exception>
        public static DueItemsDatabase TryLoadDueItems(ProjectFolder projectFolder)
        {
            EncryptedDatabaseProtobufSerializer<DueItemsDatabase> encryptedDbSerializer =
                new EncryptedDatabaseProtobufSerializer<DueItemsDatabase>(GetDueItemsFilePath(projectFolder));

            try
            {
                DueItemsDatabase loadedServices = encryptedDbSerializer.TryLoadDatabase();
                return loadedServices;
            }
            catch (CouldNotLoadDatabase)
            {
                throw;
            }
            catch (DatabaseFileNotFound)
            {
                // If the due items file does not exist, create an empty database.
                return DueItemsDatabase.CreateEmpty();
            }
        }

        /// <summary>
        /// Try to save the due items database.
        /// </summary>
        /// <returns>True if the due items database could be saved.</returns>
        /// <exception cref="CouldNotSaveDatabase"></exception>
        public static bool TrySaveDueItems(ProjectFolder projectFolder, DueItemsDatabase dueItemsDatabase)
        {
            EncryptedDatabaseProtobufSerializer<DueItemsDatabase> encryptedDbSerializer =
                new EncryptedDatabaseProtobufSerializer<DueItemsDatabase>(GetDueItemsFilePath(projectFolder));

            try
            {
                encryptedDbSerializer.TrySaveDatabase(dueItemsDatabase);
                return true;
            }
            catch (CouldNotSaveDatabase)
            {
                return false;
            }
        }
    }
}
