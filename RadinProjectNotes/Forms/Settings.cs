using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using RadinProjectNotes.HelperClasses;

namespace RadinProjectNotes
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            if (RegistryFunctions.GetRegistryKeyValue(RegistryEntry.StartWithWindows) == "1")
            {
                chkStartup.Checked = true;
            }
            else
            {
                chkStartup.Checked = false;
            }

            if (RegistryFunctions.GetRegistryKeyValue(RegistryEntry.MinimizeToSystemTray) == "1")
            {
                chkTray.Checked = true;
            }
            else
            {
                chkTray.Checked = false;
            }

            if (RegistryFunctions.GetRegistryKeyValue(RegistryEntry.MinimizeInsteadOfProgramExit) == "1")
            {
                chkMinimize.Checked = true;
            }
            else
            {
                chkMinimize.Checked = false;
            }

            txtDisplayName.Text = ServerConnection.credentials.currentUser.displayName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (chkStartup.Checked)
            {
                RegistryFunctions.SetRegistryKeyValue(RegistryEntry.StartWithWindows, "1");
                RegistryFunctions.SetAppToRunOnStartup(runOnStartup: true);
            }
            else
            {
                RegistryFunctions.SetRegistryKeyValue(RegistryEntry.StartWithWindows, "0");
                RegistryFunctions.SetAppToRunOnStartup(runOnStartup: false);
            }

            if (chkTray.Checked)
            {
                RegistryFunctions.SetRegistryKeyValue(RegistryEntry.MinimizeToSystemTray, "1");
            }
            else
            {
                RegistryFunctions.SetRegistryKeyValue(RegistryEntry.MinimizeToSystemTray, "0");
            }

            if (chkMinimize.Checked)
            {
                RegistryFunctions.SetRegistryKeyValue(RegistryEntry.MinimizeInsteadOfProgramExit, "1");
            }
            else
            {
                RegistryFunctions.SetRegistryKeyValue(RegistryEntry.MinimizeInsteadOfProgramExit, "0");
            }


            this.DialogResult = DialogResult.OK;

            this.Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChangeDisplayName_Click(object sender, EventArgs e)
        {
            if (txtDisplayName.Text.Length < 5)
            {
                MessageBox.Show("Minimum 5 characters for display name.", "Display name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //reload user database
            ServerConnection.credentials.TryLoadUserDatabase();

            ServerConnection.credentials.currentUser.displayName = txtDisplayName.Text;
            MessageBox.Show("Your display name has been updated to " + txtDisplayName.Text + ".", "Display name", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //save the last login time info
            ServerConnection.credentials.TrySaveUserDatabase();
        }
    }
}
