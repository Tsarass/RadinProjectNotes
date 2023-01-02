using RadinProjectNotes.HelperClasses;
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
    public partial class MainFormConfirmationDialog : Form
    {
        public enum ExitDialogResult
        {
            Exit,
            StayInBackground,
            Cancel
        }

        public ExitDialogResult dialogResult;

        public MainFormConfirmationDialog()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dialogResult = ExitDialogResult.Cancel;
            this.Close();
        }

        private void btnStay_Click(object sender, EventArgs e)
        {
            dialogResult = ExitDialogResult.StayInBackground;
            if (chkRemember.Checked)
            {
                RegistryFunctions.SetRegistryKeyValue(RegistryEntry.MinimizeInsteadOfProgramExit, "1");
            }

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            dialogResult = ExitDialogResult.Exit;
            if (chkRemember.Checked)
            {
                RegistryFunctions.SetRegistryKeyValue(RegistryEntry.MinimizeInsteadOfProgramExit, "0");
            }
            this.Close();
        }
    }
}
