using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    /// <summary>
    /// Collects all the UI message boxes shown to the user.
    /// </summary>
    internal static class UIMessageBoxes
    {
        public static void NoPermissionsForThisAction()
        {
            MessageBox.Show("This account has no permission for this action.", "Permission denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
