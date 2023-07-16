using System;
using System.IO;
using static RadinProjectNotes.EncryptedDatabaseSerializer<RadinProjectNotes.ProjectServices.RadinProjectServices>;

namespace RadinProjectNotes.ProjectServices
{
    [Serializable]
    public class ProjectServicesController
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

        public static ProjectServicesController _instance;    //singleton

        public static ProjectServicesController Instance
        {
            get
            {
                if (_instance != null) { return _instance; }
                _instance = new ProjectServicesController();
                return _instance;
            }
        }

        private EncryptedDatabaseSerializer<RadinProjectServices> _encryptedDbSerializer;

        private ProjectServicesController()
        {
            _encryptedDbSerializer = new EncryptedDatabaseSerializer<RadinProjectServices>(getServicesFileFilepath());
        }

        /// <summary>
        /// Try to load the project services.
        /// </summary>
        public RadinProjectServices TryLoadProjectServices()
        {
            try
            {
                RadinProjectServices loadedServices = _encryptedDbSerializer.TryLoadDatabase();
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
        public bool TrySaveProjectServices(RadinProjectServices services)
        {
            try
            {   
                _encryptedDbSerializer.TrySaveDatabase(services);
                return true;
            }
            catch (CouldNotSaveDatabase)
            {
                return false;
            }
        }
    }
}
