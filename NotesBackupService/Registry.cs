using Microsoft.Win32;
using System.Reflection;


namespace NotesBackupService
{
    /// <summary>
    /// Holds the list of registry keys used by the application as fields.
    /// </summary>
    public class RegistryEntry
    {
        public static readonly string LastBackupDate = "LastBackupDate";
        public static readonly string ErrorMessage = "ErrorMessage";
        public static readonly string ErrorStackTrace = "ErrorStackTrace";
        public static readonly string ServiceStoppedOn = "ServiceStoppedOn";
    }

    public class RegistryFunctions
    {
        public static string appRegistryKey = @"SOFTWARE\\Notes Backup Service";
        private static RegistryKey applicationRegistryKey;

        /// <summary>Returns a <c>RegistryKey</c> of the app's registry key. Creates it if it doesn't exist.</summary>
        public static RegistryKey GetAppRegistryKey()
        {
            //if the registry key is not cached already,
            //go ahead and cache it now
            //otherwise, return the cached value
            if (applicationRegistryKey is null)
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(appRegistryKey, true);
                if (rk is null)
                {
                    Registry.CurrentUser.CreateSubKey(appRegistryKey);
                }
                applicationRegistryKey = Registry.CurrentUser.OpenSubKey(appRegistryKey, true);
            }

            return applicationRegistryKey;
        }

        /// <summary>
        /// Returns the value of the <paramref name="key"/> specified from the
        /// application's registry path.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRegistryKeyValue(string key)
        {
            RegistryKey rk = GetAppRegistryKey();
            object value = rk.GetValue(key);
            if (value is null)
            {
                return null;
            }

            return value.ToString();
        }

        public static void SetRegistryKeyValue(string key, string value)
        {
            RegistryKey rk = GetAppRegistryKey();
            rk.SetValue(key, value);
        }

        public static void DeleteRegistryKeyValue(string key)
        {
            RegistryKey rk = GetAppRegistryKey();
            object value = rk.GetValue(key);
            if (value != null)  //if key exists
            {
                rk.DeleteValue(key);
            }
        }

        public static void ClearRegistryValues()
        {
            DeleteRegistryKeyValue(RegistryEntry.ErrorMessage);
            DeleteRegistryKeyValue(RegistryEntry.ErrorStackTrace);
        }
    }
}
