using RadinProjectNotesCommon.EncryptedDatabaseSerializer;
using RadinProjectNotes.DatabaseFiles.ProjectServices;
using System.IO;
using ConfigurationFileIO;
using System.Linq;
using System.Collections.Generic;
using System;
using ProtoBuf.Meta;

namespace RadinProjectNotes.DatabaseFiles.Controllers
{
    public static class ProjectServicesController
    {
        private const string projectServicesFile = @"project_services.cfg";

        /// <summary>
        /// Get the filepath to the services file.
        /// </summary>
        /// <returns></returns>
        public static string getServicesFileFilepath()
        {
            return Path.Combine(ServerConnection.serverFolder, projectServicesFile);
        }

        /// <summary>
        /// Load the list of project services per version from the configuration file.
        /// </summary>
        /// <returns>A dictionary of (version, list project services).</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Dictionary<string, ProjectServicesDatabase> LoadProjectServices()
        {
            if (!File.Exists(getServicesFileFilepath())) {
                return new Dictionary<string, ProjectServicesDatabase>();
            }

            ConfigurationFile servicesCfg = new ConfigurationFile(getServicesFileFilepath());
            servicesCfg.ReadSettings();

            Dictionary<string, ProjectServicesDatabase> result = new Dictionary<string, ProjectServicesDatabase>();
            foreach (var version in servicesCfg.GetSettingsCategories()) {
                List<ServiceCategory> cats = new List<ServiceCategory>();
                var serviceCategories = servicesCfg.GetSettingsInCategory(version);
                foreach (var serviceCategory in serviceCategories) {
                    cats.Add(new ServiceCategory(serviceCategory,
                        servicesCfg.GetSettingValue(version, serviceCategory).AsString().Split(';').ToList()));
                }
                result.Add(version, new ProjectServicesDatabase(cats));
            }

            return result;
        }

        /// <summary>
        /// Save the project services.
        /// </summary>
        /// <returns>True if the services could be saved.</returns>
        public static bool SaveProjectServices(Dictionary<string, ProjectServicesDatabase> servicesPerVersion)
        {
            ConfigurationFile servicesCfg = new ConfigurationFile(getServicesFileFilepath());
            foreach (var version in servicesPerVersion.Keys) {
                servicesCfg.AddSettingsCategory(version);
                foreach (var category in servicesPerVersion[version].GetCategoryTitles()) {
                    servicesCfg.AddNewSetting(version, category, string.Join(";", servicesPerVersion[version].getServicesForCategory(category)));
                }
            }
            
            try {
                servicesCfg.WriteSettings();
            }
            catch {
                return false;
            }

            return true;
            
        }
    }
}
