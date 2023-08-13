using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using RadinProjectNotes.DatabaseFiles;
using RadinProjectNotes.DueItems;
using RadinProjectNotes.Forms;
using RadinProjectNotes.HelperClasses;

namespace RadinProjectNotes
{
    public partial class MainForm : Form
    {
        #region Constants

        private const int projectCachedListingIntervalMinutes = 60;
        private readonly Color radinColor = Color.FromArgb(202, 158, 103);
        private readonly Color darkerRadinColor = Color.FromArgb(152, 108, 53);

        #endregion

        #region Member variables

        private DateTime latestProjectsListing;
        private bool forceProgramExit = false;

        #endregion

        public static MainForm mainForm;
        public static ProjectFolder currentProject = null;
        
        public bool minimizeOnLoad = false;

        public MainForm(string[] args)
        {
            InitializeComponent();
            HandleConsoleArgs(args);

            // Set back color to the new custom radin color.
            this.BackColor = radinColor;
        }

        private void HandleConsoleArgs(string[] args)
        {
            foreach(var arg in args)
            {
                switch (arg)
                {
                    case "-minimized":
                        minimizeOnLoad = true;
                        break;
                }
            }
        }

        //prevent flickering - use double buffering
        /*protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }*/

        #region Program startup and form load

        private void MainForm_Load(object sender, EventArgs e)
        {
            mainForm = this;

            //post assembly version
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            versionLabel.Text = "v" + version;

            //set up default text for textboxes
            this.projNrBox.Enter += new EventHandler(projNrBox_Enter);
            this.projNrBox.Leave += new EventHandler(projNrBox_Leave);
            projNrBox_SetText();
            this.projNrBox.ChangePanelFont(this.projNrBox.Font);

            //check for run on startup settings
            CheckRunOnStartup();

            // Update panels.
            InitializeServiceHostPanel();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (ShowLoginDialog(resetCredentials: false) == false)
            {
                //if login fails, return here
                return;
            }

            if (minimizeOnLoad)
            {
                this.WindowState = FormWindowState.Minimized;
                CheckForMinimizeToTray();

                minimizeOnLoad = false;
            }

            //check for updates
            AutoUpdater updater = new AutoUpdater(true, this);
            updater.CheckForUpdate();

            //load project data from folders
            LoadProjectData();

            if (Credentials.Instance.currentUser != null)
            {
                UpdateWindowDescription();
            }

            // Show latest posts
            UpdateLatestPostsList(force: true);
        }

        private void InitializeServiceHostPanel()
        {
            // Size services host panel to be the same size and location as the flow panel for notes.
            servicesHostPanel.Location = flowPanel.Location;
            servicesHostPanel.Height = flowPanel.Height;
            servicesHostPanel.Width = flowPanel.Width;
            servicesHostPanel.Anchor = flowPanel.Anchor;
            servicesHostPanel.Visible = false;
        }

        private void UpdateWindowDescription()
        {
            string userName = Credentials.Instance.currentUser.displayName;
            this.Text = Credentials.Instance.currentUser.IsAdmin ? "Project Notes || " + userName + " || Administrator" : "Project Notes || " + userName;
        }

        private void CheckRunOnStartup()
        {
            string startWithWindows = RegistryFunctions.GetRegistryKeyValue(RegistryEntry.StartWithWindows);
            if (startWithWindows == "1")
            {
                RegistryFunctions.SetAppToRunOnStartup( runOnStartup : true);
            }
            else
            {
                RegistryFunctions.SetAppToRunOnStartup(runOnStartup: false);
            }
        }

        #endregion

