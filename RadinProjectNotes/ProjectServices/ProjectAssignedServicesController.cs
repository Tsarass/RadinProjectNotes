using System;
using System.IO;
using static RadinProjectNotes.EncryptedDatabaseSerializer<RadinProjectNotes.ProjectServices.RadinProjectServices>;

namespace RadinProjectNotes.ProjectServices
{
    public static class ProjectAssignedServicesController
    {
        private const string projectAssignedServicesFileNameExtension = @"bft";

        /// <summary>
        /// Get the filepath to the assigned services file for a project.
        /// </summary>
        /// <returns></returns>
        public static string getProjectAssignedServicesFileFilepath(ProjectFolder projectFolder)
        {
            string projectAssignedServicesDatabaseFilePath = $"{projectFolder.projectPath}.{projectAssignedServicesFileNameExtension}";
            return Path.Combine(ServerConnection.serverFolder, projectAssignedServicesDatabaseFilePath);
        }

        /// <summary>
        /// Try to load the project assigned services.
        /// </summary>
        public static ProjectAssignedServices TryLoadProjectServices(ProjectFolder projectFolder)
        {
            // If the assigned services file does not exist, create an empty database.
            string filePath = ServerConnection.GetDatabaseFilepathForProject(projectFolder, projectAssignedServicesFileNameExtension);
            if (string.IsNullOrEmpty(filePath))
            {
                return ProjectAssignedServices.CreateEmpty();
            }

            EncryptedDatabaseSerializer<ProjectAssignedServices>  encryptedDbSerializer = 
                new EncryptedDatabaseSerializer<ProjectAssignedServices>(filePath);

            try
            {
                ProjectAssignedServices loadedServices = encryptedDbSerializer.TryLoadDatabase();
                return loadedServices;
            }
            catch (CouldNotLoadDatabase)
            {
                return ProjectAssignedServices.CreateEmpty();
            }
        }

        /// <summary>
        /// Try to save the project assigned services.
        /// </summary>
        /// <returns>True if the assigned services could be saved.</returns>
        public static bool TrySaveProjectServices(ProjectFolder projectFolder, ProjectAssignedServices projectAssignedServices)
        {
            string filePath = ServerConnection.GetDatabaseFilepathForProject(projectFolder, projectAssignedServicesFileNameExtension);
            
            EncryptedDatabaseSerializer<ProjectAssignedServices> encryptedDbSerializer =
                new EncryptedDatabaseSerializer<ProjectAssignedServices>(filePath);

            try
            {   
                encryptedDbSerializer.TrySaveDatabase(projectAssignedServices);
                return true;
            }
            catch (CouldNotSaveDatabase)
            {
                return false;
            }
        }
    }
}
