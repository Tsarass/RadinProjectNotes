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

namespace RadinProjectNotes.Forms
{
    public partial class ListActiveProjects : Form
    {
        public ListActiveProjects()
        {
            InitializeComponent();
        }

        string[] filesToExclude = new string[] { "users.db", "latest.db"};

        private List<string> projectList = new List<string>();

        public string ProjectToOpen
        {
            get;
            set;
        }

        private void ListActiveProjects_Load(object sender, EventArgs e)
        {
            projectList = GetAllActiveProjects();
        }

        private void UpdateList(List<string> itemsToList)
        {
            listProjects.Items.Clear();
            foreach (var project in itemsToList)
            {
                listProjects.Items.Add(project);
            }

            listProjects.Columns[0].Width = listProjects.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
        }

        private List<string> GetAllActiveProjects()
        {
            List<string> projects = new List<string>();

            string[] dbFiles =Directory.GetFiles(ServerConnection.serverFolder, "*.db");
            foreach (var dbFile in dbFiles)
            {
                string projectName = Path.GetFileName(dbFile);

                if (filesToExclude.Contains<string>(projectName))
                {
                    continue;
                }

                if (projectName.EndsWith(".db"))
                {
                    projectName = projectName.Substring(0, projectName.Length - 3);
                }

                if (ProjectExistsInFolderCache(projectName))
                {
                    projects.Add(projectName);
                    
                }
            }

            return projects;

        }

        private bool ProjectExistsInFolderCache(string projectPath)
        {
            foreach (var folder in ServerConnection.folderCache)
            {
                if (folder.projectPath == projectPath)
                {
                    return true;
                }
            }

            return false;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ProjectToOpen = listProjects.SelectedItems[0].Text;
        }

        private void ListActiveProjects_Shown(object sender, EventArgs e)
        {
            UpdateList(projectList);
            CreateFilters();
        }

        private void CreateFilters()
        {
            List<string> filters = new List<string>();
            foreach (var project in projectList)
            {
                string year = project.Substring(0, 2);
                string newFilter = $"Projekte 20{year}";
                if (!filters.Contains(newFilter))
                {
                    filters.Add(newFilter);
                }
            }

            GenerateFilterControls(filters);

        }

        private void GenerateFilterControls(List<string> filters)
        {
            filterPanel.Controls.Clear();
            foreach (var filter in filters)
            {
                CheckBox chk = new CheckBox();
                chk.Text = filter;
                chk.Checked = false;
                chk.CheckedChanged += FilterCheckedChanged;
                chk.Tag = filter;

                filterPanel.Controls.Add(chk);
            }
        }

        private void FilterCheckedChanged(object sender, EventArgs e)
        {
            //first get all active filters
            List<string> activeFilters = new List<string>();
            foreach (var control in filterPanel.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox chk = (CheckBox)control;
                    if ((chk.Checked) && (!activeFilters.Contains(chk.Text)))
                    {
                        activeFilters.Add(chk.Text);
                    }
                }
            }

            //if no filters checked, show all projects
            if (activeFilters.Count <= 0)
            {
                UpdateList(projectList);
                return;
            }

            //if filters are checked, show only those projects
            List<string> itemsToShow = new List<string>();
            List<string> projectFullList = new List<string>(projectList);
            foreach (var filter in activeFilters)
            {
                string filterYear = filter.Substring(11, 2);
                for (int i = projectFullList.Count - 1; i >= 0 ; i--)
                {
                    if (projectFullList[i].StartsWith(filterYear))
                    {
                        itemsToShow.Add(projectFullList[i]);
                        projectFullList.RemoveAt(i);
                    }
                } 
            }

            UpdateList(itemsToShow);            
        }
    }
}
