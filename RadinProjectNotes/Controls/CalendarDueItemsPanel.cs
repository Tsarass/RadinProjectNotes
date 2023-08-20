using System.Windows.Forms;
using System.ComponentModel.Design;
using System.ComponentModel;
using System;
using RadinProjectNotes.DatabaseFiles.Controllers;
using RadinProjectNotes.HelperClasses;
using System.Drawing;
using DueItems;
using EncryptedDatabaseSerializer;

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

        private DueItemsDatabase _cachedDatabase;
        private DueItem _selectedDueItem;

        public CalendarDueItems()
        {
            InitializeComponent();

            // Create a list view sorter for the due date column.
            var listviewSorter = new ListviewItemDateTimeComparer() { SortColumn = 1, Order = SortOrder.Ascending };
            calendarListView.ListViewItemSorter = listviewSorter;

            calendarListView.Sort();
        }

        /// <summary>The due item currently selected in the list view.</summary>
        public DueItem SelectedDueItem { get { return _selectedDueItem; } }

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

        /// <summary>
        /// Add a due item to the list view.
        /// </summary>
        /// <param name="dueItem"></param>
        private void AddDueItemToList(DueItem dueItem)
        {
            string createdByUsername = Credentials.Instance.GetUserDisplayNameById(dueItem.CreatedByUserId);
            string[] dueItemRow = { dueItem.Description, dueItem.DueDate.ToLocalTime().ToShortDateString(), dueItem.GetStatusText(), createdByUsername };
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
            if (dueItem.IsCompleted)
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
            calendarListView.Items[itemIndex].SubItems[2].Text = dueItem.GetStatusText();
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

           _cachedDatabase = LoadAndCacheDueItemsDatabase();

            foreach (var dueItem in _cachedDatabase.DueItems)
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

        /// <summary>
        /// When the selected index in the list view changes, the selected due item is cached for the outside world to query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calendarListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (calendarListView.SelectedItems.Count > 0)
            {
                _selectedDueItem = GetSelectedDueItemFromCachedDatabase();
                HasListItemSelected?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                HasNoListItemSelected?.Invoke(this, EventArgs.Empty);
            }
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

        internal void EditSelectedDueItem()
        {
            // Load the database from file first.
            _cachedDatabase = LoadAndCacheDueItemsDatabase();
            if (_cachedDatabase is null) return;

            _selectedDueItem = GetSelectedDueItemFromCachedDatabase();
            if (_selectedDueItem is null) return;

            AddDueItem frm = new AddDueItem(_selectedDueItem);
            var result = frm.ShowDialog();
            if (result == DialogResult.OK)
            {
                _cachedDatabase.Edit(Credentials.Instance.currentUser.ID, _selectedDueItem, frm.EditedDueItemState);
                EditDueItemInList(calendarListView.SelectedIndices[0], _selectedDueItem);
            }

            SaveDueItemsDatabase(_cachedDatabase);
        }

        internal void DeleteSelectedDueItem()
        {
            // Load the database from file first.
            _cachedDatabase = LoadAndCacheDueItemsDatabase();
            if (_cachedDatabase is null) return;

            _selectedDueItem = GetSelectedDueItemFromCachedDatabase();
            if (_selectedDueItem is null) return;

            _cachedDatabase.Remove(_selectedDueItem);
            calendarListView.Items.Remove(calendarListView.SelectedItems[0]);

            SaveDueItemsDatabase(_cachedDatabase);
        }

        internal void SetSelectedDueItemCompletedStatus(bool completed)
        {
            // Load the database from file first.
            _cachedDatabase = LoadAndCacheDueItemsDatabase();
            if (_cachedDatabase is null) return;

            _selectedDueItem = GetSelectedDueItemFromCachedDatabase();
            if (_selectedDueItem is null) return;

            _selectedDueItem.SetCompletedStatus(completed);
            calendarListView.SelectedItems[0].SubItems[2].Text = _selectedDueItem.GetStatusText();
            calendarListView.SelectedItems[0].ForeColor = GetColorForDueItem(_selectedDueItem);

            SaveDueItemsDatabase(_cachedDatabase);
        }

        /// <summary>
        /// Get the due item selected in the list view.
        /// </summary>
        /// <returns>The due item that is selected or null if it was not found in the database.</returns>
        private DueItem GetSelectedDueItemFromCachedDatabase()
        {
            if (calendarListView.SelectedItems.Count <= 0) return null;
            ListViewItem selectedItem = calendarListView.SelectedItems[0];

            // Remove from database and from list view.
            Guid? dueItemGuid = selectedItem.Tag as Guid?;
            if (!dueItemGuid.HasValue)
            {
                return null; 
            }

            return _cachedDatabase.FindById(dueItemGuid.Value);
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
    }
}
