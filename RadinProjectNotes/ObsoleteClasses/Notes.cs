/*
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public class Notes
    {
        public static string GetCurrentProjectAttachmentsFolder()
        {
            string projectName = Form1.currentProject.projectPath;
            string attachmentsFolder = Path.Combine(Form1.serverFolder, projectName);
            return attachmentsFolder;
        }

        [ProtoContract]
        public enum NoteStatus
        {
            Pending = 0
        }

        [ProtoContract]
        public class ProjectNote : ICloneable
        {
            [ProtoMember(1)]
            public string noteText;          //text content of the note
            [ProtoMember(2)]
            public User userCreated;         //user that made the comment
            [ProtoMember(3)]
            public long dateAdded;           //date comment was added (ticks since Unix era)
            [ProtoMember(4)]
            private string pcName;           //computer name
            [ProtoMember(5)]
            public long dateLastEdited;      //date comment was last edited (ticks since Unix era)
            [ProtoMember(6, IsRequired = true)]
            private NoteStatus noteStatus;   //can be pending, completed etc
            [ProtoMember(7)]
            private User userFullfilled;     //name of the user that fullfilled the request of the comment (change status?)
            [ProtoMember(8)]
            public AttachmentLibrary attachmentLibrary = new AttachmentLibrary();

            public ProjectNote()
            {
                //parameterless constructor for protobuf
            }

            public ProjectNote(string text)
            {
                //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string userName = Properties.Settings.Default.UserName;
                string pcName = Environment.UserName;

                this.noteText = text;
                this.userCreated = ServerConnection.credentials.currentUser;
                this.dateAdded = DateTime.UtcNow.Ticks;
                this.pcName = pcName;
                this.dateLastEdited = -1;
                this.noteStatus = NoteStatus.Pending;
            }

            public ProjectNote(string text, List<Attachment> attachments)
            {


                //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string userName = Properties.Settings.Default.UserName;
                string pcName = Environment.UserName;

                this.noteText = text;
                this.userCreated = ServerConnection.credentials.currentUser;
                this.dateAdded = DateTime.UtcNow.Ticks;
                this.pcName = pcName;
                this.dateLastEdited = -1;
                this.noteStatus = NoteStatus.Pending;
                this.attachmentLibrary.SetAttachments(attachments);
            }

            public object Clone()
            {
                ProjectNote newNote = new ProjectNote();

                newNote.noteText = this.noteText;
                newNote.userCreated = this.userCreated;
                newNote.dateAdded = this.dateAdded;
                newNote.pcName = this.pcName;
                newNote.dateLastEdited = this.dateLastEdited;
                newNote.noteStatus = this.noteStatus;
                newNote.userFullfilled = this.userFullfilled;
                newNote.attachmentLibrary.attachments = new List<Attachment>(this.attachmentLibrary.attachments);   //deep copy

                return newNote;
            }

            public string DateCreatedString
            {
                get
                {
                    DateTime date = new DateTime(this.dateAdded, DateTimeKind.Utc).ToLocalTime();
                    return date.ToString();
                }
            }

            public string DateEditedString
            {
                get
                {
                    if (this.dateLastEdited > 0)
                    {
                        DateTime date = new DateTime(this.dateLastEdited, DateTimeKind.Utc).ToLocalTime();
                        return date.ToString();
                    }
                    else
                    {
                        DateTime date = new DateTime(this.dateAdded, DateTimeKind.Utc).ToLocalTime();
                        return date.ToString();
                    }
                }
            }

            public string Username
            {
                get
                {
                    //find the guid of the user
                    Guid id = this.userCreated.ID;
                    User user = ServerConnection.credentials.MatchUser(id);
                    if (user != null)
                    {
                        return user.displayName;
                    }
                    else
                    {
                        return "Deleted user";
                    }
                }
            }

            public bool Equals(ProjectNote other)
            {
                if (ReferenceEquals(other, null)) return false;
                if (ReferenceEquals(other, this)) return true;
                return (this.dateAdded == other.dateAdded) && (this.userCreated == other.userCreated);
            }

            public override int GetHashCode()
            {
                return this.noteText.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ProjectNote);
            }

            public static bool operator ==(ProjectNote A, ProjectNote B)
            {
                return Equals(A, B);
            }

            public static bool operator !=(ProjectNote A, ProjectNote B)
            {
                return !Equals(A, B);
            }

        }

        public static Versioning.SaveStructureV1 LoadDatabaseFile(Form1.ServerConnection.ProjectFolder projectFolder)
        {
            Versioning.SaveStructureV1 data = new Versioning.SaveStructureV1();

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //check to see if database exists
            //string dbFilePath = Path.Combine(projectFolder.fullPath, databaseFilename);
            string dbFilePath = Path.Combine(Form1.serverFolder, projectFolder.projectPath + ".db");
            if (File.Exists(dbFilePath))
            {
                try
                {
                    // Decryption
                    Debug.WriteLine("Trying to load database file.");
                    using (var fs = new FileStream(dbFilePath, FileMode.Open, FileAccess.Read))
                    using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(Form1.desKey, Form1.desIV), CryptoStreamMode.Read))
                    {
                        // This is where you deserialize the class
                        data = ProtoBuf.Serializer.Deserialize<Versioning.SaveStructureV1>(cryptoStream);
                        Debug.WriteLine("Successfully loaded database file.");
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(this, e.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Debug.WriteLine("Error while loading database file..", e);
                    throw;
                }

            }
            else
            {
                //check to create file maybe..
                Debug.WriteLine("LoadDatabaseFile: File doesn't exist!");
            }

            Debug.WriteLine($"Returning loaded database file with {data.noteData.Count} notes.");
            return data;
        }

        [ProtoContract]
        public class Attachment
        {
            [ProtoMember(1)]
            public string OriginalFilePath { get; set; }
            [ProtoMember(2)]
            public string FileName{ get; set; }
            [ProtoMember(3)]
            public string AttachmentFilePath { get; set;}
            [ProtoMember(4)]
            public bool SavedToDisk { get; set; }
            [ProtoMember(5)]
            public Guid Id { get; set; }

            public Icon Icon { get; set; }

            public Attachment()
            {
                //parameterless constructor for protobuf
            }

            public Attachment(string filepath)
            {
                this.OriginalFilePath = filepath;
                this.FileName = Path.GetFileName(filepath);
                this.SavedToDisk = false;
                this.Id = Guid.NewGuid();
            }

            public void SaveToDisk()
            {
                if (SavedToDisk)
                {
                    return;
                }

                string attachmentsFolder = GetCurrentProjectAttachmentsFolder();
                if (!Directory.Exists(attachmentsFolder))
                {
                    Directory.CreateDirectory(attachmentsFolder);
                }

                string trialFileName = Path.Combine(attachmentsFolder, this.FileName);
                string validFilename = Attachment.GetValidFilename(trialFileName);

                File.Copy(this.OriginalFilePath, validFilename);
                this.AttachmentFilePath = validFilename;
                SavedToDisk = true;
            }

            public void DeleteFromDisk()
            {
                if (SavedToDisk)
                {
                    try
                    {
                        File.Delete(AttachmentFilePath);
                    }
                    catch
                    {

                    }
                }
            }

            public bool OpenFile()
            {
                try
                {
                    if (this.AttachmentFilePath != null)
                    {
                        if (File.Exists(this.AttachmentFilePath))
                        {
                            System.Diagnostics.Process.Start(this.AttachmentFilePath);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(this.OriginalFilePath);
                    }
                }
                catch
                {
                    throw;
                }

                return true;
            }

            private static string GetValidFilename(string fileName)
            {
                if (!File.Exists(fileName)) return fileName;

                FileInfo fi = new FileInfo(fileName);
                string ext = fi.Extension;
                string name = fi.FullName.Substring(0, fi.FullName.Length - ext.Length);

                int i = 2;
                while (File.Exists($"{name}_{i}{ext}"))
                {
                    i++;
                }


                return $"{name}_{i}{ext}";
            }
        }

        [ProtoContract]
        public class AttachmentLibrary
        {
            [ProtoMember(1)]
            public List<Attachment> attachments = new List<Attachment>();

            public AttachmentLibrary()
            {
                //parameterless constructor for protobuf
            }

            private List<Icon> MakeIconLibrary()
            {
                List<Icon> icons = new List<Icon>();
                foreach (var attachment in attachments)
                {
                    icons.Add(attachment.Icon);
                }

                return icons;
            }

            public void SaveIconLibrary()
            {
                List<Icon> icons = MakeIconLibrary();

                string attachmentsFolder = GetCurrentProjectAttachmentsFolder();
                if (!Directory.Exists(attachmentsFolder))
                {
                    Directory.CreateDirectory(attachmentsFolder);
                }

                string filePath = Path.Combine(attachmentsFolder, "iconcache");

                IconCache ic = new IconCache(icons);
                ic.SaveIconCache(filePath);
                return;

                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        formatter.Serialize(fs, icons);
                    }
                }
                catch
                {
                    throw;
                }
            }

            public List<Icon> LoadIconLibrary()
            {
                List<Icon> icons = new List<Icon>();

                string attachmentsFolder = GetCurrentProjectAttachmentsFolder();
                if (!Directory.Exists(attachmentsFolder))
                {
                    Directory.CreateDirectory(attachmentsFolder);
                }

                string filePath = Path.Combine(attachmentsFolder, "iconcache");

                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        icons = (List<Icon>)formatter.Deserialize(fs);
                    }
                }
                catch (FileNotFoundException e)
                {
                    return null;
                }

                return icons;
            }

            public void SetAttachmentIcons()
            {
                List<Icon> icons = LoadIconLibrary();

                if (icons == null)
                {
                    return;
                }

                for (int i = 0; i < attachments.Count; i++)
                {
                    attachments[i].Icon = icons[i];
                }
            }

            //FindAttachment:
            //traverses the list of attachments and returns the matching attachment,
            //otherwise, returns null
            private Attachment FindAttachment(Attachment attachment)
            {
                foreach (var _attachment in attachments)
                {
                    if (_attachment.Id == attachment.Id)
                    {
                        return _attachment;
                    }
                }

                return null;
            }

            public Attachment AddAttachment(Attachment attachment)
            {
                if (FindAttachment(attachment) == null)
                {
                    attachments.Add(attachment);
                    return attachment;
                }

                return null;
            }

            public Attachment RemoveAttachment(Attachment attachment, bool deleteFromDisk = false)
            {
                Attachment match = FindAttachment(attachment);
                if (match != null)
                {
                    attachments.Remove(match);

                    if (deleteFromDisk)
                    {
                        match.DeleteFromDisk();
                    }

                    return match;
                }

                return null;
            }

            public List<Attachment> GetAttachments()
            {
                return attachments;
            }

            public void SetAttachments(List<Attachment> attachments)
            {
                this.attachments = attachments;
            }

            public void SaveAttachmentsToDisk()
            {
                foreach (var attachment in attachments)
                {
                    attachment.SaveToDisk();
                }

                SaveIconLibrary();
            }
        }
    }
}
*/