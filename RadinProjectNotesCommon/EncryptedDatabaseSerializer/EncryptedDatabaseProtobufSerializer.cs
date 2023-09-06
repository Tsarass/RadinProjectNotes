using RadinProjectNotesCommon.RetryOnExceptionHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;


namespace RadinProjectNotesCommon.EncryptedDatabaseSerializer
{
    /// <summary>
    /// Serialize and encrypt any serializable class to a file usign protobuf.<br/>
    /// Supports simultaneous user access to the same file with the Try... functions.
    /// </summary>
    /// <typeparam name="T">Type of class instance to serialize.</typeparam>
    public class EncryptedDatabaseProtobufSerializer<T>
    {
        private string _filepath;
        private bool _hideDatabaseFile;

        /// <summary>
        /// Create an encrypted database serializer to save to the specified filepath.<br/>
        /// Can optionally hide the created file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="hideDatabaseFile"></param>
        public EncryptedDatabaseProtobufSerializer(string filepath, bool hideDatabaseFile = true)
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
                throw new DatabaseFileNotFound();
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
        public void SaveDatabase(T dataStructure)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            string backupFile = _filepath + ".bak";
            try
            {
                if (File.Exists(_filepath))
                {
                    // Back up file.
                    File.Copy(_filepath, backupFile, overwrite: true);
                    File.Delete(_filepath);
                }
                using (var fs = new FileStream(_filepath, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(EncryptionKeys.DesKey, EncryptionKeys.DesIV), CryptoStreamMode.Write))
                {
                    ProtoBuf.Serializer.Serialize(cryptoStream, dataStructure);
                }

                if (_hideDatabaseFile)
                {
                    File.SetAttributes(_filepath, FileAttributes.Hidden);
                }
            }
            catch
            {
                if (File.Exists(backupFile))
                {
                    // Restore backup file.
                    File.Copy(backupFile, _filepath, overwrite: true);
                    File.Delete(backupFile);
                }

                throw new CouldNotSaveDatabase();
            }

            File.Delete(backupFile);
        }

        /// <summary>
        /// Decrypt an encrypted serialized data structure of type T from a file.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T LoadDatabase()
        {
            if (!File.Exists(_filepath))
            {
                throw new DatabaseFileNotFound();
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            try
            {
                // Decryption
                using (var fs = new FileStream(_filepath, FileMode.Open, FileAccess.Read))
                using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(EncryptionKeys.DesKey, EncryptionKeys.DesIV), CryptoStreamMode.Read))
                {
                    return ProtoBuf.Serializer.Deserialize<T>(cryptoStream);
                }
            }
            catch
            {
                throw new CouldNotLoadDatabase();
            }
        }
    }
}
