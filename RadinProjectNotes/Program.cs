using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if RELEASE
            Process[] process = Process.GetProcessesByName(Application.ProductName); //Prevent multiple instance
            if (process.Length > 1)
            {
                ProcessCommunication.SendMessageToOpenProcess();

                //MessageBox.Show($"{Application.ProductName}  is already running in the background.", $"{Application.ProductName}",
                //MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Application.Exit();
                return;
            }
            else
            {
                ProcessCommunication.SetupMemoryMappedFile();
            }

#endif

            Application.Run(new MainForm(args));
        }

    }


}
