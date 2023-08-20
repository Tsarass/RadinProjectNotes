namespace RadinProjectNotes
{
    partial class AddDueItem
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
            this.btnAddDueItem = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNewEmail = new System.Windows.Forms.TextBox();
            this.btnRemoveEmail = new System.Windows.Forms.Button();
            this.btnAddEmail = new System.Windows.Forms.Button();
            this.lstEmails = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnAddDueItem
            // 
            this.btnAddDueItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddDueItem.Enabled = false;
            this.btnAddDueItem.Location = new System.Drawing.Point(237, 224);
            this.btnAddDueItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddDueItem.Name = "btnAddDueItem";
            this.btnAddDueItem.Size = new System.Drawing.Size(125, 37);
            this.btnAddDueItem.TabIndex = 1;
            this.btnAddDueItem.Text = "Add due item";
            this.btnAddDueItem.UseVisualStyleBackColor = true;
            this.btnAddDueItem.Click += new System.EventHandler(this.btnAddDueItem_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(371, 224);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(125, 37);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(16, 31);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(479, 22);
            this.txtDescription.TabIndex = 3;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(16, 90);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(139, 22);
            this.dateTimePicker1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 70);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Due date";
            // 
            // txtNewEmail
            // 
            this.txtNewEmail.Location = new System.Drawing.Point(237, 178);
            this.txtNewEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNewEmail.Name = "txtNewEmail";
            this.txtNewEmail.Size = new System.Drawing.Size(176, 22);
            this.txtNewEmail.TabIndex = 12;
            this.txtNewEmail.TextChanged += new System.EventHandler(this.txtNewEmail_TextChanged);
            // 
            // btnRemoveEmail
            // 
            this.btnRemoveEmail.Location = new System.Drawing.Point(464, 86);
            this.btnRemoveEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemoveEmail.Name = "btnRemoveEmail";
            this.btnRemoveEmail.Size = new System.Drawing.Size(33, 28);
            this.btnRemoveEmail.TabIndex = 10;
            this.btnRemoveEmail.Text = "-";
            this.btnRemoveEmail.UseVisualStyleBackColor = true;
            this.btnRemoveEmail.Click += new System.EventHandler(this.btnRemoveEmail_Click);
            // 
            // btnAddEmail
            // 
            this.btnAddEmail.Location = new System.Drawing.Point(423, 178);
            this.btnAddEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddEmail.Name = "btnAddEmail";
            this.btnAddEmail.Size = new System.Drawing.Size(33, 28);
            this.btnAddEmail.TabIndex = 11;
            this.btnAddEmail.Text = "+";
            this.btnAddEmail.UseVisualStyleBackColor = true;
            this.btnAddEmail.Click += new System.EventHandler(this.btnAddEmail_Click);
            // 
            // lstEmails
            // 
            this.lstEmails.FormattingEnabled = true;
            this.lstEmails.ItemHeight = 16;
            this.lstEmails.Location = new System.Drawing.Point(237, 86);
            this.lstEmails.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstEmails.Name = "lstEmails";
            this.lstEmails.Size = new System.Drawing.Size(217, 84);
            this.lstEmails.TabIndex = 9;
            this.lstEmails.SelectedIndexChanged += new System.EventHandler(this.lstEmails_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(233, 66);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Emails to be notified";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(16, 135);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(44, 16);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status";
            // 
            // cboStatus
            // 
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(20, 156);
            this.cboStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(160, 24);
            this.cboStatus.TabIndex = 14;
            // 
            // AddDueItem
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 272);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNewEmail);
            this.Controls.Add(this.btnRemoveEmail);
            this.Controls.Add(this.btnAddEmail);
            this.Controls.Add(this.lstEmails);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddDueItem);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AddDueItem";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add due item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAddDueItem;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNewEmail;
        private System.Windows.Forms.Button btnRemoveEmail;
        private System.Windows.Forms.Button btnAddEmail;
        private System.Windows.Forms.ListBox lstEmails;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
    }
}