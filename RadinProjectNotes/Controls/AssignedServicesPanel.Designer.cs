namespace RadinProjectNotes.Controls
{
    partial class AssignedServicesPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.servicePanel = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // servicePanel
            // 
            this.servicePanel.BackColor = System.Drawing.Color.White;
            this.servicePanel.ColumnCount = 1;
            this.servicePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.servicePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.servicePanel.Location = new System.Drawing.Point(0, 0);
            this.servicePanel.Name = "servicePanel";
            this.servicePanel.RowCount = 1;
            this.servicePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.servicePanel.Size = new System.Drawing.Size(545, 271);
            this.servicePanel.TabIndex = 6;
            // 
            // AssignedServicesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.servicePanel);
            this.Name = "AssignedServicesPanel";
            this.Size = new System.Drawing.Size(545, 271);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel servicePanel;
    }
}
