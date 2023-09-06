using RadinProjectNotesCommon;
using RadinProjectNotesCommon.DueItems;
using RadinProjectNotesCommon.EncryptedDatabaseSerializer;
using System.IO;

namespace ProjectEmailNotificationService
{
    internal class Program
    {
        //public readonly static string serverFolder = @"\\nas-radin-lp\DATEN\notes";
        public readonly static string serverFolder = @"C:\Users\Tsaras\Desktop\test_notes_server";

        public static byte[] desKey = { 10, 25, 11, 35, 207, 108, 56, 99 };
        public static byte[] desIV = { 38, 13, 7, 1, 116, 222, 111, 6 };

        private static Logger _logger;

        static void Main(string[] args)
        {
            _logger = new Logger(Filepaths.GetAppDataFolder());

            EncryptionKeys.SetEncryptionKeys(desKey, desIV);

            SendEmailsForUnprocessedExpiredDueItems();

            _logger.SaveLogFile(autoCreateFolder: true);
        }

        private static void SendEmailsForUnprocessedExpiredDueItems()
        {
            // Load the processed due items database.
            ProcessedDueItemsDatabase processedDueItems;
            try
            {
                processedDueItems = ProcessedDueItemsController.TryLoadProcessedDueItems();
            }
            catch (CouldNotLoadDatabase)
            {
                _logger.AddEntry($"Could not read from the processed due items database from file {ProcessedDueItemsController.DatabaseFilePath}.");
                return;
            }

            // Find all due item files.
            string[] dueItemFiles = Directory.GetFiles(serverFolder, "*.kal");

            foreach (string projectFile in dueItemFiles)
            {
                var serializer = new EncryptedDatabaseProtobufSerializer<DueItemsDatabase>(projectFile);

                // Load the due items for this project.
                DueItemsDatabase projectDueItems;
                try
                {
                    projectDueItems = serializer.TryLoadDatabase();
                }
                catch (CouldNotLoadDatabase)
                {
                    continue;
                }

                string projectCode = GetProjectCodeFromProjectFullName(Path.GetFileName(projectFile));
                foreach (var dueItem in projectDueItems.DueItems)
                {
                    if (dueItem.HasExpired() && !processedDueItems.Contains(projectCode, dueItem.Id))
                    {
                        EmailSender.SendEmail($"Due item \"{dueItem.Description}\" expired.");
                        _logger.AddEntry($"Sending emails for due item with id {dueItem.Id} of project {projectCode}.");
                        ProcessedDueItem processedDueItem = new ProcessedDueItem(projectCode, dueItem.Id);
                        processedDueItems.Add(processedDueItem);
                    }
                }
            }

            try
            {
                ProcessedDueItemsController.TrySaveProcessedDueItems(processedDueItems);
            }
            catch (CouldNotSaveDatabase)
            {
                _logger.AddEntry($"Could not save the processed due items database to file {ProcessedDueItemsController.DatabaseFilePath}.");
            }
        }

        private static string GetProjectCodeFromProjectFullName(string projectFullName)
        {
            if (projectFullName.Length > 9)
            {
                return projectFullName.Substring(0, 9);
            }
            return projectFullName;
        }
    }
}
