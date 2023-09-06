namespace RadinProjectNotes.Forms
{
    partial class EmailGroups
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNewEmail = new System.Windows.Forms.TextBox();
            this.btnRemoveEmail = new System.Windows.Forms.Button();
            this.btnAddEmail = new System.Windows.Forms.Button();
            this.lstEmails = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtNewEmail
            // 
            this.txtNewEmail.Location = new System.Drawing.Point(12, 87);
            this.txtNewEmail.Name = "txtNewEmail";
            this.txtNewEmail.Size = new System.Drawing.Size(219, 20);
            this.txtNewEmail.TabIndex = 16;
            this.txtNewEmail.TextChanged += new System.EventHandler(this.txtNewEmail_TextChanged);
            // 
            // btnRemoveEmail
            // 
            this.btnRemoveEmail.Enabled = false;
            this.btnRemoveEmail.Location = new System.Drawing.Point(268, 84);
            this.btnRemoveEmail.Name = "btnRemoveEmail";
            this.btnRemoveEmail.Size = new System.Drawing.Size(25, 25);
            this.btnRemoveEmail.TabIndex = 14;
            this.btnRemoveEmail.Text = "-";
            this.btnRemoveEmail.UseVisualStyleBackColor = true;
            this.btnRemoveEmail.Click += new System.EventHandler(this.btnRemoveEmail_Click);
            // 
            // btnAddEmail
            // 
            this.btnAddEmail.Location = new System.Drawing.Point(237, 84);
            this.btnAddEmail.Name = "btnAddEmail";
            this.btnAddEmail.Size = new System.Drawing.Size(25, 25);
            this.btnAddEmail.TabIndex = 15;
            this.btnAddEmail.Text = "+";
            this.btnAddEmail.UseVisualStyleBackColor = true;
            this.btnAddEmail.Click += new System.EventHandler(this.btnAddEmail_Click);
            // 
            // lstEmails
            // 
            this.lstEmails.FormattingEnabled = true;
            this.lstEmails.Location = new System.Drawing.Point(12, 12);
            this.lstEmails.Name = "lstEmails";
            this.lstEmails.Size = new System.Drawing.Size(281, 69);
            this.lstEmails.TabIndex = 13;
            this.lstEmails.SelectedIndexChanged += new System.EventHandler(this.lstEmails_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(199, 120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 30);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(99, 120);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(94, 30);
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // EmailGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 162);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtNewEmail);
            this.Controls.Add(this.btnRemoveEmail);
            this.Controls.Add(this.btnAddEmail);
            this.Controls.Add(this.lstEmails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EmailGroups";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Email groups";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNewEmail;
        private System.Windows.Forms.Button btnRemoveEmail;
        private System.Windows.Forms.Button btnAddEmail;
        private System.Windows.Forms.ListBox lstEmails;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}