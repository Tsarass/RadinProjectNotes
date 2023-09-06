using RadinProjectNotesCommon.DueItems;
using RadinProjectNotes.Forms;
using RadinProjectNotes.HelperClasses;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RadinProjectNotesCommon;

namespace RadinProjectNotes
{
    public partial class AddDueItem : Form
    {
        DueItem _editDueItem;

        public AddDueItem(DueItem editDueItem = null)
        {
            InitializeComponent();

            _editDueItem = editDueItem;

            if (editDueItem != null )
            {
                setFromExisting(editDueItem);
                btnAddDueItem.Text = "Edit due item";
            }
            else
            {
                SetEmailsFromGroups();
                lblStatus.Visible = false;
                txtStatus.Visible = false;
            }
        }

        /// <summary>
        /// Set the emails in the email list from the saved email groups.
        /// </summary>
        private void SetEmailsFromGroups()
        {
            string emailGroups = RegistryFunctions.GetRegistryKeyValue(RegistryEntry.EmailGroups);
            if (!string.IsNullOrEmpty(emailGroups) )
            {
                foreach (var email in emailGroups.Split(','))
                {
                    lstEmails.Items.Add(email);
                }
            }
        }

        public DueItem SavedDueItem { get; set; }
        public DueItemState EditedDueItemState { get; set; }

        /// <summary>
        /// Set the form control data according to an existing due item.
        /// </summary>
        /// <param name="editDueItem"></param>
        private void setFromExisting(DueItem editDueItem)
        {
            txtDescription.Text = editDueItem.Description;
            dateTimePicker1.Value = editDueItem.DueDate.ToUniversalTime();
            editDueItem.EmailsToBeNotified.ForEach(a => lstEmails.Items.Add(a));
            txtStatus.Text = editDueItem.GetStatusText();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAddDueItem_Click(object sender, EventArgs e)
        {
            List<string> emails = new List<string>();
            foreach (string email in lstEmails.Items)
            {
                emails.Add(email);
            }

            // Warn the user if no emails have been entered.
            if (emails.Count == 0)
            {
                var result = MessageBox.Show("Are you sure you want to create this due item without any emails for notification?", "No emails fr notification",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            // Check if we are editing an existing due item.
            if (_editDueItem != null)
            {
                // Change status if the date has changed.
                DueStatus newStatus = _editDueItem.DueStatus;
                if (DueDateChanged(_editDueItem.DueDate, dateTimePicker1.Value.ToUniversalTime()))
                {
                    newStatus = DueStatus.Transferred;
                }

                EditedDueItemState = new DueItemState(txtDescription.Text, dateTimePicker1.Value.ToUniversalTime(),
                    newStatus, emails);
            }
            else
            {
                SavedDueItem = new DueItem(txtDescription.Text, dateTimePicker1.Value.ToUniversalTime(),
                Credentials.Instance.currentUser.ID, emails);
            }

            DialogResult = DialogResult.OK;
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

        private void btnRemoveEmail_Click(object sender, EventArgs e)
        {
            if (lstEmails.SelectedIndex >= 0)
            {
                lstEmails.Items.Remove(lstEmails.SelectedItem.ToString());
            }
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            btnAddDueItem.Enabled = txtDescription.Text.Length > 0;
        }

        private void btnEmailGroups_Click(object sender, EventArgs e)
        {
            EmailGroups dialog = new EmailGroups();
            var dlgResult = dialog.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                lstEmails.Items.Clear();
                dialog.GroupEmails.ForEach(a => lstEmails.Items.Add(a));
            }
        }

        private bool DueDateChanged(DateTime oldDueDate, DateTime newDueDate)
        {
            return !oldDueDate.Equals(newDueDate);
        }
    }
}
