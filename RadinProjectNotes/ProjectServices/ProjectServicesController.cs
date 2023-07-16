using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

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

        private ProjectServicesController()
        { }

        public bool SuccessfullyLoaded { get; set; }
        public ServiceCategories ServiceCategories { get; set; }

        /// <summary>
        /// Try to load the project services.
        /// </summary>
        public void TryLoadProjectServices()
        {
            var maxRetryAttempts = 30;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () => {
                LoadProjectServices();
            });
        }

        /// <summary>
        /// Try to save the project services.
        /// </summary>
        public void TrySaveProjectServices()
        {
            var maxRetryAttempts = 30;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () => {
                SaveProjectServices();
            });
        }

        private void SaveProjectServices()
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Encryption
            try
            {
                File.Delete(getServicesFileFilepath());
                using (var fs = new FileStream(getServicesFileFilepath(), FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(Security.desKey, Security.desIV), CryptoStreamMode.Write))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(cryptoStream, ServiceCategories);
                }

                //hide file
                File.SetAttributes(getServicesFileFilepath(), FileAttributes.Hidden);
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.WriteLine(e.Message.ToString());
                throw;
            }
        }

        private void LoadProjectServices()
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            if (!ServerConnection.Check())
            {
                SuccessfullyLoaded = false;
                return;
            }

            //check to see if database exists
            if (File.Exists(getServicesFileFilepath()))
            {
                try
                {
                    // Decryption
                    using (var fs = new FileStream(getServicesFileFilepath(), FileMode.Open, FileAccess.Read))
                    using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(Security.desKey, Security.desIV), CryptoStreamMode.Read))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        ServiceCategories = (ServiceCategories)formatter.Deserialize(cryptoStream);
                    }
                    SuccessfullyLoaded = true;
                }
                catch (IOException e)
                {
                    Debug.WriteLine(e.Message.ToString());
                }
            }
            else
            {
                // database not found, create empty
                ServiceCategories = new ServiceCategories(new List<ServiceCategory>());
                SuccessfullyLoaded = false;
            }
        }
    }
}
