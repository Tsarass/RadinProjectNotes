using DueItems;
using EncryptedDatabaseSerializer;
using System;
using System.IO;

namespace ProjectEmailNotificationService
{
    internal class ProcessedDueItemsController
    {
        static readonly string processedDueItemsDatabaseFilename = @"processed_items.db";

        public static string DatabaseFilePath
        {
            get
            {
                return Path.Combine(AppdataFolder, processedDueItemsDatabaseFilename);
            }
        }

        public static string AppdataFolder
        {
            get
            {
                string path = @"%appdata%\ProjektNotesEmailService";
                path = Environment.ExpandEnvironmentVariables(path);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        /// <summary>
        /// Try to load the processed due items database.
        /// </summary>
        /// <exception cref="CouldNotLoadDatabase"></exception>
        public static ProcessedDueItemsDatabase TryLoadProcessedDueItems()
        {
            EncryptedDatabaseSerializer<ProcessedDueItemsDatabase> encryptedDbSerializer =
                new EncryptedDatabaseSerializer<ProcessedDueItemsDatabase>(DatabaseFilePath);

            try
            {
                ProcessedDueItemsDatabase database = encryptedDbSerializer.TryLoadDatabase();
                return database;
            }
            catch (CouldNotLoadDatabase)
            {
                throw;
            }
            catch (DatabaseFileNotFound)
            {
                // If the due items file does not exist, create an empty database.
                return ProcessedDueItemsDatabase.CreateEmpty();
            }
        }

        /// <summary>
        /// Try to save the processed due items database.
        /// </summary>
        /// <returns>True if the due items database could be saved.</returns>
        /// <exception cref="CouldNotSaveDatabase"></exception>
        public static bool TrySaveProcessedDueItems(ProcessedDueItemsDatabase dueItemsDatabase)
        {
            EncryptedDatabaseSerializer<ProcessedDueItemsDatabase> encryptedDbSerializer =
                new EncryptedDatabaseSerializer<ProcessedDueItemsDatabase>(DatabaseFilePath);

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
