namespace RadinProjectNotes
{
    partial class CommentPackage
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
            this.components = new System.ComponentModel.Container();
            this.lblCommentTitle = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.boxCommentPreview = new System.Windows.Forms.TextBox();
            this.attachmentMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.splitButton1 = new SplitButtonDemo.SplitButton();
            this.SuspendLayout();
            // 
            // lblCommentTitle
            // 
            this.lblCommentTitle.AutoSize = true;
            this.lblCommentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lblCommentTitle.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblCommentTitle.Location = new System.Drawing.Point(3, 4);
            this.lblCommentTitle.Name = "lblCommentTitle";
            this.lblCommentTitle.Size = new System.Drawing.Size(100, 15);
            this.lblCommentTitle.TabIndex = 2;
            this.lblCommentTitle.Text = "Comment Title";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackgroundImage = global::RadinProjectNotes.Properties.Resources.delete;
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Location = new System.Drawing.Point(310, 52);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(26, 26);
            this.btnDelete.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnDelete, "Delete comment.");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.BackgroundImage = global::RadinProjectNotes.Properties.Resources.edit;
            this.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEdit.Location = new System.Drawing.Point(310, 20);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(26, 26);
            this.btnEdit.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnEdit, "Edit comment.");
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // boxCommentPreview
            // 
            this.boxCommentPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.boxCommentPreview.BackColor = System.Drawing.SystemColors.Window;
            this.boxCommentPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boxCommentPreview.Font = new System.Drawing.Font("Calibri", 12F);
            this.boxCommentPreview.Location = new System.Drawing.Point(3, 24);
            this.boxCommentPreview.Multiline = true;
            this.boxCommentPreview.Name = "boxCommentPreview";
            this.boxCommentPreview.ReadOnly = true;
            this.boxCommentPreview.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.boxCommentPreview.Size = new System.Drawing.Size(301, 93);
            this.boxCommentPreview.TabIndex = 4;
            // 
            // attachmentMenuStrip
            // 
            this.attachmentMenuStrip.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attachmentMenuStrip.Name = "attachmentMenuStrip";
            this.attachmentMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // splitButton1
            // 
            this.splitButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.splitButton1.ClickedImage = "Clicked";
            this.splitButton1.DisabledImage = "Disabled";
            this.splitButton1.FocusedImage = "Focused";
            this.splitButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitButton1.HoverImage = "Hover";
            this.splitButton1.ImageKey = "Normal";
            this.splitButton1.Location = new System.Drawing.Point(174, 0);
            this.splitButton1.Name = "splitButton1";
            this.splitButton1.NormalImage = "Normal";
            this.splitButton1.Size = new System.Drawing.Size(130, 22);
            this.splitButton1.TabIndex = 5;
            this.splitButton1.Text = "Attachments";
            this.splitButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.splitButton1.UseVisualStyleBackColor = true;
            this.splitButton1.ButtonClick += new System.EventHandler(this.splitButton1_ButtonClick);
            // 
            // CommentPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.splitButton1);
            this.Controls.Add(this.boxCommentPreview);
            this.Controls.Add(this.lblCommentTitle);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(0, 60);
            this.Name = "CommentPackage";
            this.Size = new System.Drawing.Size(342, 120);
            this.Load += new System.EventHandler(this.CommentPackage_Load);
            this.Enter += new System.EventHandler(this.CommentPackage_Enter);
            this.Leave += new System.EventHandler(this.CommentPackage_Leave);
            this.Resize += new System.EventHandler(this.CommentPackage_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label lblCommentTitle;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox boxCommentPreview;
        private SplitButtonDemo.SplitButton splitButton1;
        private System.Windows.Forms.ContextMenuStrip attachmentMenuStrip;
    }
}
