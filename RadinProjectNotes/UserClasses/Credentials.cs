using EncryptedDatabaseSerializer;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static RadinProjectNotes.UserDatabase;

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
        private EncryptedDatabaseSerializer<UserDatabase> _encryptedDbSerializer;
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

        /// <summary>
        /// Get low user permissions.
        /// </summary>
        public static Permissions getLowPermissions()
        {
            return Permissions.Read;
        }

        /// <summary>
        /// Get normal user permissions.
        /// </summary>
        public static Permissions getNormalPermissions()
        {
            return Permissions.Read | Permissions.Edit | Permissions.Delete | Permissions.AddComment;
        }

        /// <summary>
        /// Get admin user permissions.
        /// </summary>
        public static Permissions getAdminPermissions()
        {
            return getNormalPermissions() | Permissions.Admin;
        }

        private Credentials()
        {
            SuccessfullyLoaded = false;
            userDatabase = CreateEmpty();
            _encryptedDbSerializer = new EncryptedDatabaseSerializer<UserDatabase>(databaseFile);
        }

        /// <summary>
        /// Try to load the user database.
        /// </summary>
        public void TryLoadUserDatabase()
        {
            try
            {
                userDatabase = _encryptedDbSerializer.LoadDatabase();
                SuccessfullyLoaded = true;
            }
            catch (CouldNotLoadDatabase)
            {
                SuccessfullyLoaded = false;
            }
            catch (DatabaseFileNotFound)
            {
                userDatabase = CreateEmpty();
                SuccessfullyLoaded = true;
            }
        }

        /// <summary>
        /// Try to save the user database.
        /// </summary>
        /// <returns>True if the user database could be saved.</returns>
        public bool TrySaveUserDatabase()
        {
            try
            {
                _encryptedDbSerializer.SaveDatabase(userDatabase);
                return true;
            }
            catch (CouldNotSaveDatabase)
            {
                return false;
            }
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

        /// <summary>
        /// Get the display name of the user with the specified id. If the user is not found, a generic string will be returned.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUserDisplayNameById(Guid id)
        {
            try
            {
                return FindUserById(id).displayName;
            }
            catch (UserNotFound)
            {
                return "<User deleted>";
            }
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
