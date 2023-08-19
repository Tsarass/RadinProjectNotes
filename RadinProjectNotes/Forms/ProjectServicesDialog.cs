using RadinProjectNotes.DatabaseFiles.Controllers;
using RadinProjectNotes.DatabaseFiles.ProjectServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public partial class ProjectServicesDialog : Form
    {
        private RadinProjectServices _cachedServiceCategories;
        private RadinProjectServices _serviceCategories;
        private string _selectedCategoryTitle;

        public ProjectServicesDialog()
        {
            InitializeComponent();

            btnAddService.Enabled = false;
            btnRemoveService.Enabled = false;
            btnAddCategory.Enabled = false;
            btnRemoveCategory.Enabled = false;

            InitialiseProjectServices();
        }

        private void InitialiseProjectServices()
        {
            _cachedServiceCategories = ProjectServicesController.TryLoadProjectServices();

            _serviceCategories = GetDeepCopyOfServiceCategories(_cachedServiceCategories);
            UpdateCategoriesList(_serviceCategories);

            // Select the first index if it exists.
            if (lstCategories.Items.Count > 0)
            {
                lstCategories.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Update the categories list with the data from the supplied service categories object.
        /// </summary>
        /// <param name="serviceCategories"></param>
        private void UpdateCategoriesList(RadinProjectServices serviceCategories)
        {
            lstCategories.Items.Clear();

            foreach (var categoryTitle in serviceCategories.GetCategoryTitles())
            {
                lstCategories.Items.Add(categoryTitle);
            }
        }

        /// <summary>
        /// Update the services list with the data from the supplied service categories object.
        /// </summary>
        /// <param name="serviceCategories"></param>
        /// <param name="categoryTitle"></param>
        private void UpdateServicesList(RadinProjectServices serviceCategories, string categoryTitle)
        {
            foreach (var service in serviceCategories.getServicesForCategory(categoryTitle))
            {
                lstServices.Items.Add(service);
            }
        }

        /// <summary>
        /// Create a deep copy of the service categories.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private RadinProjectServices GetDeepCopyOfServiceCategories(RadinProjectServices input)
        {
            var categoryTitles = input.GetCategoryTitles();

            List<ServiceCategory> serviceCategories = categoryTitles.Select(title =>
            {
                return new ServiceCategory(title, input.getServicesForCategory(title).ToList());
            }).ToList();

            return new RadinProjectServices(serviceCategories);
        }

        private void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstServices.Items.Clear();

            if (lstCategories.SelectedItems.Count > 0)
            {
                _selectedCategoryTitle = lstCategories.SelectedItem.ToString();
                btnRemoveCategory.Enabled = true;

                UpdateServicesList(_serviceCategories, _selectedCategoryTitle);

                // Select the first service.
                if (lstServices.Items.Count > 0)
                {
                    lstServices.SelectedIndex = 0;
                }
            }
            else
            {
                _selectedCategoryTitle = "";
                btnRemoveCategory.Enabled = false;
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewCategory.Text)) return;

            ServiceCategory category = new ServiceCategory(txtNewCategory.Text, new List<string>());
            _serviceCategories.AddServiceCategory(category);

            UpdateCategoriesList(_serviceCategories);

            // Select the last index.
            lstCategories.SelectedIndex = lstCategories.Items.Count - 1;

            // Clear the text from textbox.
            txtNewCategory.Text = "";
        }

        private void btnRemoveCategory_Click(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItems.Count == 0) return;

            string categoryTitle = lstCategories.SelectedItem.ToString();
            _serviceCategories.RemoveServiceCategory(categoryTitle);

            UpdateCategoriesList(_serviceCategories);

            // Select the last index if it exists.
            if (lstCategories.Items.Count > 0)
            {
                lstCategories.SelectedIndex = lstCategories.Items.Count - 1;
            }
            else
            {
                btnRemoveCategory.Enabled = false;
            }
        }

        private void btnAddService_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewService.Text)) return;
            if (_selectedCategoryTitle == "") return;

            _serviceCategories.AddServiceToCategory(_selectedCategoryTitle, txtNewService.Text);

            UpdateCategoriesList(_serviceCategories);

            // Select the cached category.
            SelectListItemByValue(lstCategories, _selectedCategoryTitle);
            // Select the last secvice index.
            lstServices.SelectedIndex = lstServices.Items.Count - 1;

            // Clear the text from textbox.
            txtNewService.Text = "";
        }

        private void btnRemoveService_Click(object sender, EventArgs e)
        {
            if (lstServices.SelectedItems.Count == 0) return;
            if (_selectedCategoryTitle == "") return;

            string service = lstServices.SelectedItem.ToString();
            _serviceCategories.removeServiceFromCategory(_selectedCategoryTitle, service);

            UpdateCategoriesList(_serviceCategories);

            // Select the cached category.
            SelectListItemByValue(lstCategories, _selectedCategoryTitle);
            // Select the last index if it exists.
            if (lstServices.Items.Count > 0)
            {
                lstServices.SelectedIndex = lstServices.Items.Count - 1;
            }
            else
            {
                btnRemoveService.Enabled = false;
            }
        }

        /// <summary>
        /// Select a listbox item with the supplied value.
        /// </summary>
        /// <param name="lstCategories"></param>
        /// <param name="selectedCategoryTitle"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SelectListItemByValue(ListBox listBox, string value)
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].ToString() == value)
                {
                    listBox.SelectedIndex = i;
                }
            }
        }

        private void lstServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItems.Count > 0)
            {
                btnRemoveService.Enabled = true;
            }
            else
            {
                btnRemoveService.Enabled = false;
            }
        }

        private void txtNewCategory_TextChanged(object sender, EventArgs e)
        {
            btnAddCategory.Enabled = txtNewCategory.Text.Length > 0;
        }

        private void txtNewService_TextChanged(object sender, EventArgs e)
        {
            btnAddService.Enabled = txtNewService.Text.Length > 0;
        }

        private void txtNewCategory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddCategory_Click(sender, e);
            }
        }

        private void txtNewService_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddService_Click(sender, e);
            }
        }

        /// <summary>
        /// Check if the services have changed as a result of use of the dialog.
        /// </summary>
        /// <remarks>Compares the temporary services object of this dialog with the one cached from the database file.</remarks>
        /// <returns></returns>
        private bool ServicesHaveChanged()
        {
            // Compare number of categories.
            if (_cachedServiceCategories.GetCategoriesCount() != _serviceCategories.GetCategoriesCount())
            {
                return true;
            }

            // Deep compare.
            foreach (var categoryTitle in _cachedServiceCategories.GetCategoryTitles())
            {
                var cachedCategory = _serviceCategories.GetCategoryTitles().FirstOrDefault(a => a == categoryTitle);
                if (cachedCategory != null)
                {
                    // Compare services.
                    if (_cachedServiceCategories.GetServicesCount(categoryTitle) != _serviceCategories.GetServicesCount(categoryTitle))
                    {
                        return true;
                    }

                    foreach (var service in _cachedServiceCategories.getServicesForCategory(categoryTitle))
                    {
                        if (!_serviceCategories.getServicesForCategory(categoryTitle).Contains(service))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (ServicesHaveChanged())
            {
                var dlgResult = MessageBox.Show("Changes have been made to the services list. Continue without saving?",
                    "Close without saving", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dlgResult == DialogResult.Cancel)
                {
                    return;
                }
            }

            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool couldSaveProjectServices = ProjectServicesController.TrySaveProjectServices(_serviceCategories);
            if (!couldSaveProjectServices)
            {
                MessageBox.Show("Could not save project services to file. Ensure connection is working and try again.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            this.Close();
        }
    }
}
