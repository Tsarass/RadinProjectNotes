using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
    public class AutoUpdaterUpdater
    {

        private string updateFromPath = "";
        private string updateToPath = "";
        private string executablePath = "";

        /// <summary>
        /// Creates an instance of an updater for the AutoUpdater application.
        /// </summary>
        /// <param name="updateFromPath">the path where the installation files are found</param>
        public AutoUpdaterUpdater(string updateFromPath, string updateToPath, string executablePath)
        {
            this.updateFromPath = updateFromPath;
            this.updateToPath = updateToPath;
            this.executablePath = executablePath;
            UpdateAutoUpdater();
        }

        /// <summary>
        /// Checks and updates the Auto Updater application.
        /// </summary>
        private void UpdateAutoUpdater()
        {
            string currentUpdaterPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string updaterFileName = new FileInfo(currentUpdaterPath).Name;
            string candidateUpdaterPath = Path.Combine(updateFromPath, updaterFileName);

            if (AssemblyVersionChecker.CheckIfNewerVersion(currentUpdaterPath, candidateUpdaterPath))
            {
                //delete backup file if it exists
                string backupFileName = updaterFileName + ".bak";
                DeleteBackupUpdaterFile(backupFileName);

                //update the auto updater
                Console.WriteLine($"Updating file {updaterFileName}.");
                //first, rename the currently running auto updater app
                FileInfo fileInfo = new FileInfo(currentUpdaterPath);
                
                fileInfo.MoveTo(Path.Combine(fileInfo.DirectoryName, backupFileName));

                FileInfo sourcefile = new FileInfo(candidateUpdaterPath);
                try
                {
                    sourcefile.CopyTo(currentUpdaterPath, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                    return;
                }

                //run the new updater if successfull
                Console.WriteLine($"Running updated updater app..");
                RunUpdaterApp(currentUpdaterPath);

                //close this instance
                Environment.Exit(0);
            }
        }

        private void RunUpdaterApp(string targetPath)
        {

            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = targetPath,
                        Arguments = $"\"{updateToPath}\" \"{executablePath}\""
                    }
                };
                process.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not start updated AutoUpdater application: {targetPath}");
                Console.WriteLine(e.Message.ToString());
            }
        }

        private void DeleteBackupUpdaterFile(string fileName)
        {
            string filename = Path.Combine(updateToPath, fileName);
            if (File.Exists(filename))
            {
                try
                {
                    File.Delete(filename);
                }
                catch
                {

                }
                
            }
        }
    }
}