using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RadinProjectNotes.ServerConnection;

namespace RadinProjectNotes.HelperClasses
{
    public class ProjectInfosSpreadsheetController
    {
        private readonly string PROJECT_INFOS_FOLDER = @"Projektinfos";
        private readonly string PROJECT_INFOS_NEW_FOLDER = @"09_Projektinfos";
        private readonly string PROJECT_INFOS_SPREADSHEET = @"Projekt Infos-Ansprechpartner.xlsx";
        private ProjectFolder _projectFolder;

        public ProjectInfosSpreadsheetController(ProjectFolder projectFolder)
        {
            _projectFolder = projectFolder;
        }

        /// <summary>
        /// Open the project infos spreadsheet.
        /// </summary>
        /// <remarks>If the spreadsheet does not exist in a project, the program will attempt to copy the template spreadsheet from the server.</remarks>
        /// <exception cref="TemplateNotFound"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public void openSpreadsheet()
        {
            string projectInfosFolder = getProjectInfosFolder();
            string spreadsheetFolderPath = Path.Combine(_projectFolder.fullPath, projectInfosFolder);
            if (!Directory.Exists(spreadsheetFolderPath))
            {
                Directory.CreateDirectory(spreadsheetFolderPath);
            }

            string spreadsheetFullPath = Path.Combine(spreadsheetFolderPath, PROJECT_INFOS_SPREADSHEET);

            // If the file does not exist, copy the template file.
            if (!File.Exists(spreadsheetFullPath))
            {
                string templateSpreadsheetFilePath = Path.Combine(serverFolder, PROJECT_INFOS_SPREADSHEET);
                try
                {
                    File.Copy(templateSpreadsheetFilePath, spreadsheetFullPath);
                }
                catch
                {
                    throw new TemplateNotFound();
                }
            }

            if (!File.Exists(spreadsheetFullPath))
            {
                throw new FileNotFoundException();
            }

            // Open the spreadsheet with the default application.
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = spreadsheetFullPath;
            psi.UseShellExecute = true;
            Process.Start(psi);
        }

        private string getProjectInfosFolder()
        {
            // Find the Projektinfos folder.
            string[] subfolders = Directory.GetDirectories(_projectFolder.fullPath);
            string projectInfosFolder = subfolders.FirstOrDefault(a => a.Contains(PROJECT_INFOS_FOLDER));

            // If the folder was not found with the generic name, set to the hardcoded one.
            if (string.IsNullOrEmpty(projectInfosFolder))
            {
                return PROJECT_INFOS_NEW_FOLDER;
            }
            return projectInfosFolder;
        }

        public class TemplateNotFound : Exception
        {
            public TemplateNotFound() { }
        }
    }
}
