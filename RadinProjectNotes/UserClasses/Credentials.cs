using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace RadinProjectNotes
{
    /// <summary>
    /// Main interface class to interact with and store the application's user
    /// database. An instance of this class must be loaded in order for the application
    /// to function correctly.
    /// </summary>
    public class Credentials
    {
        private string databaseFile = Path.Combine(ServerConnection.serverFolder, "users.db");

        public UserDatabase userDatabase;
        public User currentUser;
        public bool SuccessfullyLoaded { get; set; }

        public Credentials()
        {
            SuccessfullyLoaded = false;
            userDatabase = new UserDatabase();
        }

        /// <summary>
        /// Try to load the user database.
        /// </summary>
        public void TryLoadUserDatabase()
        {
            var maxRetryAttempts = 30;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () => {
                LoadUserDatabase();
            });
        }

        /// <summary>
        /// Try to save the user database.
        /// </summary>
        public void TrySaveUserDatabase()
        {
            var maxRetryAttempts = 30;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(300);
            RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () => {
                SaveUserDatabase();
            });
        }

        /// <summary>
        /// Adds a user to the database safely; after reloading the database first.
        /// Saves the database in the end.
        /// </summary>
        public User AddUserSafelyAndSaveDatabase(string username, string password, Permissions permissions)
        {
            //reload database
            TryLoadUserDatabase();

            User newUser = userDatabase.AddUser(username, password, permissions);

            //save
            TrySaveUserDatabase();

            return newUser;
        }

        public User MatchUserOrNull(string username, string password)
        {
            return userDatabase.FindUserOrNull(username, password);
        }

        public User MatchUserOrNull(Guid id)
        {
            return userDatabase.FindUserOrNull(id);
        }

        public bool DeleteUser(Guid id)
        {
            return userDatabase.DeleteUser(id);
        }

        public bool UsernameExists(string username)
        {
            return userDatabase.UsernameExists(username);
        }

        /// <summary>
        /// Changes a user's password in the database safely; after reloading the database first.
        /// Saves the database in the end.
        /// </summary>
        public bool ChangeUserPasswordSafelyAndSaveDatabase(string username, string newPassword)
        {
            //reload database
            TryLoadUserDatabase();

            bool success = userDatabase.ChangePassword(username, newPassword);
            if (!success) { return false; }

            TrySaveUserDatabase();

            return true;
        }

        private void SaveUserDatabase()
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Encryption
            try
            {
                using (var fs = new FileStream(databaseFile, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(Security.desKey, Security.desIV), CryptoStreamMode.Write))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    // This is where you serialize the class
                    formatter.Serialize(cryptoStream, this.userDatabase);
                }

            //hide file
            File.SetAttributes(databaseFile, FileAttributes.Hidden);
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.WriteLine(e.Message.ToString());
            }
        }

        private void LoadUserDatabase()
        {

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            if (!ServerConnection.Check())
            {
                SuccessfullyLoaded = false;
                return;
            }

            //check to see if database exists
            if (File.Exists(databaseFile))
            {
                try
                {
                // Decryption
                using (var fs = new FileStream(databaseFile, FileMode.Open, FileAccess.Read))
                using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(Security.desKey, Security.desIV), CryptoStreamMode.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    // This is where you deserialize the class
                    userDatabase = (UserDatabase)formatter.Deserialize(cryptoStream);
                }
                    SuccessfullyLoaded = true;
                    SetCorrectCurrentUser();
                }
                catch (IOException e)
                {
                    Debug.WriteLine(e.Message.ToString());
                }

            }
            else
            {
                //database not found, create empty
                userDatabase = new UserDatabase();
                SuccessfullyLoaded = false;
            }
        }

        /// <summary>
        /// Updates the currentUser variable with a correct reference after a LoadUserDatabase has occured.
        /// </summary>
        private void SetCorrectCurrentUser()
        {
            if (currentUser != null)
            {
                User user = this.MatchUserOrNull(currentUser.ID);
                if (user != null)
                {
                    currentUser = user;
                }
            }
        }
    }
}
