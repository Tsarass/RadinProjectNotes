using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public partial class AddComment : Form
    {

        public enum AddCommentFormMode
        {
            AddCommentMode = 0,
            EditCommentMode = 1
        }

        public string projectName = "";
        public AddCommentFormMode formMode = 0;

        public Notes.ProjectNote currentNote = null;

        public AddComment()
        {
            InitializeComponent();
        }

        private void AddComment_Load(object sender, EventArgs e)
        {   
            
            if (currentNote is null)
            {
                currentNote = new Notes.ProjectNote();
            }

            switch (formMode)
            {
                case AddCommentFormMode.AddCommentMode:
                    this.Text = "Add Comment || " + projectName;
                    this.btnAddComment.Text = "Add Comment";
                    break;
                case AddCommentFormMode.EditCommentMode:
                    this.Text = "Edit Comment || " + projectName;
                    this.btnAddComment.Text = "Edit Comment";

                    textBox1.Text = currentNote.noteText;
                    AddAttachmentsToList(currentNote.attachmentLibrary);
                    btnAddComment.Enabled = true;
                    break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAddComment_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            currentNote.noteText = textBox1.Text;
            
            //save all attachments to disk
            if(currentNote.attachmentLibrary.GetAttachments().Count > 0)
            {
                currentNote.attachmentLibrary.SaveAttachmentsToDisk();
            }

            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                btnAddComment.Enabled = true;
            }
            else
            {
                btnAddComment.Enabled = false;
            }
        }

        private void addAttachmentBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Title = "Browse Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "*.*",
                Filter = "All files (*.*)|*.*",
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true,
                Multiselect = true
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string[] files = dlg.FileNames;
                HandleAttachmentInput(files);
            }
        }

        private void HandleAttachmentInput(string[] files)
        {
            foreach (var filename in files)
            {
                //check if max file size exceeded
                FileInfo fi = new FileInfo(filename);
                if (fi.Length > Notes.maxAttachmentByteSize)
                {
                    MessageBox.Show($"File {fi.Name} exceeded the maximum filesize of {Notes.maxAttachmentMB}MB.", "File too large", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                Attachment attachment = new Attachment(filename);
                if (SuccessfullyAddedAttachment(attachment))
                {
                    AddAttachmentToList(attachment);
                }
            }
        }

        private bool SuccessfullyAddedAttachment(Attachment attachment)
        {
            return currentNote.attachmentLibrary.AddAttachment(attachment) != null;
        }

        private void AddAttachmentsToList(AttachmentLibrary attachmentLibrary)
        {
            //set up icons first
            attachmentLibrary.LoadAttachmentIcons();

            foreach (var attachment in attachmentLibrary.attachments)
            {
                AddAttachmentToList(attachment);
            }
        }

        private void AddAttachmentToList(Attachment attachment)
        {
            string attachmentFilePath = attachment.SavedToDisk ? attachment.AttachmentSavedToDiskFilePath : attachment.OriginalFilePath;

            Icon icon = null;
            //if icon is cached, use this
            if (attachment.Icon != null)
            {
                icon = attachment.Icon;
            }
            //else load icon from file
            else
            {
                try
                {
                    icon = ExtractIcon.ExtractAssociatedIcon(attachmentFilePath);
                }
                catch
                {
                    throw;
                }
            }

            attachmentImgList.Images.Add(icon);
            attachment.Icon = icon;

            ListViewItem newitem = new ListViewItem();
            string fileNameInList = "";
            if (!File.Exists(attachment.AttachmentSavedToDiskFilePath) && (attachment.SavedToDisk))
            {
                fileNameInList = $"(not found){attachment.FileName}";
            }
            else
            {
                fileNameInList = attachment.FileName;
            }
            newitem.Text = fileNameInList;
            newitem.Tag = attachment.Id;    //set the Tag of the listview item equal to the guid of the attachment
            newitem.ImageIndex = attachmentImgList.Images.Count - 1;
            attachmentListView.Items.Add(newitem);
        }

        private void attachmentListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedAttachment();
            }
        }

        private void attachmentListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (attachmentListView.SelectedItems.Count > 0)
            {
                removeAttachmentBtn.Enabled = true;
            }
            else
            {
                removeAttachmentBtn.Enabled = false;
            }
        }

        private void removeAttachmentBtn_Click(object sender, EventArgs e)
        {
            DeleteSelectedAttachment();
        }

        private void DeleteSelectedAttachment()
        {
            if (attachmentListView.SelectedItems.Count > 0)
            {
                Attachment attachment = currentNote.attachmentLibrary.FindAttachment((Guid)attachmentListView.SelectedItems[0].Tag);
                currentNote.attachmentLibrary.RemoveAttachment(attachment, true);
                attachmentListView.SelectedItems[0].Remove();
            }
        }

        private void attachmentListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (attachmentListView.SelectedItems.Count > 0)
            {
                Attachment match = currentNote.attachmentLibrary.FindAttachment((Guid)attachmentListView.SelectedItems[0].Tag);
                if (match != null)
                {
                    bool fileFound = match.OpenFile();  //OpenFile returns false if file not found
                    if (!fileFound)
                    {
                        DialogResult dlg = MessageBox.Show($"Could not locate file {match.FileName}. Remove it from the attachment list?", "File not found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dlg == DialogResult.Yes)
                        {
                            DeleteSelectedAttachment();
                        }
                        
                    }
                        
                }
            }
        }

        private void attachmentListView_ClientSizeChanged(object sender, EventArgs e)
        {
            int minWidth = 220;
            attachmentListView.TileSize = new Size(Math.Max(attachmentListView.ClientSize.Width,minWidth), attachmentListView.TileSize.Height);
        }

        private void AddComment_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            HandleAttachmentInput(files);
        }

        private void AddComment_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void AddComment_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*
            if (((int)e.KeyChar == 22) && (Control.ModifierKeys == Keys.Control))
            {
                if (Clipboard.ContainsImage())
                {
                    CreateAttachmentFromClipboardImage();
                }
                
            }*/
        }

        private void CreateAttachmentFromClipboardImage()
        {
            Image image = Clipboard.GetImage();

            Forms.PastedImagePreview frm = new Forms.PastedImagePreview(image);
            DialogResult dlg = frm.ShowDialog();

            if (dlg == DialogResult.OK)
            {
                string filePath = GetImageTempFilename(frm.FileNameToSave);
                Attachment attachment = new Attachment(filePath);

                image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                HandleAttachmentInput(new string[] { filePath });
            }
           
        }

        private string GetImageTempFilename(string filename)
        {
            string filepath = filename.EndsWith(".png") ? Path.GetTempPath() + filename : Path.GetTempPath() + filename + ".png";
            return filepath;
        }

        private bool isControlKeyDown = false;

        private void AddComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys == Keys.Control) isControlKeyDown = true;

            if (isControlKeyDown && e.KeyCode == Keys.V)
            {
                if (Clipboard.ContainsImage())
                {
                    CreateAttachmentFromClipboardImage();
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void AddComment_KeyUp(object sender, KeyEventArgs e)
        {
            isControlKeyDown = ModifierKeys == Keys.Control;
        }

        private void imageFromPasteBtn_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                CreateAttachmentFromClipboardImage();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
             imageFromPasteBtn.Enabled = Clipboard.ContainsImage();
        }
    }
}
