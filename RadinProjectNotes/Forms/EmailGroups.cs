using RadinProjectNotes.HelperClasses;
using RadinProjectNotesCommon;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RadinProjectNotes.Forms
{
    public partial class EmailGroups : Form
    {
        public EmailGroups()
        {
            InitializeComponent();

            string emailGroups = RegistryFunctions.GetRegistryKeyValue(RegistryEntry.EmailGroups);
            if (!string.IsNullOrEmpty(emailGroups))
            {
                foreach (string email in emailGroups.Split(','))
                {
                    lstEmails.Items.Add(email);
                }
            }
        }

        /// <summary>List of emails in the email list.</summary>
        public List<string> GroupEmails { get; set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtNewEmail_TextChanged(object sender, EventArgs e)
        {
            btnAddEmail.Enabled = txtNewEmail.Text.Length > 0;
        }

        private void lstEmails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEmails.SelectedItems.Count > 0)
            {
                btnRemoveEmail.Enabled = true;
            }
            else
            {
                btnRemoveEmail.Enabled = false;
            }
        }

        private void btnAddEmail_Click(object sender, EventArgs e)
        {
            if (txtNewEmail.Text.Length > 0)
            {
                // Verify that email is in the correct format.
                if (!Emails.VerifyEmailFormat(txtNewEmail.Text))
                {
                    MessageBox.Show($"Email address {txtNewEmail.Text} is not valid.", "Invalid email address", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!EmailExistsInList(txtNewEmail.Text))
                {
                    lstEmails.Items.Add(txtNewEmail.Text);
                }
            }
        }

        /// <summary>
        /// Check if an email exists in the list of emails.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool EmailExistsInList(string email)
        {
            foreach (string listItem in lstEmails.Items)
            {
                if (listItem == email)
                {
                    return true;
                }
            }

            return false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GroupEmails = new List<string>();

            foreach (string email in lstEmails.Items)
            {
                GroupEmails.Add(email);
            }

            string registryentry = string.Join(",", GroupEmails);
            RegistryFunctions.SetRegistryKeyValue(RegistryEntry.EmailGroups, registryentry);

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnRemoveEmail_Click(object sender, EventArgs e)
        {
            if (lstEmails.SelectedIndex >= 0)
            {
                lstEmails.Items.Remove(lstEmails.SelectedItem.ToString());
            }
        }
    }
}
