using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            lstPermissions.Columns[0].Width = -1;
            PopulatePermissionsListView();

            lblUnsaved.Text = "";
            LoadDatabase();
        }

        private void PopulatePermissionsListView()
        {
            var permissions = Enum.GetValues(typeof(Permissions));
            foreach (var permission in permissions) 
            {
                lstPermissions.Items.Add(permission.ToString());
            }
        }

        private void LoadDatabase()
        {

            Credentials.Instance.TryLoadUserDatabase();

            if (!Credentials.Instance.SuccessfullyLoaded)
            {
                return;
            }

            lstUsers.Items.Clear();
            foreach (User user in Credentials.Instance.userDatabase)
            {
                var item = lstUsers.Items.Add(user.ID.ToString());
                item.SubItems.Add(user.username);
                item.SubItems.Add(user.Password);
                string adminText = user.IsAdmin ? "Yes" : "No";
                item.SubItems.Add(adminText);
                item.SubItems.Add(user.LocalLastLoginDate);
                item.SubItems.Add(user.displayName);
                item.SubItems.Add(user.appVersion);
            }
            
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count <= 0)
            {
                return;
            }

            Guid guid = new Guid(lstUsers.SelectedItems[0].Text);

            if (guid == Credentials.Instance.currentUser.ID)
            {
                MessageBox.Show(this, "Can't delete current user!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = lstUsers.SelectedItems[0].SubItems[1].Text;

            DialogResult result = MessageBox.Show(this, "Are you sure you want to delete user " + username + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                
                bool deleted = Credentials.Instance.DeleteUser(guid);
                if (deleted)
                {
                    lstUsers.SelectedItems[0].Remove();
                }

                SetSaveStatus(saved: false);
            }
        }

        private void addUserBtn_Click(object sender, EventArgs e)
        {

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Minimum 6 characters for password.", "Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtUsername.Text.Length < 5)
            {
                MessageBox.Show("Minimum 5 characters for username.", "Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //permissions
            //default permissions
            Permissions newUserPermissions = 0;

            foreach (ListViewItem lstItem in lstPermissions.Items)
            {
                if (lstItem.Checked)
                {
                    var enumValue = Enum.TryParse(lstItem.Text, out Permissions parsedEnum);
                    newUserPermissions |= parsedEnum;
                }
            }

            User newUser = Credentials.Instance.userDatabase.AddUser(txtUsername.Text,txtPassword.Text,newUserPermissions);
            var item = lstUsers.Items.Add(newUser.ID.ToString());
            item.SubItems.Add(newUser.username);
            item.SubItems.Add(newUser.Password);
            string adminText = newUser.IsAdmin ? "Yes" : "No";
            item.SubItems.Add(adminText);
            item.SubItems.Add(newUser.LocalLastLoginDate);

            SetSaveStatus(saved: false);
        }

        private void btnChangePermissions_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count <= 0)
            {
                return;
            }

            //get selected user's Guid
            Guid guidOfSelectedUser = new Guid(lstUsers.SelectedItems[0].Text);

            Permissions newPermissions = 0;

            //if selected user is the current user (admin),
            //reset to admin
            if (Credentials.Instance.currentUser.ID == guidOfSelectedUser)
            {
                newPermissions = Credentials.getAdminPermissions();
            }
            else
            {
                foreach (ListViewItem lstItem in lstPermissions.Items)
                {
                    if (lstItem.Checked)
                    {
                        var enumValue = Enum.TryParse(lstItem.Text, out Permissions parsedEnum);
                        newPermissions |= parsedEnum;
                    }
                }
            }

            User userFromSelectedGuid;
            try
            {
                userFromSelectedGuid = Credentials.Instance.userDatabase.SetUserPermissions(guidOfSelectedUser, newPermissions);
            }
            catch (UserDatabase.UserNotFound ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string adminText = userFromSelectedGuid.IsAdmin ? "Yes" : "No";
            lstUsers.SelectedItems[0].SubItems[3].Text = adminText;

            SetSaveStatus(saved: false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to save changes to the database?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Credentials.Instance.TrySaveUserDatabase();
                this.Close();
            }
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Discard changes and close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Credentials.Instance.TryLoadUserDatabase();
            }
        }

        private void btnChangeUsername_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count <= 0)
            {
                return;
            }

            if (txtChangeUsername.Text.Length < 5)
            {
                MessageBox.Show("Minimum 5 characters for username.", "Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User user = getSelectedUser();
            if (user is null) return;

            user.username = txtChangeUsername.Text;
            lstUsers.SelectedItems[0].SubItems[1].Text = txtChangeUsername.Text;

            SetSaveStatus(saved: false);
        }

        /// <summary>
        /// Find the user selected in the list view in the database.
        /// </summary>
        /// <returns></returns>
        private User getSelectedUser()
        {
            if (lstUsers.SelectedItems.Count <= 0)
            {
                return null;
            }

            Guid guid = new Guid(lstUsers.SelectedItems[0].Text);
 
            try
            {
                return Credentials.Instance.FindUserById(guid);
            }
            catch (UserDatabase.UserNotFound ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to refresh the database? Any changes will be lost", "Refresh database", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                LoadDatabase();

                SetSaveStatus(saved: true);
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count <= 0)
            {
                return;
            }

            if (txtChangePassword.Text.Length < 6)
            {
                MessageBox.Show("Minimum 6 characters for password.", "Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User user = getSelectedUser();
            if (user is null) return;

            string password = txtChangePassword.Text;
            Credentials.Instance.userDatabase.ChangePassword(user.username, password);
            lstUsers.SelectedItems[0].SubItems[2].Text = user.Password;

            SetSaveStatus(saved: false);

        }

        private void SetSaveStatus(bool saved)
        {
            if (saved)
            {
                lblUnsaved.Text = "";
                btnSave.Enabled = false;
            }
            else
            {
                lblUnsaved.Text = "Database changed. Changes not automatically saved.";
                btnSave.Enabled = true;
            }
        }

        private void resetPasswordBtn_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count <= 0)
            {
                return;
            }

            var result = MessageBox.Show("This will reset the selected user's password. He will be prompted to enter a new password at next login. Proceed?", "Reset user password",MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                User user = getSelectedUser();
                if (user is null) return;

                user.ResetPassword();
                lstUsers.SelectedItems[0].SubItems[2].Text = user.Password;

                SetSaveStatus(saved: false);

            }
        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count <= 0)
            {
                return;
            }

            User user = getSelectedUser();
            if (user is null) return;

            txtChangeUsername.Text = user.username;

            // Set the permissions checkboxes according to user permissions.
            ResetPermissionsItems();
            var permissions = Enum.GetValues(typeof(Permissions));
            foreach (Permissions permission in permissions)
            {
                if (user.permissions.HasFlag(permission))
                {
                    foreach (ListViewItem item in lstPermissions.Items)
                    {
                        if (item.Text == permission.ToString())
                        {
                            item.Checked = true;
                        }
                    }
                }
            }


        }

        /// <summary>
        /// Set all permissions checkboxes in the list view to false.
        /// </summary>
        private void ResetPermissionsItems()
        {
            foreach (ListViewItem item in lstPermissions.Items)
            {
                item.Checked = false;
            }
        }
    }
}
