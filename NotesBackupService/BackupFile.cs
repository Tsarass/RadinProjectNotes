using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace NotesBackupService
{
    public partial class BackupFileInfo
    {
        [Serializable]
        private class BackupFile
        {
            private static readonly int maxRevisions = 8;

            [Serializable]
            public class BackupRevisionFile
            {
                public string filePath;
                public DateTime timeCreated;

                public BackupRevisionFile(string filePath)
                {
                    this.filePath = filePath;
                    this.timeCreated = DateTime.Now;
                }
            }
            public BackupFile(string filePath)
            {
                this.filePath = filePath;
                UpdateFile();
            }

            public string filePath;
            public DateTime timeLastUpdated;    //last time the file was updated (added, revised or skipped)
            public List<BackupRevisionFile> revisions = new List<BackupRevisionFile>();

            /// <summary>
            /// Updates the file's timestamp and deletes any revisions that do not exist anymore.
            /// </summary>
            public void UpdateFile()
            {
                this.timeLastUpdated = DateTime.Now;

                for (int i = revisions.Count - 1; i >= 0; i--)
                {
                    if (!File.Exists(revisions[i].filePath))
                    {
                        revisions.RemoveAt(i);
                    }
                }
            }

            public void AddRevision()
            {
                //first update to remove nonexistent revisions and update the timestamp
                UpdateFile();

                FileInfo fi = new FileInfo(this.filePath);
                string revisionFileName = GetRevisionFileName();
                fi.MoveTo(revisionFileName);

                revisions.Add(new BackupRevisionFile(revisionFileName));
                Logger.AddEntry($"Saving revision {revisionFileName}");

                if (revisions.Count > maxRevisions)
                {
                    RemoveOldestRevision();
                }
            }

            public void RemoveOldestRevision()
            {
                //oldest revision always resides at index 0 of the list
                
                if (File.Exists(revisions[0].filePath))
                {
                    Logger.AddEntry($"Removing revision {revisions[0].filePath}");
                    File.Delete(revisions[0].filePath);
                }

                revisions.RemoveAt(0);
            }

            private string GetRevisionFileName()
            {
                DateTime now = DateTime.Now;

                string trailingFileName = now.ToString("MMddHmmss", CultureInfo.InvariantCulture);

                return this.filePath + ".revision" + trailingFileName;
            }

            /// <summary>
            /// Deletes a file with all it's revisions from the backup directory.
            /// </summary>
            public void Delete()
            {
                if (File.Exists(this.filePath))
                {
                    File.Delete(this.filePath);
                    Logger.AddEntry($"Deleting obsolete file {this.filePath}");
                }

                foreach (var revision in revisions)
                {
                    if (File.Exists(revision.filePath))
                    {
                        File.Delete(revision.filePath);
                        Logger.AddEntry($"Deleting obsolete revision {revision.filePath}");
                    }
                }
            }
        }
    }
}
