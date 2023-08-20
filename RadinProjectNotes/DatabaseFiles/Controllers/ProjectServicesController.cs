using EncryptedDatabaseSerializer;
using RadinProjectNotes.DatabaseFiles.ProjectServices;
using System.IO;

namespace RadinProjectNotes.DatabaseFiles.Controllers
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
        /// <exception cref="CouldNotLoadDatabase"></exception>
        public static RadinProjectServices TryLoadProjectServices()
        {
            EncryptedDatabaseSerializer<RadinProjectServices>  encryptedDbSerializer = 
                new EncryptedDatabaseSerializer<RadinProjectServices>(getServicesFileFilepath());

            try
            {
                RadinProjectServices loadedServices = encryptedDbSerializer.LoadDatabase();
                return loadedServices;
            }
            catch (DatabaseFileNotFound)
            {
                // If the project services file does not exist, create an empty database.
                return RadinProjectServices.CreateEmpty();
            }
            catch (CouldNotLoadDatabase)
            {
                throw;
            }
        }

        /// <summary>
        /// Try to save the project services.
        /// </summary>
        /// <returns>True if the services could be saved.</returns>
        /// <exception cref="CouldNotSaveDatabase"></exception>
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
