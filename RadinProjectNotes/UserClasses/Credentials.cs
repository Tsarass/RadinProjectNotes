using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
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
        private readonly string databaseFile = Path.Combine(ServerConnection.serverFolder, "users.db");

        public UserDatabase userDatabase;
        public User currentUser;
        public bool SuccessfullyLoaded { get; set; }

        public static Credentials _instance;    //singleton

        public static Credentials Instance
        {
            get
            {
                if (_instance != null) { return _instance; }
                _instance = new Credentials();
                return _instance;
            }
        }

        private Credentials()
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

        public User FindUserById(Guid id)
        {
            return userDatabase.FindUserById(id);
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
        /// Checks the credentials and updates user info when a user tries to logs in.
        /// </summary>
        /// <param name="user"></param>
        public User LogInUser(string username, string password)
        {
            User user = userDatabase.MatchUsernameAndPassword(username, password);
                        
            //update last login info
            user.lastLogin = DateTime.UtcNow.Ticks;
            user.appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Instance.currentUser = user;

            return user;
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
                    RefreshCurrentUser();
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
        private void RefreshCurrentUser()
        {
            if (currentUser != null)
            {
                try
                {
                    User user = this.FindUserById(currentUser.ID);
                    currentUser = user;
                }
                catch (UserDatabase.UserNotFound ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                }
            }
        }
    }
}
