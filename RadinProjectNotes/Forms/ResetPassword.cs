using System;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public partial class ResetPassword : Form
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public ResetPassword()
        {
            InitializeComponent();
        }

        private void ResetPassword_Load(object sender, EventArgs e)
        {
            txtUsername.Text = Username;
            txtPassword.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ServerConnection.Check())
            {
                MessageBox.Show("Could not retrieve user database. Check connection and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //change the user's password safely
            bool success = ServerConnection.credentials.ChangeUserPasswordSafelyAndSaveDatabase(Username, txtPassword.Text);

            if (!success)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                Password = txtPassword.Text;
                this.DialogResult = DialogResult.OK;
            }

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

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }
    }
}
