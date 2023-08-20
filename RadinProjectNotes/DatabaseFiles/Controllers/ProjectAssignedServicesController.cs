
using EncryptedDatabaseSerializer;
using RadinProjectNotes.DatabaseFiles.ProjectServices;

namespace RadinProjectNotes.DatabaseFiles.Controllers
{
    public static class ProjectAssignedServicesController
    {
        private const string projectAssignedServicesFileNameExtension = @"bft";

        private static string GetProjectAssignedServicesFilePath(ProjectFolder projectFolder)
        {
            return ServerConnection.GetDatabaseFilepathForProject(projectFolder, projectAssignedServicesFileNameExtension);
        }

        /// <summary>
        /// Try to load the project assigned services.
        /// </summary>
        /// <exception cref="CouldNotLoadDatabase"></exception>
        public static ProjectAssignedServices TryLoadProjectServices(ProjectFolder projectFolder)
        {
            EncryptedDatabaseSerializer<ProjectAssignedServices>  encryptedDbSerializer = 
                new EncryptedDatabaseSerializer<ProjectAssignedServices>(GetProjectAssignedServicesFilePath(projectFolder));

            try
            {
                ProjectAssignedServices loadedServices = encryptedDbSerializer.LoadDatabase();
                return loadedServices;
            }
            catch (DatabaseFileNotFound)
            {
                // If the assigned services file does not exist, create an empty database.
                return ProjectAssignedServices.CreateEmpty();
            }
            catch (CouldNotLoadDatabase)
            {
                throw;
            }
        }

        /// <summary>
        /// Try to save the project assigned services.
        /// </summary>
        /// <returns>True if the assigned services could be saved.</returns>
        /// <exception cref="CouldNotSaveDatabase"></exception>
        public static bool TrySaveProjectServices(ProjectFolder projectFolder, ProjectAssignedServices projectAssignedServices)
        {            
            EncryptedDatabaseSerializer<ProjectAssignedServices> encryptedDbSerializer =
                new EncryptedDatabaseSerializer<ProjectAssignedServices>(GetProjectAssignedServicesFilePath(projectFolder));

            try
            {   
                encryptedDbSerializer.SaveDatabase(projectAssignedServices);
                return true;
            }
            catch (CouldNotSaveDatabase)
            {
                return false;
            }
        }
    }
}
