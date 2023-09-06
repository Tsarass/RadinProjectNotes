using Microsoft.Win32;
using System.Reflection;
using System.Windows.Forms;

namespace RadinProjectNotes.HelperClasses
{
    internal class AppSetup
    {
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
