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

        readonly string[] filesToExclude = new string[] { "users.db", "latest.db"};

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
            var projectFolder = ServerConnection.GetProjectFolderFromProjectPath(projectPath);
            if (projectFolder is null)
            {
                return false;                
            }

            return true;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (listProjects.SelectedItems.Count > 0)
            {
                ProjectToOpen = listProjects.SelectedItems[0].Text;
                this.DialogResult= DialogResult.OK;
                this.Close();
            }
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
                CheckBox chk = new CheckBox
                {
                    Text = filter,
                    Checked = false,
                    Tag = filter
                };
                chk.CheckedChanged += FilterCheckedChanged;

                filterPanel.Controls.Add(chk);
            }
        }

        private void FilterCheckedChanged(object sender, EventArgs e)
        {
            //when a filter changes, selection is always reset
            setOpenButtonEnabled(false);

            //first get all active filters
            List<string> activeFilters = getActiveFilters();

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
                for (int i = projectFullList.Count - 1; i >= 0; i--)
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

        private List<string> getActiveFilters()
        {
            List<string> activeFilters = new List<string>();
            foreach (var control in filterPanel.Controls)
            {
                if (control is CheckBox chk)
                {
                    if (chk.Checked && (!activeFilters.Contains(chk.Text)))
                    {
                        activeFilters.Add(chk.Text);
                    }
                }
            }

            return activeFilters;
        }

        private void listProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            setOpenButtonEnabled(listProjects.SelectedItems.Count > 0);
        }

        private void setOpenButtonEnabled(bool enabled)
        {
            btnOpen.Enabled = enabled;
        }
    }
}
