using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes.Forms
{
    public partial class PastedImagePreview : Form
    {
        public PastedImagePreview(Image imageToPreview)
        {
            InitializeComponent();
            _imageToPreview = imageToPreview;
        }

        private Image _imageToPreview;
        private int bottomPadding = 0;

        private int minWidth = 0;

        private const int MAX_HEIGHT = 768;
        private const int MAX_WIDTH = 1280;

        public string FileNameToSave
        {
            get;
            set;
        }

        private void PastedImagePreview_Load(object sender, EventArgs e)
        {
            minWidth = this.Width;

            if (_imageToPreview != null)
            {
                bottomPadding = this.Height - previewBox.Height;

                LoadPreview();
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void SetUpFilenameBox()
        {
            string fileName = "pastedImage.png";
            filenameBox.Text = fileName;
            filenameBox.Focus();
            filenameBox.SelectAll();
        }

        private void LoadPreview()
        {
            SetPreviewBoxDimensions();

            previewBox.SizeMode = PictureBoxSizeMode.StretchImage;
            previewBox.Image = _imageToPreview;

        }

        private void SetPreviewBoxDimensions()
        {
            //get pasted image aspect ratio
            float aspect = (float)_imageToPreview.Width / _imageToPreview.Height;

            int width = _imageToPreview.Width;
            int height = _imageToPreview.Height;

            if (width < minWidth)
            {
                width = minWidth;
                height = (int)(width / aspect);
            }

            if (width > MAX_WIDTH)
            {
                width = MAX_WIDTH;
                height = (int)(width / aspect);
            }
            else if (height > MAX_HEIGHT)
            {
                height = MAX_HEIGHT;
                width = (int)(height * aspect);
            }

            previewBox.Width = width;
            previewBox.Height = height;
        }

        private void PastedImagePreview_Shown(object sender, EventArgs e)
        {
            SetUpFilenameBox();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FileNameToSave = filenameBox.Text;
        }

        private void previewBox_Resize(object sender, EventArgs e)
        {
            this.Height = previewBox.Height + bottomPadding;
            this.Width = Math.Max(previewBox.Width,minWidth);
        }
    }
}
