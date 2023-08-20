using System.Windows.Forms;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing;
using System;
using RadinProjectNotes.DatabaseFiles.ProjectServices;
using RadinProjectNotes.DatabaseFiles.Controllers;
using RadinProjectNotes.DatabaseFiles;
using EncryptedDatabaseSerializer;

namespace RadinProjectNotes.Controls
{
    // Make the control act as a container in the form designer.
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class AssignedServicesPanel : UserControl
    {
        private readonly Font warningFont = new Font("Microsoft Sans Serif", 9.5f, FontStyle.Italic);
        private readonly Font headingFont = new Font("Microsoft Sans Serif", 9.5f, FontStyle.Bold);
        private readonly Font serviceFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);

        private ProjectAssignedServices _cachedAssignedServices;
        private Label _warningLabel;

        public AssignedServicesPanel()
        {
            InitializeComponent();

            _warningLabel = new Label { Text = "Use the search box to select a project first.", Font = warningFont, AutoSize = true };
            _warningLabel.Location = new Point(10, 10);
            this.Controls.Add(_warningLabel);
        }

        public void UpdateServicesPanel()
        {
            if (MainForm.currentProject is null)
            {
                servicePanel.Visible = false;
                _warningLabel.Visible = true;

                return;
            }
            else
            {
                servicePanel.Visible = true;
                _warningLabel.Visible = false;
            }

            // Clear panel.
            ClearServicesPanel();

            RadinProjectServices services;
            try
            {
                _cachedAssignedServices = ProjectAssignedServicesController.TryLoadProjectServices(MainForm.currentProject);
                services = ProjectServicesController.TryLoadProjectServices();
            }
            catch (CouldNotLoadDatabase)
            {
                MessageBox.Show("Could not access the services database. Check connection and try again.", "Connection failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            SetUpServiceColumns(services);

            int columnId = 0;
            foreach (var categoryTitle in services.GetCategoryTitles())
            {
                // Add label as a heading to each.
                servicePanel.Controls.Add(new Label() { Font = headingFont, Text = categoryTitle, AutoSize = true }, columnId, 0);

                // Start from the second row (first is the heading)
                int rowId = 1;
                foreach (var service in services.getServicesForCategory(categoryTitle))
                {
                    CheckBox newCheckbox = new CheckBox { Font = serviceFont, Text = service, AutoSize = true, Tag = categoryTitle };
                    newCheckbox.AutoCheck = Credentials.Instance.currentUser.CanEditProjectServices();
                    newCheckbox.Checked = _cachedAssignedServices.ContainsService(categoryTitle, service);
                    servicePanel.Controls.Add(newCheckbox, columnId, rowId);

                    rowId++;
                }

                columnId++;
            }
        }

        private void SetUpServiceColumns(RadinProjectServices services)
        {
            servicePanel.ColumnCount = services.GetCategoriesCount();
            servicePanel.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 25f);
            for (int i = 1; i < servicePanel.ColumnCount; i++)
            {
                servicePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            }
        }

        private void ClearServicesPanel()
        {
            while (servicePanel.Controls.Count > 0)
            {
                servicePanel.Controls[0].Dispose();
            }
        }

        public void ServicesPanelClosing()
        {
            // Check if changes have been made to the assignments.
            if (_cachedAssignedServices is null) return;

            ProjectAssignedServices currentAssignedServices = GetCurrentProjectAssignedServices();
            if (ProjectAssignedServicesHaveChanged(currentAssignedServices))
            {
                ProjectAssignedServicesController.TrySaveProjectServices(MainForm.currentProject, currentAssignedServices);
            }
        }

        private bool ProjectAssignedServicesHaveChanged(ProjectAssignedServices currentAssignedServices)
        {
            // Compare the cached services to the current ones.
            if (currentAssignedServices.AssignedServices.Count != _cachedAssignedServices.AssignedServices.Count)
            {
                return true;
            }
            foreach (var assignedService in currentAssignedServices.AssignedServices)
            {
                if (!_cachedAssignedServices.AssignedServices.Contains(assignedService))
                {
                    return true;
                }
            }

            return false;
        }

        private ProjectAssignedServices GetCurrentProjectAssignedServices()
        {
            ProjectAssignedServices currentAssignedServices = ProjectAssignedServices.CreateEmpty();

            // Loop all checkbox items in the table layout panel.
            foreach (var control in servicePanel.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox checkBox = control as CheckBox;
                    if (checkBox.Checked)
                    {
                        // The checkbox tag is equal to the service category title, and the text is equal to the service itself.
                        ProjectAssignedServices.AssignedService assignedService = new ProjectAssignedServices.AssignedService(checkBox.Tag as String, checkBox.Text);
                        currentAssignedServices.AddAssignedService(assignedService);
                    }
                }
            }

            return currentAssignedServices;
        }
    }
}
