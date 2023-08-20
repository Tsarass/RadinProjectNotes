using System.Windows.Forms;
using System.ComponentModel.Design;
using System.ComponentModel;
using System;
using RadinProjectNotes.DatabaseFiles.Controllers;
using RadinProjectNotes.DueItems;
using RadinProjectNotes.DatabaseFiles;
using RadinProjectNotes.HelperClasses;
using System.Drawing;

namespace RadinProjectNotes.Controls
{
    // Make the control act as a container in the form designer.
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class CalendarDueItems : UserControl
    {
        private readonly Color _completedDueItemColor = Color.Green;
        private readonly Color _expiredDueItemColor = Color.Red;

        public event EventHandler HasListItemSelected;
        public event EventHandler HasNoListItemSelected;

        public CalendarDueItems()
        {
            InitializeComponent();

            // Create a list view sorter for the due date column.
            var listviewSorter = new ListviewItemDateTimeComparer() { SortColumn = 1, Order = SortOrder.Ascending };
            calendarListView.ListViewItemSorter = listviewSorter;

            calendarListView.Sort();
        }

        internal void AddNewDueItem(DueItem dueItem)
        {
            var dueItemsDatabase = LoadAndCacheDueItemsDatabase();
            if (dueItemsDatabase is null) return;

            // Add to database and to list view.
            dueItemsDatabase.Add(dueItem);
            AddDueItemToList(dueItem);

            calendarListView.Sort();

            SaveDueItemsDatabase(dueItemsDatabase);
        }

        internal void DeleteSelectedDueItem()
        {
            if (calendarListView.SelectedItems.Count <= 0)
            {
                return;
            }

            ListViewItem selectedItem = calendarListView.SelectedItems[0];

            var dueItemsDatabase = LoadAndCacheDueItemsDatabase();
            if (dueItemsDatabase is null) return;

            // Remove from database and from list view.
            Guid? dueItemGuid = selectedItem.Tag as Guid?;
            if (dueItemGuid.HasValue)
            {
                dueItemsDatabase.Remove(dueItemGuid.Value);
                calendarListView.Items.Remove(selectedItem);
            }
            else
            {
                return;
            }

            SaveDueItemsDatabase(dueItemsDatabase);
        }

        private void SaveDueItemsDatabase(DueItemsDatabase dueItemsDatabase)
        {
            try
            {
                DueItemsDatabaseController.TrySaveDueItems(MainForm.currentProject, dueItemsDatabase);
            }
            catch (CouldNotSaveDatabase)
            {
                MessageBox.Show("Could not access the calendar due items database. Check connection and try again.", "Connection failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        /// <summary>
        /// Add a due item to the list view.
        /// </summary>
        /// <param name="dueItem"></param>
        private void AddDueItemToList(DueItem dueItem)
        {
            string createdByUsername = Credentials.Instance.GetUserDisplayNameById(dueItem.CreatedByUserId);
            string[] dueItemRow = { dueItem.Description, dueItem.DueDate.ToLocalTime().ToShortDateString(), dueItem.DueStatus.ToString(), createdByUsername };
            var listViewItem = new ListViewItem(dueItemRow);
            // Tag the due date column to be able to compare and sort dates.
            listViewItem.SubItems[1].Tag = dueItem.DueDate;
            // Tag the listview item itself to be able to identify it by id.
            listViewItem.Tag = dueItem.Id;

            listViewItem.ForeColor = GetColorForDueItem(dueItem);

            calendarListView.Items.Add(listViewItem);
        }

        private Color GetColorForDueItem(DueItem dueItem)
        {
            if (dueItem.DueStatus == DueStatus.Complete)
            {
                return _completedDueItemColor;
            }
            else if (dueItem.HasExpired())
            {
                return _expiredDueItemColor;
            }
            else
            {
                return calendarListView.ForeColor;
            }
        }

        /// <summary>
        /// Edit the list item in the indicated index with the values from the supplied due item.
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="dueItem"></param>
        private void EditDueItemInList(int itemIndex, DueItem dueItem)
        {
            calendarListView.Items[itemIndex].SubItems[0].Text = dueItem.Description;
            calendarListView.Items[itemIndex].SubItems[1].Text = dueItem.DueDate.ToShortDateString();
            calendarListView.Items[itemIndex].SubItems[2].Text = dueItem.DueStatus.ToString();
            calendarListView.Items[itemIndex].ForeColor = GetColorForDueItem(dueItem);
        }

        internal void UpdateCalendarDueItemsPanel()
        {
            if (MainForm.currentProject is null)
            {
                return;
            }

            // Clear due item list panel.
            ClearDueItemsList();

            var dueItemsDatabase = LoadAndCacheDueItemsDatabase();

            foreach (var dueItem in dueItemsDatabase.DueItems)
            {
                AddDueItemToList(dueItem);
            }

            calendarListView.Sort();
        }

        private DueItemsDatabase LoadAndCacheDueItemsDatabase()
        {
            try
            {
                return DueItemsDatabaseController.TryLoadDueItems(MainForm.currentProject);
            }
            catch (CouldNotLoadDatabase)
            {
                MessageBox.Show("Could not access the calendar due items database. Check connection and try again.", "Connection failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        private void ClearDueItemsList()
        {
            calendarListView.Items.Clear();
        }

        private void calendarListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (calendarListView.SelectedItems.Count > 0)
            {
                HasListItemSelected?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                HasNoListItemSelected?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void EditSelectedDueItem()
        {
            if (calendarListView.SelectedItems.Count <= 0)
            {
                return;
            }

            ListViewItem selectedItem = calendarListView.SelectedItems[0];

            var dueItemsDatabase = LoadAndCacheDueItemsDatabase();
            if (dueItemsDatabase is null) return;

            // Tag contains the guid of the due item.
            Guid? dueItemGuid = selectedItem.Tag as Guid?;
            if (!dueItemGuid.HasValue) return;

            DueItem selectedDueItem = dueItemsDatabase.FindById(dueItemGuid.Value);
            if (selectedDueItem is null) return;

            AddDueItem frm = new AddDueItem(MainForm.currentProject, selectedDueItem);
            var result = frm.ShowDialog();
            if (result == DialogResult.OK)
            {
                // dueItemsDatabase.Edit(selectedDueItem, frm.SavedDueItem.Description, frm.SavedDueItem.DueDate,
                //    frm.SavedDueItem.EmailsToBeNotified, frm.SavedDueItem.DueStatus);
                EditDueItemInList(calendarListView.SelectedIndices[0], selectedDueItem);
            }           

            SaveDueItemsDatabase(dueItemsDatabase);
        }

        private void calendarListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!Credentials.Instance.currentUser.CanEditCalendarDueItems())
            {
                return;
            }

            ListViewItem item = calendarListView.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                EditSelectedDueItem();
            }
        }
    }
}
