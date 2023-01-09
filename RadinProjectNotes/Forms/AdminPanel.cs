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
            lblUnsaved.Text = "";
            LoadDatabase();
        }

        private void LoadDatabase()
        {

            Credentials.Instance.TryLoadUserDatabase();

            if (!Credentials.Instance.SuccessfullyLoaded)
            {
                return;
            }

            listView1.Items.Clear();
            foreach (User user in Credentials.Instance.userDatabase)
            {
                var item = listView1.Items.Add(user.ID.ToString());
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
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            Guid guid = new Guid(listView1.SelectedItems[0].Text);

            if (guid == Credentials.Instance.currentUser.ID)
            {
                MessageBox.Show(this, "Can't delete current user!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = listView1.SelectedItems[0].SubItems[1].Text;

            DialogResult result = MessageBox.Show(this, "Are you sure you want to delete user " + username + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                
                bool deleted = Credentials.Instance.DeleteUser(guid);
                if (deleted)
                {
                    listView1.SelectedItems[0].Remove();
                }

                SetSaveStatus(saved: false);
            }
        }

        private void addUserBtn_Click(object sender, EventArgs e)
        {

            if (textBox2.Text.Length < 6)
            {
                MessageBox.Show("Minimum 6 characters for password.", "Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (textBox1.Text.Length < 5)
            {
                MessageBox.Show("Minimum 5 characters for username.", "Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //permissions
            //default permissions
            Permissions toSet = Permissions.Normal;

            if (comboBox1.SelectedIndex >= 0)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case (0):
                        toSet = Permissions.Low;
                        break;
                    case (1):
                        toSet = Permissions.Normal;
                        break;
                    case (2):
                        toSet = Permissions.All;
                        break;
                }
            }

            User newUser = Credentials.Instance.userDatabase.AddUser(textBox1.Text,textBox2.Text,toSet);
            var item = listView1.Items.Add(newUser.ID.ToString());
            item.SubItems.Add(newUser.username);
            item.SubItems.Add(newUser.Password);
            string adminText = newUser.IsAdmin ? "Yes" : "No";
            item.SubItems.Add(adminText);
            item.SubItems.Add(newUser.LocalLastLoginDate);

            SetSaveStatus(saved: false);
        }

        private void btnChangePermissions_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            //get selected user's Guid
            Guid guidOfSelectedUser = new Guid(listView1.SelectedItems[0].Text);

            if (comboBox1.SelectedIndex < 0)
            {
                return;
            }

            Permissions toSet = Permissions.Normal;

            //if selected user is the current user (admin),
            //reset to admin
            if (Credentials.Instance.currentUser.ID == guidOfSelectedUser)
            {
                toSet = Permissions.All;
            }
            else
            {
                switch (comboBox1.SelectedIndex)
                {
                    case (0):
                        toSet = Permissions.Low;
                        break;
                    case (1):
                        toSet = Permissions.Normal;
                        break;
                    case (2):
                        toSet = Permissions.All;
                        break;
                }
            }

            User userFromSelectedGuid;
            try
            {
                userFromSelectedGuid = Credentials.Instance.userDatabase.SetUserPermissions(guidOfSelectedUser, toSet);
            }
            catch (UserDatabase.UserNotFound ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string adminText = userFromSelectedGuid.IsAdmin ? "Yes" : "No";
            listView1.SelectedItems[0].SubItems[3].Text = adminText;

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
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            if (textBox3.Text.Length < 5)
            {
                MessageBox.Show("Minimum 5 characters for username.", "Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //get selected user's Guid
            Guid guid = new Guid(listView1.SelectedItems[0].Text);
            User user;
            try
            {
                user = Credentials.Instance.FindUserById(guid);
            }
            catch (UserDatabase.UserNotFound ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            user.username = textBox3.Text;
            listView1.SelectedItems[0].SubItems[1].Text = textBox3.Text;

            SetSaveStatus(saved: false);

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
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            if (textBox4.Text.Length < 6)
            {
                MessageBox.Show("Minimum 6 characters for password.", "Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //get selected user's Guid
            Guid guid = new Guid(listView1.SelectedItems[0].Text);
            User user;
            try
            {
                user = Credentials.Instance.FindUserById(guid);
            }
            catch (UserDatabase.UserNotFound ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string password = textBox4.Text;
            Credentials.Instance.userDatabase.ChangePassword(user.username, password);
            listView1.SelectedItems[0].SubItems[2].Text = user.Password;

            SetSaveStatus(saved: false);

        }

        private void AdminPanel_FormClosing(object sender, FormClosingEventArgs e)
        {

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
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            var result = MessageBox.Show("This will reset the selected user's password. He will be prompted to enter a new password at next login. Proceed?", "Reset user password",MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                //get selected user's Guid
                Guid guid = new Guid(listView1.SelectedItems[0].Text);
                User user;
                try
                {
                    user = Credentials.Instance.FindUserById(guid);
                }
                catch (UserDatabase.UserNotFound ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                user.ResetPassword();
                listView1.SelectedItems[0].SubItems[2].Text = user.Password;

                SetSaveStatus(saved: false);

            }
        }
    }
}
