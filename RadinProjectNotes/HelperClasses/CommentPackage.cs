using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace RadinProjectNotes
{
    public partial class CommentPackage : UserControl
    {
        private const int MAX_ATTACHMENT_MENU_ITEM_CHARACTERS = 36;

        public event EventHandler<EditCommentArgs> EditCommentEvent;
        public event EventHandler<EditCommentArgs> DeleteCommentEvent;

        public Notes.ProjectNote Note
        {
            get;
            set;
        }

        public bool Editable
        {
            get;
            set;
        }

        public bool Deleteable
        {
            get;
            set;
        }

        public bool attachmentMenuStripCreated = false;

        public class EditCommentArgs : EventArgs
        {
            public Notes.ProjectNote Note { get; set; }
        }

        protected virtual void OnEditComment(EditCommentArgs e)
        {
            EventHandler< EditCommentArgs> eh = EditCommentEvent;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected virtual void OnDeleteComment(EditCommentArgs e)
        {
            EventHandler<EditCommentArgs> eh = DeleteCommentEvent;
            if (eh != null)
            {
                eh(this, e);
            }
        }


        private int minTextboxHeight = 80;
        private int maxTextboxHeight = 160;
        private const int BORDER_SIZE = 2;
        private int heightBuffer;
        private int widthBuffer;

        private bool packageHasFocus = false;

        private FlowLayoutPanel parentControl;

        public CommentPackage()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            

            base.OnPaint(e);
            /*
            if (packageHasFocus)
            {
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                                         Color.Blue, BORDER_SIZE, ButtonBorderStyle.Inset,
                                         Color.Blue, BORDER_SIZE, ButtonBorderStyle.Inset,
                                         Color.Blue, BORDER_SIZE, ButtonBorderStyle.Inset,
                                         Color.Blue, BORDER_SIZE, ButtonBorderStyle.Inset);
            }
            */
            
        }

        private void CommentPackage_Load(object sender, EventArgs e)
        {
            if (this.Parent != null && this.Parent.GetType() == typeof(FlowLayoutPanel))
            {
                parentControl = this.Parent as FlowLayoutPanel;
            }

            heightBuffer = this.Height - this.boxCommentPreview.Height;
            widthBuffer = this.Width - this.boxCommentPreview.Width;

            UpdatePanel();

            if (!Editable)
            {
                this.btnEdit.Enabled = false;
            }
            if (!Deleteable)
            {
                this.btnDelete.Enabled = false;
            }
        }

        public void UpdatePanel()
        {
            this.lblCommentTitle.Text = Note.CreatedByUsername + " posted on " + Note.DateCreatedString;
            this.boxCommentPreview.Text = Note.noteText;

            if(Note.attachmentLibrary.attachments.Count > 0)
            {
                splitButton1.Visible = true;
                splitButton1.Text = Note.attachmentLibrary.attachments.Count > 1 ? $"{Note.attachmentLibrary.attachments.Count} Attachments" : $"{Note.attachmentLibrary.attachments.Count} Attachment";
            }
            else
            {
                splitButton1.Visible = false;
            }

            attachmentMenuStripCreated = false;

            SetControlWidth();
            SetTextboxHeight();
        }

        private void SetTextboxHeight()
        {
            //count lines pes paragraph
            string[] pars = this.boxCommentPreview.Text.Split('\n');
            int lines = 0;
            foreach(string par in pars)
            {
                Size size = TextRenderer.MeasureText(par, boxCommentPreview.Font);
                int width = Math.Max(this.boxCommentPreview.Width, 1);
                lines += size.Width / width + 1;
            }
            int height = Math.Max(lines * boxCommentPreview.Font.Height, minTextboxHeight);
            this.boxCommentPreview.Height = Math.Min(height, maxTextboxHeight);
            this.Height = heightBuffer + this.boxCommentPreview.Height + 3;
        }

        private void CommentPackage_Resize(object sender, EventArgs e)
        {
            SetTextboxHeight();
            SetControlWidth();
        }

        private void SetControlWidth()
        {
            //find out if vertical scrollbar is visible in the flow layout panel
            if (parentControl != null)
            {
                if (parentControl.VerticalScroll.Visible)
                {
                    parentControl.HorizontalScroll.Visible = false;
                }
            }

            Control parent = this.Parent;
            this.Width = parent.ClientSize.Width - 12;  //12 pixels buffer
            this.boxCommentPreview.Width = this.Width - widthBuffer;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditCommentArgs eh = new EditCommentArgs();
            eh.Note = this.Note;
            OnEditComment(eh);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            EditCommentArgs eh = new EditCommentArgs();
            eh.Note = this.Note;
            OnDeleteComment(eh);
        }

        private void splitButton1_ButtonClick(object sender, EventArgs e)
        {
            if (!attachmentMenuStripCreated)
            {
                PopulateAttachmentMenuStrip();
            }

            attachmentMenuStrip.Show(splitButton1, new Point(splitButton1.Width - attachmentMenuStrip.Width, splitButton1.Height));
        }

        private void PopulateAttachmentMenuStrip()
        {
            //check if attachment icons were loaded already for this note
            if (!Note.attachmentLibrary.AttachmentIconsLoaded)
            {
                this.Cursor = Cursors.WaitCursor;
                //load attachment icons
                Note.attachmentLibrary.LoadAttachmentIcons();
                this.Cursor = Cursors.Default;
            }

            attachmentMenuStrip.Items.Clear();

            foreach (var attachment in Note.attachmentLibrary.attachments)
            {
                if (File.Exists(attachment.AttachmentSavedToDiskFilePath))
                {
                    ToolStripMenuItem newMenuItem = new ToolStripMenuItem(GetAttachmentMenuItemFilename(attachment.FileName), attachment.Icon.ToBitmap());
                    newMenuItem.Tag = attachment.Id;
                    newMenuItem.Font = attachmentMenuStrip.Font;
                    newMenuItem.Click += new EventHandler(MenuItemClickHandler);
                    attachmentMenuStrip.Items.Add(newMenuItem);
                }
            }

            //create download attachments submenu
            ToolStripSeparator line = new ToolStripSeparator();
            Stream bmpStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RadinProjectNotes.icons.save.png");
            Bitmap saveIconBmp = new Bitmap(bmpStream);
            ToolStripMenuItem saveAttachmentsButton = new ToolStripMenuItem("Save attachments", saveIconBmp);
            saveAttachmentsButton.Font = attachmentMenuStrip.Font;
            saveAttachmentsButton.Click += new EventHandler(MenuDownloadAttachmentsClickHandler);
            attachmentMenuStrip.Items.Add(line);
            attachmentMenuStrip.Items.Add(saveAttachmentsButton);

            attachmentMenuStripCreated = true;
        }

        private void MenuItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            //find attachment by Guid (Tag)
            Attachment attachmentToOpen;
            try
            {
                attachmentToOpen = Note.attachmentLibrary.FindAttachmentById((Guid)clickedItem.Tag);
            }
            catch (AttachmentLibrary.AttachmentNotFound)
            {
                return;
            }

            bool couldOpenFile = attachmentToOpen.TryOpenFile();
            if (!couldOpenFile)
            {
                MessageBox.Show($"Could not open file {clickedItem.Text}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void MenuDownloadAttachmentsClickHandler(object sender, EventArgs e)
        {
            DownloadAttachments frm = new DownloadAttachments(Note);
            frm.ShowDialog();

        }

        private string GetAttachmentMenuItemFilename(string fullFilename)
        {
            if (fullFilename.Length > MAX_ATTACHMENT_MENU_ITEM_CHARACTERS)
            {
                string subString = fullFilename.Substring(0, MAX_ATTACHMENT_MENU_ITEM_CHARACTERS - 3);
                return subString + "...";
            }

            return fullFilename;
        }

        private void CommentPackage_Enter(object sender, EventArgs e)
        {
            packageHasFocus = true;

            //this.Invalidate(false);
        }

        private void CommentPackage_Leave(object sender, EventArgs e)
        {
            packageHasFocus = false;

            //this.Invalidate(false);
        }
    }
}
