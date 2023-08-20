using ProtoBuf;
using System;
using System.Collections.Generic;

namespace RadinProjectNotes
{
    public partial class Notes
    {
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
                string pcName = Environment.UserName;

                this.noteText = text;
                this.userCreated = Credentials.Instance.currentUser;
                this.dateAdded = DateTime.UtcNow.Ticks;
                this.pcName = pcName;
                this.dateLastEdited = -1;
                this.noteStatus = NoteStatus.Pending;
            }

            public ProjectNote(string text, List<Attachment> attachments) : this(text)
            {
                this.attachmentLibrary.SetAttachments(attachments);
            }

            public object Clone()
            {
                ProjectNote newNote = new ProjectNote
                {
                    noteText = this.noteText,
                    userCreated = this.userCreated,
                    dateAdded = this.dateAdded,
                    pcName = this.pcName,
                    dateLastEdited = this.dateLastEdited,
                    noteStatus = this.noteStatus,
                    userFullfilled = this.userFullfilled
                };
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

            public string CreatedByUsername
            {
                get
                {
                    //find the guid of the user
                    return Credentials.Instance.GetUserDisplayNameById(this.userCreated.ID);
                }
            }

            public bool IsWithinAllowedIntervalToEdit()
            {
                return AllowedIntervalHasNotElapsed(maxEditHours);
            }

            public bool IsWithinAllowedIntervalToDelete()
            {
                return AllowedIntervalHasNotElapsed(maxDeleteHours);
            }

            private bool AllowedIntervalHasNotElapsed(int allowedInterval)
            {
                TimeSpan difference = DateTime.Now.Subtract(new DateTime(dateAdded));
                return difference.TotalHours <= allowedInterval;
            }

            public bool Equals(ProjectNote other)
            {
                if (other is null) return false;
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
    }
}