        /// <summary>
        /// Shows the login form and waits for user login.
        /// </summary>
        /// <param name="resetCredentials"></param>
        /// <returns>true if login was successfull</returns>
        private bool ShowLoginDialog(bool resetCredentials)
        {
            UserLogin frm = new UserLogin();
            //login form will start hidden and only show later if the automatic authentication fails
            frm.Visible = false;
            frm.ShowDialog(this, resetCredentials);

            if (frm.DialogResult == DialogResult.Cancel)
            {
                ExitApp(forceProgramExit : true);   //close application
                return false;   //returns false if unsuccessfull login
            }
            else
            {
                UpdateControlsBasedOnUserPermissions();

                // Update full comment panel with new user permissions.
                UpdateFullCommentPanel();

                ResetTabInMainPanel();

                return true;    //returns true if successfull login
            }
        }

        /// <summary>
        /// Reset to the notes tab in the main tab panel.
        /// </summary>
        private void ResetTabInMainPanel()
        {
            tabPanelSwitch.SelectedIndex = 0;
        }

        /// <summary>
        /// Update form controls based on user permissions.
        /// </summary>
        private void UpdateControlsBasedOnUserPermissions()
        {
            administratorToolStripMenuItem.Visible = Credentials.Instance.currentUser.IsAdmin;
        }

        public void LoadProjectData()
        {
            List<string> projectFolders = ServerConnection.GetAndCacheValidProjectFolders();

            projNrBox.AutoCompleteList = projectFolders;

            latestProjectsListing = DateTime.UtcNow;
        }

