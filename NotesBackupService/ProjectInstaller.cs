﻿using System.ComponentModel;
using System.Configuration.Install;

namespace NotesBackupService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
