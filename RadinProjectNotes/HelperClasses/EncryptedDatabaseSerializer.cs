using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;


namespace RadinProjectNotes
{
    /// <summary>
    /// Serialize and encrypt any serializable class to a file.<br/>
    /// Supports simultaneous user access to the same file with the Try... functions.
    /// </summary>
    /// <typeparam name="T">Type of class instance to serialize.</typeparam>
    public class EncryptedDatabaseSerializer<T>
    {
        private string _filepath;
        private bool _hideDatabaseFile;

        /// <summary>
        /// Create an encrypted database serializer to save to the specified filepath.<br/>
        /// Can optionally hide the created file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="hideDatabaseFile"></param>
        public EncryptedDatabaseSerializer(string filepath, bool hideDatabaseFile = true)
        {
            _filepath = filepath;
            _hideDatabaseFile = hideDatabaseFile;
        }

        /// <summary>
        /// Try to load the project services.
        /// </summary>
        /// <exception cref="CouldNotLoadDatabase"></exception>
        public T TryLoadDatabase()
        {
            if (!File.Exists(_filepath))
            {
                throw new CouldNotLoadDatabase();
            }

            var maxRetryAttempts = 30;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            var loadedDatabase = RetryHelper<T>.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () => {
                return LoadDatabase();
            });

            // If the default value was returned, the database could not be loaded.
            if (EqualityComparer<T>.Default.Equals(loadedDatabase, default(T)))
            {
                throw new CouldNotLoadDatabase();
            }

            return loadedDatabase;
        }

        /// <summary>
        /// Try to save the project services.
        /// </summary>
        /// <param name="dataStructure">The class instance to save.</param>
        /// <exception cref="CouldNotSaveDatabase"></exception>
        public void TrySaveDatabase(T dataStructure)
        {
            var maxRetryAttempts = 30;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            var couldSave = RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () => {
                SaveDatabase(dataStructure);
            });

            if (!couldSave)
            {
                throw new CouldNotSaveDatabase();
            }
        }

        /// <summary>
        /// Encrypt and serialize a data structure of type T to a file.
        /// </summary>
        /// <param name="dataStructure"></param>
        private void SaveDatabase(T dataStructure)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            try
            {
                File.Delete(_filepath);
                using (var fs = new FileStream(_filepath, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(Security.desKey, Security.desIV), CryptoStreamMode.Write))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(cryptoStream, dataStructure);
                }

                if (_hideDatabaseFile)
                {
                    File.SetAttributes(_filepath, FileAttributes.Hidden);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message.ToString());
                throw;
            }
        }

        /// <summary>
        /// Decrypt an encrypted serialized data structure of type T from a file.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private T LoadDatabase()
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            try
            {
                // Decryption
                using (var fs = new FileStream(_filepath, FileMode.Open, FileAccess.Read))
                using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(Security.desKey, Security.desIV), CryptoStreamMode.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (T)formatter.Deserialize(cryptoStream);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message.ToString());
            }

            throw new Exception();
        }

        /// <summary>
        /// Throw when a data structure could not be saved to a file.
        /// </summary>
        public class CouldNotSaveDatabase : Exception
        {
            public CouldNotSaveDatabase() : base("Database could not be saved.")
            { }
        }

        /// <summary>
        /// Throw when a data structure could not be loaded from a file.
        /// </summary>
        public class CouldNotLoadDatabase : Exception
        {
            public CouldNotLoadDatabase() : base("Database could not be loaded.")
            { }
        }
    }
}
