﻿using Microsoft.Win32;
using RadinProjectNotes.HelperClasses;
using RadinProjectNotesCommon;
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

namespace RadinProjectNotes
{
    public partial class UserLogin : Form
    {
        public UserLogin()
        {
            InitializeComponent();
        }

        private bool resetCredentialsOnLoad = false;    //used when logout is clicked

        public DialogResult ShowDialog(IWin32Window owner, bool resetCredentialsOnLoad)
        {
            this.resetCredentialsOnLoad = resetCredentialsOnLoad;
            return ShowDialog(owner);
        }

        private void UserLogin_Load(object sender, EventArgs e)
        {
            if (resetCredentialsOnLoad)
            {
                txtUsername.Text = "";
                txtPassword.Text = "";
                chkRemember.Checked = false;
            }
            else
            {
                //check registry for auto login

                string username = RegistryFunctions.GetRegistryKeyValue(RegistryEntry.Username);
                if ((username != null) && (username != String.Empty))
                {
                    txtUsername.Text = username;
                }
                else
                {
                    txtUsername.Text = Environment.UserName;
                }

                string autologin = RegistryFunctions.GetRegistryKeyValue(RegistryEntry.AutoLogin);
                if (autologin == "1")
                {
                    string password = RegistryFunctions.GetRegistryKeyValue(RegistryEntry.Password);
                    if (password != null)
                    {
                        txtPassword.Text = password.ToString();
                        btnLogin_Click(this, new EventArgs());
                    }

                    chkRemember.Checked = true;
                }
                else
                {
                    this.Visible = true;
                    chkRemember.Checked = false;
                }
            }

            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //check if credentials was successfully loaded
            if (!Credentials.Instance.SuccessfullyLoaded)
            {
                //try to load it
                Credentials.Instance.TryLoadUserDatabase();

                if (!Credentials.Instance.SuccessfullyLoaded)
                {
                    MessageBox.Show("Could not retrieve user database. Check connection and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            //check credentials
            User user;
            try
            {
                user = Credentials.Instance.LogInUser(txtUsername.Text, txtPassword.Text);
            }
            catch (UserDatabase.InvalidUsernamePassword ex)
            {
                this.Visible = true;
                MessageBox.Show(this, ex.Message.ToString(), "Invalid login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (UserDatabase.UserHasResetPassword)
            {
                if (ChangePasswordThroughResetDialog(txtUsername.Text))
                {
                    //recall Sign in with new password
                    btnLogin_Click(sender, e);
                }
                else
                {
                    //stay stuck in login form
                    this.Visible = true;
                    return;
                }
            }

            //save the last login time and app version info 
            Credentials.Instance.TrySaveUserDatabase();

            if (chkRemember.Checked)
            {
                SaveLoginCredentials();
            }
            else
            {
                ResetLoginCredentials();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
            
        }

        public bool ChangePasswordThroughResetDialog(string username)
        {
            //show reset password form
            ResetPassword frm = new ResetPassword
            {
                Username = username
            };
            frm.ShowDialog(this);

            if (frm.DialogResult == DialogResult.OK)
            {
                txtPassword.Text = frm.Password;
            }
            else
            {
                return false;
            }

            return true;
        }

        public void SaveLoginCredentials()
        {
            RegistryFunctions.SetRegistryKeyValue(RegistryEntry.AutoLogin, "1");
            RegistryFunctions.SetRegistryKeyValue(RegistryEntry.Username, Credentials.Instance.currentUser.username);
            RegistryFunctions.SetRegistryKeyValue(RegistryEntry.Password, Credentials.Instance.currentUser.Password);
        }

        public static void ResetLoginCredentials()
        {
            RegistryFunctions.SetRegistryKeyValue(RegistryEntry.AutoLogin, "0");
            RegistryFunctions.SetRegistryKeyValue(RegistryEntry.Username, Credentials.Instance.currentUser.username);
            RegistryFunctions.DeleteRegistryKeyValue(RegistryEntry.Password);
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

        private void linkSignup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //load signup form
            UserSignup frm = new UserSignup();
            frm.ShowDialog(this);

            if (frm.DialogResult == DialogResult.OK)
            {
                //signup successfull
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if ((txtPassword.Text.Length < 6) || (txtUsername.Text.Length < 5))
            {
                btnLogin.Enabled = false;
            }
            else
            {
                btnLogin.Enabled = true;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if ((txtPassword.Text.Length < 6) || (txtUsername.Text.Length < 5))
            {
                btnLogin.Enabled = false;
            }
            else
            {
                btnLogin.Enabled = true;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Please contact an administrator to reset your password.","Reset password",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
