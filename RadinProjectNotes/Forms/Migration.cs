﻿using RadinProjectNotes.DatabaseFiles.Controllers;
using RadinProjectNotes.DatabaseFiles.ProjectServices;
using RadinProjectNotesCommon.EncryptedDatabaseSerializer;
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
    // Take care of the following files:
    // users.db
    // All .bft files -> with default "Version 1"

    public partial class Migration : Form
    {
        const string migrationTarget = "21-235-05 Felder.bft";

        public Migration() {
            InitializeComponent();

            migrateFile(migrationTarget);
        }

        private void migrateFile(string migrationTarget) {
            string[] bfts = Directory.GetFiles(ServerConnection.serverFolder, "*.bft");
            foreach (string bft in bfts) {
                var reader = new EncryptedDatabaseSerializer<ProjectAssignedServices>(bft);

                ProjectAssignedServices data = reader.LoadDatabase();

                if (string.IsNullOrEmpty(data.versionString)) {
                    data.versionString = "Version 1";
                }

                var writer = new EncryptedDatabaseProtobufSerializer<ProjectAssignedServices>(bft);
                writer.SaveDatabase(data);
            }
        }
    }
}
