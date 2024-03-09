using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public class AutoUpdater
    {
        private readonly string updateFilesFolder = @"latest_version";
        private readonly string updaterExecutable = @"AutoUpdate.exe";
        private readonly string updateFilesDirectory;

        private bool silent = false;
        private MainForm mainForm;

        public AutoUpdater(bool silent, MainForm mainForm)
        {
            updateFilesDirectory = Path.Combine(ServerConnection.serverFolder, updateFilesFolder);
            this.silent = silent;
            this.mainForm = mainForm;
        }

        public void CheckForUpdate()
        {
            //check if update directory exists
            if (!Directory.Exists(updateFilesDirectory))
            {
                if (!silent)
                {
                    MessageBox.Show($"Update directory not found.\n{updateFilesDirectory}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            //try to locate the executable file
            string[] updateFiles = Directory.GetFiles(updateFilesDirectory);
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string executable = FindExecutable(updateFiles, assemblyName);

            if (executable == String.Empty)
            {
                if (!silent)
                {
                    MessageBox.Show($"Executable not found.\n{assemblyName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            //get the version of the executable's assembly
            FileVersionInfo fv;
            try
            {
               fv = FileVersionInfo.GetVersionInfo(executable);
            }
            catch
            {
                if (!silent)
                {
                    MessageBox.Show($"Could not get assembly info on {executable}.\nUpdate will not continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            Version currentVersion = FileVersion;

            if (UpdateFileVersionNewer(currentVersion, fv))
            {
                DialogResult dlg = MessageBox.Show($"Update available: Version {fv.FileVersion}\nDo you want to update now?\r\nIt is highly recommended to keep the application up to date.",
                    "Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dlg == DialogResult.Yes)
                {
                    RunUpdater(new string[] { executable });
                }
            }
            else
            {
                if (!silent)
                {
                    MessageBox.Show($"No updates found.", "Update not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void RunUpdater(string[] filesToUpdate)
        {

            //get the current directory
            string strExeFilePath = Assembly.GetExecutingAssembly().Location;
            string strWorkPath = Path.GetDirectoryName(strExeFilePath);
            string updaterFileName = Path.Combine(strWorkPath, updaterExecutable);

            var process = new Process
            {
              StartInfo =
              {
                  FileName = updaterFileName,
                  Arguments = $"\"{strWorkPath}\" \"{filesToUpdate[0]}\""
              }
            };
            process.Start();

            mainForm.ExitApp(forceProgramExit : true);
        }

        private string FindExecutable(string[] updateFiles, string assemblyName)
        {
            foreach (var file in updateFiles)
            {
                if (file.EndsWith($"{assemblyName}.exe"))
                {
                    return file;
                }
            }

            return String.Empty;
        }

        public static Version FileVersion
        {
            get
            {
                return new Version(FileVersionInfo.GetVersionInfo(Assembly.GetCallingAssembly().Location).FileVersion);
            }
        }

        private bool UpdateFileVersionNewer(Version current, FileVersionInfo updateFile)
        {
            Version updateFileVersion = new Version(string.Format("{0}.{1}.{2}.{3}", updateFile.FileMajorPart, updateFile.FileMinorPart, updateFile.FileBuildPart, updateFile.FilePrivatePart));
            return updateFileVersion > current;
        }
    }
}
