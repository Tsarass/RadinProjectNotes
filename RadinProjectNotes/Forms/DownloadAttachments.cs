using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public partial class DownloadAttachments : Form
    {
        Notes.ProjectNote note;

        public DownloadAttachments(Notes.ProjectNote note)
        {
            InitializeComponent();
            this.note = note;
        }

        private void DownloadAttachments_Load(object sender, EventArgs e)
        {
            LoadAttachments();

            //select all
            chkSelectAll.Checked = true;
        }

        private void LoadAttachments()
        {
            foreach (var attachment in note.attachmentLibrary.GetAttachments())
            {
                ListViewItem newItem = new ListViewItem(attachment.FileName);
                newItem.Checked = false;
                newItem.Tag = attachment.Id;

                newItem.ToolTipText = attachment.FileName;

                attachmentListView.Items.Add(newItem);
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in attachmentListView.Items)
            {
                item.Checked = chkSelectAll.Checked;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Prepare a dummy string, thos would appear in the dialog
            string dummyFileName = "Save Here";

            SaveFileDialog sf = new SaveFileDialog();
            // Feed the dummy name to the save dialog
            sf.FileName = dummyFileName;
            sf.Filter = "Directory | directory";

            if (sf.ShowDialog() == DialogResult.OK)
            {
                // Now here's our save folder
                string savePath = Path.GetDirectoryName(sf.FileName);

                Cursor.Current = Cursors.WaitCursor;

                for (int i = 0; i < attachmentListView.Items.Count; i++)
                {
                    ListViewItem item = attachmentListView.Items[i];
                    Attachment attachmentToSave;
                    try
                    {
                        attachmentToSave = note.attachmentLibrary.FindAttachmentById((Guid)item.Tag);
                    }
                    catch (AttachmentLibrary.AttachmentNotFound)
                    {
                        continue;
                    }

                    if (item.Checked)
                    {
                        SaveAttachment(attachmentToSave.AttachmentSavedToDiskFilePath, Path.Combine(savePath,attachmentToSave.FileName));
                    }
                }

                Cursor.Current = Cursors.Default;
            }

        }

        private void SaveAttachment(string sourcePath, string destinationFolder)
        {
            try
            {
                File.Copy(sourcePath, destinationFolder, true);
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }
        }
    }
}
