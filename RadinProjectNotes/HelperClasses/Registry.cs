using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes.HelperClasses
{
    /// <summary>
    /// Holds the list of registry keys used by the application as fields.
    /// </summary>
    public class RegistryEntry
    {
        public static readonly string Username = "Username";
        public static readonly string Password = "Password";
        public static readonly string StartWithWindows = "StartWithWindows";
        public static readonly string AutoLogin = "AutoLogin";
        public static readonly string MinimizeToSystemTray = "MinimizeToSystemTray";
        public static readonly string LatestPostsDatabaseLastSize = "LatestPostsDatabaseLastSize";
        public static readonly string MinimizeInsteadOfProgramExit = "MinimizeInsteadOfProgramExit"; 
    }

    public class RegistryFunctions
    {
        public static string appRegistryKey = @"SOFTWARE\\Radin Project Notes";
        private static RegistryKey applicationRegistryKey;

        /// <summary>Returns a <c>RegistryKey</c> of the app's registry key. Creates it if it doesn't exist.</summary>
        public static RegistryKey GetAppRegistryKey()
        {
            //if the registry key is not cached already,
            //go ahead and cache it now
            //otherwise, return the cached value
            if (applicationRegistryKey is null)
            {
                CreateAndCacheAppRegistryKey();
            }

            return applicationRegistryKey;
        }

        /// <summary>
        /// Create and cache the app's registry key.
        /// </summary>
        private static void CreateAndCacheAppRegistryKey()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(appRegistryKey, true);
            if (rk is null)
            {
                Registry.CurrentUser.CreateSubKey(appRegistryKey);
            }
            applicationRegistryKey = Registry.CurrentUser.OpenSubKey(appRegistryKey, true);
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

        public static void SetAppToRunOnStartup(bool runOnStartup = true)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string appName = Assembly.GetExecutingAssembly().GetName().Name;
            if (rk != null)
            {
                object pathRegistered = rk.GetValue(appName);

                if (runOnStartup)
                {
                    rk.SetValue(appName, Application.ExecutablePath + " -minimized");
                }
                else
                {
                    if (pathRegistered != null)
                    {
                        rk.DeleteValue(appName);
                    }
                }
            }
        }
    }
}
