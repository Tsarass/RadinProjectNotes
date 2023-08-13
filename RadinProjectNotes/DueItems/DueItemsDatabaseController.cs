using RadinProjectNotes.DatabaseFiles;
using System.IO;

namespace RadinProjectNotes.DueItems
{
    public class DueItemsDatabaseController
    {
        private const string dueItemsDatabaseFile = @"due_items.db";

        /// <summary>
        /// Get the filepath to the due items database file.
        /// </summary>
        /// <returns></returns>
        public static string getDueItemsDatabaseFilepath()
        {
            return Path.Combine(ServerConnection.serverFolder, dueItemsDatabaseFile);
        }

        /// <summary>
        /// Try to load the due items database.
        /// </summary>
        /// <exception cref="DatabaseFileNotFound"></exception>
        public static DueItemsDatabase TryLoadDueItems()
        {
            EncryptedDatabaseSerializer<DueItemsDatabase> encryptedDbSerializer =
                new EncryptedDatabaseSerializer<DueItemsDatabase>(getDueItemsDatabaseFilepath());

            try
            {
                DueItemsDatabase loadedServices = encryptedDbSerializer.LoadDatabase();
                return loadedServices;
            }
            catch (CouldNotLoadDatabase)
            {
                throw;
            }
            catch (DatabaseFileNotFound)
            {
                return DueItemsDatabase.CreateEmpty();
            }
        }

        /// <summary>
        /// Try to save the due items database.
        /// </summary>
        /// <returns>True if the due items database could be saved.</returns>
        public static bool TrySaveDueItems(DueItemsDatabase dueItemsDatabase)
        {
            EncryptedDatabaseSerializer<DueItemsDatabase> encryptedDbSerializer =
                new EncryptedDatabaseSerializer<DueItemsDatabase>(getDueItemsDatabaseFilepath());

            try
            {
                encryptedDbSerializer.SaveDatabase(dueItemsDatabase);
                return true;
            }
            catch (CouldNotSaveDatabase)
            {
                return false;
            }
        }
    }
}
