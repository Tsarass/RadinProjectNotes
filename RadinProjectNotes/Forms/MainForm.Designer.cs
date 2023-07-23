namespace RadinProjectNotes
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnAddComment = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnListActiveProjects = new System.Windows.Forms.Button();
            this.btnProjectInfo = new System.Windows.Forms.Button();
            this.btnNewDueItem = new System.Windows.Forms.Button();
            this.versionLabel = new System.Windows.Forms.Label();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectServicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsTimer = new System.Windows.Forms.Timer(this.components);
            this.noNotesLbl = new System.Windows.Forms.Label();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProject = new System.Windows.Forms.Label();
            this.latestPostsView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.latestNotesMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.latestPostsImageList = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPanelSwitch = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.servicesHostPanel = new RadinProjectNotes.Controls.AssignedServicesPanel();
            this.projNrBox = new AutoCompleteTextBoxSample.AutoCompleteTextbox();
            this.mainMenu.SuspendLayout();
            this.trayMenu.SuspendLayout();
            this.latestNotesMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPanelSwitch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.BackgroundImage = global::RadinProjectNotes.Properties.Resources.printer;
            this.btnPrint.Name = "btnPrint";
            this.toolTips.SetToolTip(this.btnPrint, resources.GetString("btnPrint.ToolTip"));
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnAddComment
            // 
            resources.ApplyResources(this.btnAddComment, "btnAddComment");
            this.btnAddComment.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddComment.BackgroundImage = global::RadinProjectNotes.Properties.Resources.add_note;
            this.btnAddComment.Name = "btnAddComment";
            this.toolTips.SetToolTip(this.btnAddComment, resources.GetString("btnAddComment.ToolTip"));
            this.btnAddComment.UseVisualStyleBackColor = false;
            this.btnAddComment.Click += new System.EventHandler(this.btnAddComment_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.BackgroundImage = global::RadinProjectNotes.Properties.Resources.folder;
            resources.ApplyResources(this.btnOpenFolder, "btnOpenFolder");
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.toolTips.SetToolTip(this.btnOpenFolder, resources.GetString("btnOpenFolder.ToolTip"));
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // btnListActiveProjects
            // 
            this.btnListActiveProjects.BackgroundImage = global::RadinProjectNotes.Properties.Resources.filter;
            resources.ApplyResources(this.btnListActiveProjects, "btnListActiveProjects");
            this.btnListActiveProjects.Name = "btnListActiveProjects";
            this.toolTips.SetToolTip(this.btnListActiveProjects, resources.GetString("btnListActiveProjects.ToolTip"));
            this.btnListActiveProjects.UseVisualStyleBackColor = true;
            this.btnListActiveProjects.Click += new System.EventHandler(this.btnListActiveProjects_Click);
            // 
            // btnProjectInfo
            // 
            this.btnProjectInfo.BackgroundImage = global::RadinProjectNotes.Properties.Resources.info;
            resources.ApplyResources(this.btnProjectInfo, "btnProjectInfo");
            this.btnProjectInfo.Name = "btnProjectInfo";
            this.toolTips.SetToolTip(this.btnProjectInfo, resources.GetString("btnProjectInfo.ToolTip"));
            this.btnProjectInfo.UseVisualStyleBackColor = true;
            this.btnProjectInfo.Click += new System.EventHandler(this.btnProjectInfo_Click);
            // 
            // btnNewDueItem
            // 
            resources.ApplyResources(this.btnNewDueItem, "btnNewDueItem");
            this.btnNewDueItem.BackColor = System.Drawing.SystemColors.Control;
            this.btnNewDueItem.BackgroundImage = global::RadinProjectNotes.Properties.Resources.add_note;
            this.btnNewDueItem.Name = "btnNewDueItem";
            this.toolTips.SetToolTip(this.btnNewDueItem, resources.GetString("btnNewDueItem.ToolTip"));
            this.btnNewDueItem.UseVisualStyleBackColor = false;
            this.btnNewDueItem.Click += new System.EventHandler(this.btnNewDueItem_Click);
            // 
            // versionLabel
            // 
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Click += new System.EventHandler(this.versionLabel_Click);
            // 
            // flowPanel
            // 
            resources.ApplyResources(this.flowPanel, "flowPanel");
            this.flowPanel.BackColor = System.Drawing.Color.White;
            this.flowPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Resize += new System.EventHandler(this.flowPanel_Resize);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.administratorToolStripMenuItem});
            resources.ApplyResources(this.mainMenu, "mainMenu");
            this.mainMenu.Name = "mainMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            resources.ApplyResources(this.logoutToolStripMenuItem, "logoutToolStripMenuItem");
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click_1);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            resources.ApplyResources(this.preferencesToolStripMenuItem, "preferencesToolStripMenuItem");
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            resources.ApplyResources(this.checkForUpdatesToolStripMenuItem, "checkForUpdatesToolStripMenuItem");
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // administratorToolStripMenuItem
            // 
            this.administratorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userDatabaseToolStripMenuItem,
            this.projectServicesToolStripMenuItem});
            this.administratorToolStripMenuItem.Name = "administratorToolStripMenuItem";
            resources.ApplyResources(this.administratorToolStripMenuItem, "administratorToolStripMenuItem");
            // 
            // userDatabaseToolStripMenuItem
            // 
            this.userDatabaseToolStripMenuItem.Name = "userDatabaseToolStripMenuItem";
            resources.ApplyResources(this.userDatabaseToolStripMenuItem, "userDatabaseToolStripMenuItem");
            this.userDatabaseToolStripMenuItem.Click += new System.EventHandler(this.userDatabaseToolStripMenuItem_Click);
            // 
            // projectServicesToolStripMenuItem
            // 
            this.projectServicesToolStripMenuItem.Name = "projectServicesToolStripMenuItem";
            resources.ApplyResources(this.projectServicesToolStripMenuItem, "projectServicesToolStripMenuItem");
            this.projectServicesToolStripMenuItem.Click += new System.EventHandler(this.projectServicesToolStripMenuItem_Click);
            // 
            // eventsTimer
            // 
            this.eventsTimer.Enabled = true;
            this.eventsTimer.Interval = 8000;
            this.eventsTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // noNotesLbl
            // 
            resources.ApplyResources(this.noNotesLbl, "noNotesLbl");
            this.noNotesLbl.Name = "noNotesLbl";
            // 
            // trayIcon
            // 
            resources.ApplyResources(this.trayIcon, "trayIcon");
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem1});
            this.trayMenu.Name = "trayMenu";
            resources.ApplyResources(this.trayMenu, "trayMenu");
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            resources.ApplyResources(this.restoreToolStripMenuItem, "restoreToolStripMenuItem");
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            resources.ApplyResources(this.exitToolStripMenuItem1, "exitToolStripMenuItem1");
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblProject
            // 
            resources.ApplyResources(this.lblProject, "lblProject");
            this.lblProject.Name = "lblProject";
            // 
            // latestPostsView
            // 
            resources.ApplyResources(this.latestPostsView, "latestPostsView");
            this.latestPostsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.latestPostsView.ContextMenuStrip = this.latestNotesMenuStrip;
            this.latestPostsView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.latestPostsView.HideSelection = false;
            this.latestPostsView.LargeImageList = this.latestPostsImageList;
            this.latestPostsView.MultiSelect = false;
            this.latestPostsView.Name = "latestPostsView";
            this.latestPostsView.UseCompatibleStateImageBehavior = false;
            this.latestPostsView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.latestPostsView_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // latestNotesMenuStrip
            // 
            this.latestNotesMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openProjectToolStripMenuItem});
            this.latestNotesMenuStrip.Name = "latestNotesMenuStrip";
            resources.ApplyResources(this.latestNotesMenuStrip, "latestNotesMenuStrip");
            this.latestNotesMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.latestNotesMenuStrip_Opening);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            resources.ApplyResources(this.openProjectToolStripMenuItem, "openProjectToolStripMenuItem");
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // latestPostsImageList
            // 
            this.latestPostsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("latestPostsImageList.ImageStream")));
            this.latestPostsImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.latestPostsImageList.Images.SetKeyName(0, "folder_static.png");
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::RadinProjectNotes.Properties.Resources.search;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // tabPanelSwitch
            // 
            resources.ApplyResources(this.tabPanelSwitch, "tabPanelSwitch");
            this.tabPanelSwitch.Controls.Add(this.tabPage1);
            this.tabPanelSwitch.Controls.Add(this.tabPage2);
            this.tabPanelSwitch.Multiline = true;
            this.tabPanelSwitch.Name = "tabPanelSwitch";
            this.tabPanelSwitch.SelectedIndex = 0;
            this.tabPanelSwitch.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabPanelSwitch.SelectedIndexChanged += new System.EventHandler(this.tabPanelSwitch_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // servicesHostPanel
            // 
            this.servicesHostPanel.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.servicesHostPanel, "servicesHostPanel");
            this.servicesHostPanel.Name = "servicesHostPanel";
            // 
            // projNrBox
            // 
            this.projNrBox.AutoCompleteList = ((System.Collections.Generic.List<string>)(resources.GetObject("projNrBox.AutoCompleteList")));
            this.projNrBox.CaseSensitive = false;
            resources.ApplyResources(this.projNrBox, "projNrBox");
            this.projNrBox.MinTypedCharacters = 2;
            this.projNrBox.Name = "projNrBox";
            this.projNrBox.SelectedIndex = -1;
            this.projNrBox.AutocompleteListItemSelected += new System.EventHandler(this.projNrBox_ItemSelected);
            this.projNrBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.projNrBox_KeyUp);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.servicesHostPanel);
            this.Controls.Add(this.tabPanelSwitch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.latestPostsView);
            this.Controls.Add(this.lblProject);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.projNrBox);
            this.Controls.Add(this.noNotesLbl);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnNewDueItem);
            this.Controls.Add(this.btnAddComment);
            this.Controls.Add(this.btnProjectInfo);
            this.Controls.Add(this.btnListActiveProjects);
            this.Controls.Add(this.btnOpenFolder);
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.trayMenu.ResumeLayout(false);
            this.latestNotesMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPanelSwitch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAddComment;
        private AutoCompleteTextBoxSample.AutoCompleteTextbox projNrBox;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private System.Windows.Forms.Timer eventsTimer;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administratorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.Label noNotesLbl;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.Button btnListActiveProjects;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProject;
        private System.Windows.Forms.ListView latestPostsView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ContextMenuStrip latestNotesMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ImageList latestPostsImageList;
        private System.Windows.Forms.Button btnProjectInfo;
        private System.Windows.Forms.ToolStripMenuItem projectServicesToolStripMenuItem;
        private System.Windows.Forms.TabControl tabPanelSwitch;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Controls.AssignedServicesPanel servicesHostPanel;
        private System.Windows.Forms.Button btnNewDueItem;
    }
}

