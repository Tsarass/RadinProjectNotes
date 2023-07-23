using System.IO;
using static RadinProjectNotes.EncryptedDatabaseSerializer<RadinProjectNotes.DueItems.DueItemsDatabase>;

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
        public static DueItemsDatabase TryLoadProjectServices()
        {
            EncryptedDatabaseSerializer<DueItemsDatabase> encryptedDbSerializer =
                new EncryptedDatabaseSerializer<DueItemsDatabase>(getDueItemsDatabaseFilepath());

            try
            {
                DueItemsDatabase loadedServices = encryptedDbSerializer.TryLoadDatabase();
                return loadedServices;
            }
            catch (CouldNotLoadDatabase)
            {
                return DueItemsDatabase.CreateEmpty();
            }
        }

        /// <summary>
        /// Try to save the due items database.
        /// </summary>
        /// <returns>True if the due items database could be saved.</returns>
        public static bool TrySaveProjectServices(DueItemsDatabase dueItemsDatabase)
        {
            EncryptedDatabaseSerializer<DueItemsDatabase> encryptedDbSerializer =
                new EncryptedDatabaseSerializer<DueItemsDatabase>(getDueItemsDatabaseFilepath());

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
