using RadinProjectNotesCommon;
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

            private string _filePath;
            private DateTime _timeLastUpdated;
            private List<BackupRevisionFile> _revisions = new List<BackupRevisionFile>();
            private Logger _logger;
            private int _maxRevisions;

            public BackupFile(string filePath, int maxRevisions, Logger logger)
            {
                _filePath = filePath;
                _logger = logger;
                _maxRevisions = maxRevisions;

                UpdateFile();
            }

            /// <summary>Get the file's filepath.</summary>
            public string FilePath { get { return _filePath; } }

            /// <summary>Get the file's last update time (added, revised or skipped).</summary>
            public DateTime TimeLastUpdated { get { return _timeLastUpdated; } }

            /// <summary>
            /// Updates the file's timestamp and deletes any revisions that do not exist anymore.
            /// </summary>
            public void UpdateFile()
            {
                this._timeLastUpdated = DateTime.Now;

                for (int i = _revisions.Count - 1; i >= 0; i--)
                {
                    if (!File.Exists(_revisions[i].filePath))
                    {
                        _revisions.RemoveAt(i);
                    }
                }
            }

            public void AddRevision()
            {
                //first update to remove nonexistent revisions and update the timestamp
                UpdateFile();

                FileInfo fi = new FileInfo(this._filePath);
                string revisionFileName = GetRevisionFileName();
                fi.MoveTo(revisionFileName);

                _revisions.Add(new BackupRevisionFile(revisionFileName));
                _logger.AddEntry($"Saving revision {revisionFileName}");

                if (_revisions.Count > _maxRevisions)
                {
                    RemoveOldestRevision();
                }
            }

            public void RemoveOldestRevision()
            {
                //oldest revision always resides at index 0 of the list
                
                if (File.Exists(_revisions[0].filePath))
                {
                    _logger.AddEntry($"Removing revision {_revisions[0].filePath}");
                    File.Delete(_revisions[0].filePath);
                }

                _revisions.RemoveAt(0);
            }

            private string GetRevisionFileName()
            {
                DateTime now = DateTime.Now;

                string trailingFileName = now.ToString("MMddHmmss", CultureInfo.InvariantCulture);

                return this._filePath + ".revision" + trailingFileName;
            }

            /// <summary>
            /// Deletes a file with all it's revisions from the backup directory.
            /// </summary>
            public void Delete()
            {
                if (File.Exists(this._filePath))
                {
                    File.Delete(this._filePath);
                    _logger.AddEntry($"Deleting obsolete file {this._filePath}");
                }

                foreach (var revision in _revisions)
                {
                    if (File.Exists(revision.filePath))
                    {
                        File.Delete(revision.filePath);
                        _logger.AddEntry($"Deleting obsolete revision {revision.filePath}");
                    }
                }
            }
        }
    }
}
