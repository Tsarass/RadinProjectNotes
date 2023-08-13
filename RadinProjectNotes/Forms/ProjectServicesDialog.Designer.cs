namespace RadinProjectNotes
{
    partial class ProjectServicesDialog
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.btnAddCategory = new System.Windows.Forms.Button();
            this.btnRemoveCategory = new System.Windows.Forms.Button();
            this.btnAddService = new System.Windows.Forms.Button();
            this.btnRemoveService = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstCategories = new System.Windows.Forms.ListBox();
            this.txtNewCategory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstServices = new System.Windows.Forms.ListBox();
            this.txtNewService = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(344, 209);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 32);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.toolTips.SetToolTip(this.btnCancel, "Close the dialog.");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(255, 209);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(83, 32);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.toolTips.SetToolTip(this.btnOK, "Save any changes to the services and close the dialog.");
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.Location = new System.Drawing.Point(154, 149);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(25, 23);
            this.btnAddCategory.TabIndex = 7;
            this.btnAddCategory.Text = "+";
            this.toolTips.SetToolTip(this.btnAddCategory, "Add a new service category.");
            this.btnAddCategory.UseVisualStyleBackColor = true;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // btnRemoveCategory
            // 
            this.btnRemoveCategory.Location = new System.Drawing.Point(185, 25);
            this.btnRemoveCategory.Name = "btnRemoveCategory";
            this.btnRemoveCategory.Size = new System.Drawing.Size(25, 23);
            this.btnRemoveCategory.TabIndex = 7;
            this.btnRemoveCategory.Text = "-";
            this.toolTips.SetToolTip(this.btnRemoveCategory, "Remove selected service category.");
            this.btnRemoveCategory.UseVisualStyleBackColor = true;
            this.btnRemoveCategory.Click += new System.EventHandler(this.btnRemoveCategory_Click);
            // 
            // btnAddService
            // 
            this.btnAddService.Location = new System.Drawing.Point(373, 175);
            this.btnAddService.Name = "btnAddService";
            this.btnAddService.Size = new System.Drawing.Size(25, 23);
            this.btnAddService.TabIndex = 7;
            this.btnAddService.Text = "+";
            this.toolTips.SetToolTip(this.btnAddService, "Add a new service.");
            this.btnAddService.UseVisualStyleBackColor = true;
            this.btnAddService.Click += new System.EventHandler(this.btnAddService_Click);
            // 
            // btnRemoveService
            // 
            this.btnRemoveService.Location = new System.Drawing.Point(404, 25);
            this.btnRemoveService.Name = "btnRemoveService";
            this.btnRemoveService.Size = new System.Drawing.Size(25, 23);
            this.btnRemoveService.TabIndex = 7;
            this.btnRemoveService.Text = "-";
            this.toolTips.SetToolTip(this.btnRemoveService, "Remove selected service.");
            this.btnRemoveService.UseVisualStyleBackColor = true;
            this.btnRemoveService.Click += new System.EventHandler(this.btnRemoveService_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Service categories";
            // 
            // lstCategories
            // 
            this.lstCategories.FormattingEnabled = true;
            this.lstCategories.Location = new System.Drawing.Point(15, 25);
            this.lstCategories.Name = "lstCategories";
            this.lstCategories.Size = new System.Drawing.Size(164, 121);
            this.lstCategories.TabIndex = 6;
            this.lstCategories.SelectedIndexChanged += new System.EventHandler(this.lstCategories_SelectedIndexChanged);
            // 
            // txtNewCategory
            // 
            this.txtNewCategory.Location = new System.Drawing.Point(15, 152);
            this.txtNewCategory.Name = "txtNewCategory";
            this.txtNewCategory.Size = new System.Drawing.Size(133, 20);
            this.txtNewCategory.TabIndex = 8;
            this.txtNewCategory.TextChanged += new System.EventHandler(this.txtNewCategory_TextChanged);
            this.txtNewCategory.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNewCategory_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(231, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Services";
            // 
            // lstServices
            // 
            this.lstServices.FormattingEnabled = true;
            this.lstServices.Location = new System.Drawing.Point(234, 25);
            this.lstServices.Name = "lstServices";
            this.lstServices.Size = new System.Drawing.Size(164, 147);
            this.lstServices.TabIndex = 6;
            this.lstServices.SelectedIndexChanged += new System.EventHandler(this.lstServices_SelectedIndexChanged);
            // 
            // txtNewService
            // 
            this.txtNewService.Location = new System.Drawing.Point(234, 178);
            this.txtNewService.Name = "txtNewService";
            this.txtNewService.Size = new System.Drawing.Size(133, 20);
            this.txtNewService.TabIndex = 8;
            this.txtNewService.TextChanged += new System.EventHandler(this.txtNewService_TextChanged);
            this.txtNewService.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNewService_KeyUp);
            // 
            // ProjectServicesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 254);
            this.Controls.Add(this.txtNewService);
            this.Controls.Add(this.txtNewCategory);
            this.Controls.Add(this.btnAddService);
            this.Controls.Add(this.btnRemoveService);
            this.Controls.Add(this.btnRemoveCategory);
            this.Controls.Add(this.btnAddCategory);
            this.Controls.Add(this.lstServices);
            this.Controls.Add(this.lstCategories);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ProjectServicesDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project services";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstCategories;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.TextBox txtNewCategory;
        private System.Windows.Forms.Button btnRemoveCategory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstServices;
        private System.Windows.Forms.Button btnAddService;
        private System.Windows.Forms.TextBox txtNewService;
        private System.Windows.Forms.Button btnRemoveService;
    }
}