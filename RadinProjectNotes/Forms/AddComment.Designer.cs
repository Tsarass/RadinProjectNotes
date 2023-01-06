namespace RadinProjectNotes
{
    partial class AddComment
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
            this.btnAddComment = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.attachmentImgList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.imageFromPasteBtn = new System.Windows.Forms.Button();
            this.removeAttachmentBtn = new System.Windows.Forms.Button();
            this.addAttachmentBtn = new System.Windows.Forms.Button();
            this.attachmentListView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.updateImageFromPasteBtn = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddComment
            // 
            this.btnAddComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddComment.Enabled = false;
            this.btnAddComment.Location = new System.Drawing.Point(462, 362);
            this.btnAddComment.Name = "btnAddComment";
            this.btnAddComment.Size = new System.Drawing.Size(94, 30);
            this.btnAddComment.TabIndex = 1;
            this.btnAddComment.Text = "Add Comment";
            this.btnAddComment.UseVisualStyleBackColor = true;
            this.btnAddComment.Click += new System.EventHandler(this.btnAddComment_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(562, 362);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // attachmentImgList
            // 
            this.attachmentImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.attachmentImgList.ImageSize = new System.Drawing.Size(24, 24);
            this.attachmentImgList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(644, 344);
            this.splitContainer1.SplitterDistance = 468;
            this.splitContainer1.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(468, 344);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.imageFromPasteBtn);
            this.panel1.Controls.Add(this.removeAttachmentBtn);
            this.panel1.Controls.Add(this.addAttachmentBtn);
            this.panel1.Controls.Add(this.attachmentListView);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(172, 344);
            this.panel1.TabIndex = 10;
            // 
            // imageFromPasteBtn
            // 
            this.imageFromPasteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.imageFromPasteBtn.BackgroundImage = global::RadinProjectNotes.Properties.Resources.add_image;
            this.imageFromPasteBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imageFromPasteBtn.Enabled = false;
            this.imageFromPasteBtn.Location = new System.Drawing.Point(139, 311);
            this.imageFromPasteBtn.Name = "imageFromPasteBtn";
            this.imageFromPasteBtn.Size = new System.Drawing.Size(30, 30);
            this.imageFromPasteBtn.TabIndex = 13;
            this.toolTip1.SetToolTip(this.imageFromPasteBtn, "Create attachment from clipboard image (Ctrl+V).");
            this.imageFromPasteBtn.UseVisualStyleBackColor = true;
            this.imageFromPasteBtn.Click += new System.EventHandler(this.imageFromPasteBtn_Click);
            // 
            // removeAttachmentBtn
            // 
            this.removeAttachmentBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeAttachmentBtn.BackgroundImage = global::RadinProjectNotes.Properties.Resources.remove_link;
            this.removeAttachmentBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.removeAttachmentBtn.Enabled = false;
            this.removeAttachmentBtn.Location = new System.Drawing.Point(50, 311);
            this.removeAttachmentBtn.Name = "removeAttachmentBtn";
            this.removeAttachmentBtn.Size = new System.Drawing.Size(30, 30);
            this.removeAttachmentBtn.TabIndex = 13;
            this.toolTip1.SetToolTip(this.removeAttachmentBtn, "Delete selected attachment(s).");
            this.removeAttachmentBtn.UseVisualStyleBackColor = true;
            this.removeAttachmentBtn.Click += new System.EventHandler(this.removeAttachmentBtn_Click);
            // 
            // addAttachmentBtn
            // 
            this.addAttachmentBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addAttachmentBtn.BackgroundImage = global::RadinProjectNotes.Properties.Resources.add_link;
            this.addAttachmentBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.addAttachmentBtn.Location = new System.Drawing.Point(14, 311);
            this.addAttachmentBtn.Name = "addAttachmentBtn";
            this.addAttachmentBtn.Size = new System.Drawing.Size(30, 30);
            this.addAttachmentBtn.TabIndex = 12;
            this.toolTip1.SetToolTip(this.addAttachmentBtn, "Add file(s) as attachments.");
            this.addAttachmentBtn.UseVisualStyleBackColor = true;
            this.addAttachmentBtn.Click += new System.EventHandler(this.addAttachmentBtn_Click);
            // 
            // attachmentListView
            // 
            this.attachmentListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.attachmentListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.attachmentListView.HideSelection = false;
            this.attachmentListView.LargeImageList = this.attachmentImgList;
            this.attachmentListView.Location = new System.Drawing.Point(12, 22);
            this.attachmentListView.Name = "attachmentListView";
            this.attachmentListView.Size = new System.Drawing.Size(157, 283);
            this.attachmentListView.SmallImageList = this.attachmentImgList;
            this.attachmentListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.attachmentListView.TabIndex = 11;
            this.attachmentListView.TileSize = new System.Drawing.Size(219, 24);
            this.toolTip1.SetToolTip(this.attachmentListView, "List of attachments for current note.");
            this.attachmentListView.UseCompatibleStateImageBehavior = false;
            this.attachmentListView.View = System.Windows.Forms.View.Tile;
            this.attachmentListView.SelectedIndexChanged += new System.EventHandler(this.attachmentListView_SelectedIndexChanged);
            this.attachmentListView.ClientSizeChanged += new System.EventHandler(this.attachmentListView_ClientSizeChanged);
            this.attachmentListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.attachmentListView_KeyDown);
            this.attachmentListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.attachmentListView_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Attachments:";
            // 
            // updateImageFromPasteBtn
            // 
            this.updateImageFromPasteBtn.Enabled = true;
            this.updateImageFromPasteBtn.Interval = 700;
            this.updateImageFromPasteBtn.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // AddComment
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 401);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddComment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "AddComment";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Comment";
            this.Load += new System.EventHandler(this.AddComment_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.AddComment_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.AddComment_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddComment_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AddComment_KeyUp);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAddComment;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ImageList attachmentImgList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button removeAttachmentBtn;
        private System.Windows.Forms.Button addAttachmentBtn;
        private System.Windows.Forms.ListView attachmentListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button imageFromPasteBtn;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer updateImageFromPasteBtn;
    }
}