        private void LoadProjectNotes(string projectTitle)
        {
            if (!ServerConnection.Check())
            {
                MessageBox.Show("Could not retrieve project notes database. Check connection and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ResetTabInMainPanel();

            //update UI of form first
            this.Update();

            //clear note data from previous project
            Notes.currentNoteData = null;

            //match the project folder with the full path from cache list
            ProjectFolder projectFolder = ServerConnection.GetProjectFolderFromProjectPath(projectTitle);
            currentProject = projectFolder;
            if (projectFolder == null)
            {
                EmptyCommentPanel();
                ShowNoNotesLabel($"Could not find notes for project {projectTitle}");
            }

            //enable buttons
            btnAddComment.Enabled = true;
            btnPrint.Enabled = true;
            btnOpenFolder.Enabled = true;
            btnProjectInfo.Enabled = true;

            //open notes database
            LoadNotesDatabaseAndUpdatePanel(projectFolder);
        }

        /// <summary>
        /// Load the notes database and update the Comments panel.
        /// </summary>
        /// <param name="projectFolder"></param>
        /// <param name="onlyInMemory">true only if trying to internally load the current notes database into memory</param>
        private void LoadNotesDatabaseAndUpdatePanel(ProjectFolder projectFolder)
        {
            Notes.TryLoadNotesDatabaseInMemory(projectFolder);

            if ((Notes.currentNoteData is null) || (Notes.currentNoteData.IsEmpty) )
            {
                EmptyCommentPanel();
                ShowNoNotesLabel();
            }
            else
            {
                UpdateFullCommentPanel();
            }

            lblProject.Text = currentProject.projectPath;
        }

        #region Control event handlers

        #region Menu functions

        private void logoutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Credentials.Instance.SuccessfullyLoaded = false;
            UserLogin.ResetLoginCredentials();
            ShowLoginDialog(resetCredentials: true);
            if (Credentials.Instance.currentUser != null)
            {
                UpdateWindowDescription();
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings frm = new Settings();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.OK)
            {
                UpdateWindowDescription();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApp(forceProgramExit: true);
        }

        private void userDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminPanel frm = new AdminPanel();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        #endregion

        #region TextBox visuals functions

        string promptString = "Suche nach Projektname oder -code";
        protected void projNrBox_SetText()
        {
            this.projNrBox.Text = promptString;
            projNrBox.ForeColor = Color.Gray;
        }

        private void projNrBox_Enter(object sender, EventArgs e)
        {
            if (projNrBox.ForeColor == Color.Black)
                return;
            if (projNrBox.Text == promptString)
                projNrBox.Text = "";
            projNrBox.ForeColor = Color.Black;
        }
        private void projNrBox_Leave(object sender, EventArgs e)
        {
            if (projNrBox.Text.Trim() == "")
                projNrBox_SetText();
        }

        #endregion

        #region Edit/Delete Comment events
        private void container_EditCommentEvent(object sender, CommentPackage.EditCommentArgs e)
        {
            EditComment(e.Note);
        }

        private void container_DeleteCommentEvent(object sender, CommentPackage.EditCommentArgs e)
        {
            DeleteComment(e.Note);
        }
        
        #endregion

        private void projNrBox_ItemSelected(object sender, EventArgs e)
        {
            // Only process if the textbox actually has at least 9 characters for the project code.
            if (projNrBox.Text.Length < 9) return;

            LoadProjectNotes(projNrBox.Text.ToString());
        }

        private void btnAddComment_Click(object sender, EventArgs e)
        {
            //check user permissions
            if (!Credentials.Instance.currentUser.HasAddCommentPermission())
            {
                MessageBox.Show("Insufficient priviledges to add comment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddComment frm = new AddComment();
            frm.projectName = currentProject.projectPath;
            frm.formMode = AddComment.AddCommentFormMode.AddCommentMode;
            frm.ShowDialog(this);

            if (frm.DialogResult == DialogResult.OK)
            {
                string commentText = frm.currentNote.noteText.Trim();

                Notes.ProjectNote newNote = new Notes.ProjectNote(commentText, frm.currentNote.attachmentLibrary.GetAttachments());
                Notes.SaveNewNoteToProjectNoteDatabase(currentProject, newNote);

                UpdateFullCommentPanel();

                //Post to latest changes
#if RELEASE
                RecentChange latestChange = new RecentChange(currentProject.projectPath, Credentials.Instance.currentUser.username, DateTime.UtcNow);
                LatestPostsController.PostRecentChange(latestChange);
#endif
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
            {
                OpenFolder(currentProject.fullPath);
            }
        }

        private void flowPanel_Resize(object sender, EventArgs e)
        {
            //resize all children, triggering the comment package resize function
            foreach(Control container in flowPanel.Controls)
            {
                if (container is CommentPackage)
                {
                    container.Width = flowPanel.Width;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (Notes.currentNoteData.noteData.Count <= 0) return;

            Printer printer = new Printer(currentProject, Notes.currentNoteData);

            PrintDialog PrintDialog1 = new PrintDialog();
            PrintDialog1.Document = printer.PrintDoc;
            PrintDialog1.AllowSelection = true;
            PrintDialog1.AllowSomePages = true;
            DialogResult result = PrintDialog1.ShowDialog();

            if (result == DialogResult.OK) 
            {
                printer.Print();
            }

        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimedNoteRefresh();
            TimedProjectRefresh();
            UpdateLatestPostsList();
        }        

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoUpdater updater = new AutoUpdater(false, this);
            updater.CheckForUpdate();
        }

        private void projNrBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                projNrBox_ItemSelected(sender, new EventArgs());
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                CheckForMinimizeToTray();
            }
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RestoreAppFromTray();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreAppFromTray();
        }        

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExitApp(forceProgramExit = true);
        }

        private void btnListActiveProjects_Click(object sender, EventArgs e)
        {
            Forms.ListActiveProjects frm = new Forms.ListActiveProjects();
            DialogResult dlg = frm.ShowDialog();

            if (dlg == DialogResult.OK)
            {
                //projNrBox.Text = frm.ProjectToOpen;
                
                LoadProjectNotes(frm.ProjectToOpen);
            }
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            Forms.AboutForm frm = new Forms.AboutForm();
            frm.ShowDialog();
        }

        private void latestPostsView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = latestPostsView.GetItemAt(e.X, e.Y);

            string projectTitle = item.SubItems[0].Text;

            LoadProjectNotes(projectTitle);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if flag is true, exit without confirmation
            if (forceProgramExit)
            {
                return;
            }

            if (RegistryFunctions.GetRegistryKeyValue(RegistryEntry.MinimizeInsteadOfProgramExit) == "1")
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                CheckForMinimizeToTray();

                return;
            }

            MainFormConfirmationDialog dlg = new MainFormConfirmationDialog();
            dlg.ShowDialog();
            var result = dlg.dialogResult;

            if (result == MainFormConfirmationDialog.ExitDialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else if (result == MainFormConfirmationDialog.ExitDialogResult.Exit)
            {
                //close form normally
            }
            else if (result == MainFormConfirmationDialog.ExitDialogResult.StayInBackground)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                CheckForMinimizeToTray();
            }
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string projectTitle = latestPostsView.SelectedItems[0].Text;

            LoadProjectNotes(projectTitle);
        }

        private void latestNotesMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (latestPostsView.SelectedItems.Count <= 0)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Comment panel methods
        public void EditComment(Notes.ProjectNote note)
        {
            Notes.ProjectNote noteToEdit;
            try
            {
                //get a match from current project notes list
                noteToEdit = Notes.currentNoteData.FindNote(note);
            }
            catch (Versioning.SaveStructureV1.NoteNotFoundInDatabase ex)
            {
                MessageBox.Show(this, ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateFullCommentPanel();
                return;
            }

            if (!Credentials.Instance.currentUser.CanEditNote(noteToEdit))
            {
                MessageBox.Show(this, "User not authorized to edit note.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!noteToEdit.IsWithinAllowedIntervalToEdit() && (!Credentials.Instance.currentUser.IsAdmin))
            {
                string prompt = "Comments can only be edited within " + Notes.maxEditHours + " hours of creation!";
                MessageBox.Show(this, prompt, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddComment frm = new AddComment();
            frm.projectName = currentProject.projectPath;
            frm.formMode = AddComment.AddCommentFormMode.EditCommentMode;
            frm.currentNote = noteToEdit;
            frm.ShowDialog(this);

            if (frm.DialogResult == DialogResult.OK)
            {
                string commentText = frm.currentNote.noteText;

                //first make a deep copy of the note that will be edited
                Notes.ProjectNote noteCopy = (Notes.ProjectNote)noteToEdit.Clone();

                //then open the file from server
                //required to have simultaneous multi-user access to same project notes
                Notes.TryLoadNotesDatabaseInMemory(currentProject);

                //lastly find the old note in the new note list
                noteToEdit = matchOldNoteInNewNoteList(noteCopy);

                noteToEdit.noteText = commentText;
                noteToEdit.attachmentLibrary.SetAttachments(frm.currentNote.attachmentLibrary.GetAttachments());
                noteToEdit.dateLastEdited = DateTime.UtcNow.Ticks;

                UpdateCommentPanelNote(noteToEdit);

                Notes.TrySaveNotesDatabase(currentProject);
            }
        }

        public void DeleteComment(Notes.ProjectNote note)
        {
            Notes.ProjectNote noteToDelete;
            try
            {
                //get a match from current project notes list
                noteToDelete = Notes.currentNoteData.FindNote(note);
            }
            catch (Versioning.SaveStructureV1.NoteNotFoundInDatabase ex)
            {
                MessageBox.Show(this, ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateFullCommentPanel();
                return;
            }

            if (!Credentials.Instance.currentUser.CanDeleteNote(noteToDelete))
            {
                MessageBox.Show(this, "User not authorized to delete note.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!noteToDelete.IsWithinAllowedIntervalToDelete() && (!Credentials.Instance.currentUser.IsAdmin))
            {
                string prompt = "Comments can only be edited within " + Notes.maxDeleteHours / 24 + " days of creation!";
                MessageBox.Show(this, prompt, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string promptText = "Are you sure you want to delete this comment?";
            DialogResult result = MessageBox.Show(this, promptText, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //first make a deep copy of the note that will be deleted
                Notes.ProjectNote noteCopy = (Notes.ProjectNote)noteToDelete.Clone();

                //then open the file from server
                //required to have simultaneous multi-user access to same project notes
                //call the procedure which loads the database file and finds any changes
                TimedNoteRefresh();

                //lastly find the old note in the new note list
                noteToDelete = matchOldNoteInNewNoteList(noteCopy);

                if (noteToDelete is null)
                {
                    MessageBox.Show(this, "Could not find note in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //delete comment
                Notes.currentNoteData.DeleteNote(noteToDelete);
                Notes.currentNote = null;
                Notes.TrySaveNotesDatabase(currentProject);
                DeleteCommentPanelNote(noteToDelete);
            }
        }

        private Notes.ProjectNote matchOldNoteInNewNoteList(Notes.ProjectNote noteCopy)
        {
            foreach (Notes.ProjectNote note in Notes.currentNoteData.noteData)
            {
                if (note == noteCopy)
                {
                    return note;
                }
            }

            return null;
        }

        private void EmptyCommentPanel()
        {
            this.flowPanel.Controls.Clear();
        }

        private void UpdateFullCommentPanel()
        {
            Point CurrentPoint = flowPanel.AutoScrollPosition;
            EmptyCommentPanel();

            //if there are no data, return
            if (Notes.currentNoteData == null) { return; }

            this.flowPanel.SuspendLayout();
            foreach (Notes.ProjectNote note in Notes.currentNoteData.noteData)
            {
                CommentPackage pack = AddCommentPackage(note);
            }
            this.flowPanel.ResumeLayout();
            flowPanel.AutoScrollPosition = new Point(Math.Abs(flowPanel.AutoScrollPosition.X), Math.Abs(CurrentPoint.Y));
        }

        private void UpdateCommentPanelNote(Notes.ProjectNote note)
        {
            foreach (var control in flowPanel.Controls)
            {
                if (control is CommentPackage)
                {
                    CommentPackage pack = (CommentPackage)control;
                    if (pack.Note == note)
                    {
                        pack.Note = note;
                        pack.UpdatePanel();
                    }
                }
            }
        }

        private void DeleteCommentPanelNote(Notes.ProjectNote note)
        {
            foreach (var control in flowPanel.Controls)
            {
                if (control is CommentPackage)
                {
                    CommentPackage pack = (CommentPackage)control;
                    if (pack.Note == note)
                    {
                        pack.Dispose();
                    }
                }
            }
        }

        private void AddCommentPanelNote(Notes.ProjectNote note)
        {
            AddCommentPackage(note);
        }

        private CommentPackage AddCommentPackage(Notes.ProjectNote note)
        {
            CommentPackage pack = new CommentPackage();
            pack.Note = note;

            //button visuals
            if (Credentials.Instance.currentUser.HasAuthorizationToEditOrDeleteNote(note))
            {
                pack.Editable = true;
                pack.Deleteable = true;
            }
            else
            {
                pack.Editable = false;
                pack.Deleteable = false;
            }

            //add events to the container
            pack.EditCommentEvent += new EventHandler<CommentPackage.EditCommentArgs>(container_EditCommentEvent);
            pack.DeleteCommentEvent += new EventHandler<CommentPackage.EditCommentArgs>(container_DeleteCommentEvent);

            this.flowPanel.Controls.Add(pack);

            return pack;
        }

        #endregion

        #region UI update methods

        private void ShowNoNotesLabel(string customText = "")
        {
            Label lbl = new Label();
            lbl.Text = customText == "" ? noNotesLbl.Text : customText;
            lbl.Font = noNotesLbl.Font;
            lbl.Width = noNotesLbl.Width;
            this.flowPanel.Controls.Add(lbl);
        }

        public void RestoreAppFromTray()
        {
            trayIcon.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void CheckForMinimizeToTray()
        {
            if (RegistryFunctions.GetRegistryKeyValue(RegistryEntry.MinimizeToSystemTray) == "1")
            {
                HideToTray();
            }
        }

        private void HideToTray()
        {
            this.Hide();
            trayIcon.Visible = true;
            trayIcon.ShowBalloonTip(0);
        }

        public void ExitApp(bool forceProgramExit = false)
        {
            this.forceProgramExit = forceProgramExit;
            this.Close();
            System.Windows.Forms.Application.Exit();
        }

        private void TimedProjectRefresh()
        {
            if (DateTime.UtcNow.Subtract(latestProjectsListing).TotalMinutes > projectCachedListingIntervalMinutes)
            {
                LoadProjectData();
            }
        }

        private void OpenFolder(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        Arguments = folderPath,
                        FileName = "explorer.exe"
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
                }
            }
            catch
            {

            }
        }

        private void TimedNoteRefresh()
        {
            //check if a project is open
            if (currentProject is null) { return; }
            if (Notes.currentNoteData is null) { return; }

            Versioning.SaveStructureV1 loadedNoteData = null;

            //load the database for the current project
            //and check if the last saved time has changed
            var maxRetryAttempts = 5;
            var pauseBetweenFailures = TimeSpan.FromMilliseconds(200);
            RetryHelper.RetryOnException(maxRetryAttempts, pauseBetweenFailures, () => {
                loadedNoteData = Notes.LoadDatabaseFile(currentProject);
            });

            if (loadedNoteData != null)
            {
                if (loadedNoteData.lastSavedTime > Notes.currentNoteData.lastSavedTime) //newer database detected
                {
                    //first make a deep copy of the current note database to use as a stack
                    //remove any already processed notes from this stack and if anything is left, treat is as deleted notes
                    List<Notes.ProjectNote> currentNoteTempStack = new List<Notes.ProjectNote>(Notes.currentNoteData.noteData);

                    currentNoteTempStack = AddNewOrUpdateExistingNotes(loadedNoteData, currentNoteTempStack);

                    //any items left in the stack are notes that are to be deleted
                    DeleteLeftoverNotes(currentNoteTempStack);

                    //in the end swap the current database with the newer one
                    Notes.currentNoteData = loadedNoteData;
                    Debug.WriteLine("Found change in the database @ " + DateTime.Now.ToString());
                }
            }
        }

        private void DeleteLeftoverNotes(List<Notes.ProjectNote> currentNoteTempStack)
        {
            foreach (Notes.ProjectNote note in currentNoteTempStack)
            {
                DeleteCommentPanelNote(note);
            }
        }

        /// <summary>
        /// Loops through all the notes in the newer database and adds any new notes or updates existing ones.
        /// </summary>
        /// <param name="loadedNoteData"></param>
        /// <param name="currentNoteTempStack"></param>
        private List<Notes.ProjectNote> AddNewOrUpdateExistingNotes(Versioning.SaveStructureV1 loadedNoteData, List<Notes.ProjectNote> currentNoteTempStack)
        {
            foreach (Notes.ProjectNote note in loadedNoteData.noteData)
            {
                int ind = currentNoteTempStack.IndexOf(note);
                if (noteNotFound(ind)) //we have a brand new note
                {
                    AddCommentPanelNote(note);
                }
                else
                {
                    //this note already exists, update it
                    currentNoteTempStack.RemoveAt(ind);
                    UpdateCommentPanelNote(note);
                }
            }

            return currentNoteTempStack;
        }

        private static bool noteNotFound(int ind)
        {
            return ind < 0;
        }

        private void UpdateLatestPostsList(bool force = false)
        {
            if ((!LatestPostsController.IsUpdateRequired()) && !force)
            {
                Debug.WriteLine("Skipping latest post list update...");
                return;
            }

            Debug.WriteLine("Updating latest post list update...");
            latestPostsView.Items.Clear();

            var latestUniquePosts = GetUniqueProjectLatestPosts();

            //need to reverse results to get the newer ones first on the list
            latestUniquePosts.Reverse();
            //show max of 10 latest posts
            int latestPostsShown = 10;
            foreach (var post in latestUniquePosts)
            {
                string[] itemStrings = new string[] { post.projectTitle, post.username, new DateTime(post.dateTimeTicks).ToString() };
                ListViewItem newItem = new ListViewItem(itemStrings, 0);
                latestPostsView.Items.Add(newItem);
                if (--latestPostsShown <= 0)
                {
                    break;
                }
            }
        }

        private List<RecentChange> GetUniqueProjectLatestPosts()
        {
            var latestPosts = LatestPostsController.GetLatestPosts();
            List<RecentChange> uniqueProjects = new List<RecentChange>();

            foreach (var post in latestPosts)
            {
                var containedPost = uniqueProjects.Find(x => x.projectTitle == post.projectTitle);
                if (containedPost != null)
                {
                    //check if post is more recent
                    if (post.dateTimeTicks > containedPost.dateTimeTicks)
                    {
                        //update values from other post
                        containedPost.dateTimeTicks = post.dateTimeTicks;
                        containedPost.username = post.username;
                    }
                }
                else
                {
                    uniqueProjects.Add(post);
                }
            }

            return uniqueProjects;
        }

        #endregion

        /// <summary>
        /// Open the project contact info spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProjectInfo_Click(object sender, EventArgs e)
        {
            if (currentProject is null) return;

            ProjectInfosSpreadsheetController spreadsheetController = new ProjectInfosSpreadsheetController(currentProject);
            try
            {
                spreadsheetController.openSpreadsheet();
            }
            catch (ProjectInfosSpreadsheetController.TemplateNotFound)
            {
                MessageBox.Show("Project infos spreadsheet is missing from the current project. " +
                    "Attempting to copy template speadsheet failed: \n" +
                    "Could not find template spreadsheet in the server.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Could not open project infos spreadsheet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void projectServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectServicesDialog dialog = new ProjectServicesDialog();
            dialog.ShowDialog();
        }

        private void tabPanelSwitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabPanelSwitch.SelectedIndex == 1)
            {
                servicesHostPanel.UpdateServicesPanel();

                flowPanel.Visible = false;
                btnAddComment.Visible = false;
                btnPrint.Visible = false;
                servicesHostPanel.Visible = true;
            }
            else
            {
                servicesHostPanel.ServicesPanelClosing();

                flowPanel.Visible = true;
                btnAddComment.Visible = true;
                btnPrint.Visible = true;
                servicesHostPanel.Visible = false;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // If the main form has closed, reset the tab in the main panel to try and save changes.
            ResetTabInMainPanel();
        }

        private void btnNewDueItem_Click(object sender, EventArgs e)
        {
            AddDueItem frm = new AddDueItem(currentProject);
            var result = frm.ShowDialog();
            if (result == DialogResult.OK)
            {
                DueItemsDatabase dueItemsDatabase;
                try
                {
                    dueItemsDatabase = DueItemsDatabaseController.TryLoadDueItems();
                }
                catch (CouldNotLoadDatabase)
                {
                    MessageBox.Show("Could not access due items database file. Ensure connection is working and try again.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                dueItemsDatabase.Add(frm.SavedDueItem);

                bool couldSave = DueItemsDatabaseController.TrySaveDueItems(dueItemsDatabase);
                if (!couldSave)
                {
                    MessageBox.Show("Could not save to due items database file. Ensure connection is working and try again.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void btnEditDueItem_Click(object sender, EventArgs e)
        {
            DueItemsDatabase dueItemsDatabase;
            try
            {
                dueItemsDatabase = DueItemsDatabaseController.TryLoadDueItems();
            }
            catch (CouldNotLoadDatabase)
            {
                MessageBox.Show("Could not access due items database file. Ensure connection is working and try again.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dueItemsDatabase.DueItems.Count > 0)
            {
                AddDueItem frm = new AddDueItem(currentProject, dueItemsDatabase.DueItems[dueItemsDatabase.DueItems.Count - 1]);
                var result = frm.ShowDialog();
            }
        }
    }
}
