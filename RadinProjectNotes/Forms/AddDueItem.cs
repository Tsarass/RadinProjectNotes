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
    public partial class AddDueItem : Form
    {

        public enum AddDueItemFormMode
        {
            AddDueItemMode = 0,
            EditCommentMode = 1
        }

        public string projectName = "";
        public AddDueItemFormMode formMode = 0;

        public AddDueItem()
        {
            InitializeComponent();
        }

    }
}
