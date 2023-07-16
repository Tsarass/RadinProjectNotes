using RadinProjectNotes.ProjectServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RadinProjectNotes
{
    /// <summary>
    /// Serialize and encrypt any serializable class to a file.<br/>
    /// Supports simultaneous user access to the same file with the Try... functions.
    /// </summary>
    /// <typeparam name="T">Type of class to serialize.</typeparam>
    public class EncryptedDatabaseSerializer<T>
    {
        private string _filepath;

        public EncryptedDatabaseSerializer(string filepath)
        {
            _filepath = filepath;
        }

        public bool SuccessfullyLoaded { get; set; }

        /// <summary>
        /// Try to load the project services.
        /// </summary>
        public void TryLoadDatabase()
        {
            var maxRetryAttempts = 30;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () => {
                LoadDatabase();
            });
        }

        /// <summary>
        /// Try to save the project services.
        /// </summary>
        public void TrySaveDatabase()
        {
            var maxRetryAttempts = 30;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () => {
                SaveDatabase();
            });
        }

        private void SaveDatabase()
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

        private void LoadDatabase()
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
