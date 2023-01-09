using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public partial class UserSignup : Form
    {
        public UserSignup()
        {
            InitializeComponent();
        }

        private void FirstUseSetup_Load(object sender, EventArgs e)
        {
            txtUsername.Text = Environment.UserName;
            txtUsername.Focus();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            if (!ServerConnection.Check())
            {
                MessageBox.Show("Could not retrieve user database. Check connection and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //check if username exists
            Credentials.Instance.TryLoadUserDatabase(); //reload database first
            if (Credentials.Instance.UsernameExists(txtUsername.Text))
            {
                string prompt = "Username " + txtUsername.Text + " already exists. Please enter a different one.";
                MessageBox.Show(this, prompt, "Username exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //save user
            User newUSer = Credentials.Instance.AddUserSafelyAndSaveDatabase(txtUsername.Text, txtPassword.Text, Permissions.Normal);
            Credentials.Instance.currentUser = newUSer;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if ((txtPassword.Text.Length < 6) || (txtUsername.Text.Length < 5))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if ((txtPassword.Text.Length < 6) || (txtUsername.Text.Length < 5))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }
    }
}
