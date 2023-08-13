using RadinProjectNotes.DatabaseFiles;
using System.IO;

namespace RadinProjectNotes.ProjectServices
{
    public static class ProjectServicesController
    {
        private const string projectServicesFile = @"project_services.db";

        /// <summary>
        /// Get the filepath to the services file.
        /// </summary>
        /// <returns></returns>
        public static string getServicesFileFilepath()
        {
            return Path.Combine(ServerConnection.serverFolder, projectServicesFile);
        }

        /// <summary>
        /// Try to load the project services.
        /// </summary>
        public static RadinProjectServices TryLoadProjectServices()
        {
            EncryptedDatabaseSerializer<RadinProjectServices>  encryptedDbSerializer = 
                new EncryptedDatabaseSerializer<RadinProjectServices>(getServicesFileFilepath());

            try
            {
                RadinProjectServices loadedServices = encryptedDbSerializer.LoadDatabase();
                return loadedServices;
            }
            catch (CouldNotLoadDatabase)
            {
                return RadinProjectServices.CreateEmpty();
            }
        }

        /// <summary>
        /// Try to save the project services.
        /// </summary>
        /// <returns>True if the services could be saved.</returns>
        public static bool TrySaveProjectServices(RadinProjectServices services)
        {
            EncryptedDatabaseSerializer<RadinProjectServices> encryptedDbSerializer =
                new EncryptedDatabaseSerializer<RadinProjectServices>(getServicesFileFilepath());

            try
            {   
                encryptedDbSerializer.SaveDatabase(services);
                return true;
            }
            catch (CouldNotSaveDatabase)
            {
                return false;
            }
        }
    }
}
