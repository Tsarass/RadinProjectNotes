using ProtoBuf;
using System;
using System.Reflection;

namespace RadinProjectNotes
{
    [ProtoContract,Serializable]
    public class User
    {
        [ProtoMember(1)]
        private Guid id;
        [ProtoMember(2)]
        public string username;
        [ProtoMember(3)]
        private string password;
        [ProtoMember(4)]
        public Permissions permissions;
        [ProtoMember(5)]
        public string displayName;
        [ProtoMember(6)]
        public long lastLogin;  //in unix ticks
        [ProtoMember(7)]
        public string appVersion;

        public string Password
        {
            get { return password; }
        }

        public User()
        {
            //parameterless constructor for protobuf
        }

        //User constructor called only when new user is registered
        public User(string username, string password, Permissions permissions)
        {
            this.id = Guid.NewGuid();
            this.username = username;
            //hash the password
            string hashedPassword = HashPassword(password,this.id);
            this.password = hashedPassword;
            this.permissions = permissions;
            this.displayName = username;
            this.lastLogin = DateTime.UtcNow.Ticks;
            this.appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string HashPassword(string password, Guid id)
        {
            return Security.HashSHA1(password + id.ToString());
        }

        public bool Equals(User other)
        {
            if (other is null) return false;
            if (ReferenceEquals(other, this)) return true;
            return this.ID == other.ID;
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public static bool operator ==(User A, User B)
        {
            return Equals(A, B);
        }

        public static bool operator !=(User A, User B)
        {
            return !Equals(A, B);
        }

        public bool IsAdmin
        {
            get { return this.permissions.HasFlag(Permissions.Admin); }
        }

        public Guid ID
        {
            get { return this.id; }
        }

        public string LocalLastLoginDate
        {
            get
            { 
                var localTime = new DateTime(this.lastLogin, DateTimeKind.Utc).ToLocalTime();
                return localTime.ToString();
            }
        }
        /// <summary>
        /// Checks if the user can edit the passed note.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool CanEditNote(Notes.ProjectNote note)
        {
            if (!HasAuthorizationToEditOrDeleteNote(note) || !HasEditingPermission())
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Checks if the user can delete the passed note.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool CanDeleteNote(Notes.ProjectNote note)
        {
            if (!HasAuthorizationToEditOrDeleteNote(note) || !HasDeletingPermission())
            {
                return false;
            }

            return true;
        }

        public bool HasAuthorizationToEditOrDeleteNote(Notes.ProjectNote note)
        {
            if ((this == note.userCreated) || (this.IsAdmin))
            {
                return true;
            }
            return false;
        }

        public void ResetPassword()
        {
            this.password = Security.ResetPassword;
        }

        public void SetPassword(string newPassword)
        {
            this.password = newPassword;
        }

        public bool HasResetPassword()
        {
            return this.password == Security.ResetPassword;
        }

        public bool HasAddCommentPermission()
        {
            return permissions.HasFlag(Permissions.AddComment);
        }

        private bool HasEditingPermission()
        {
            return permissions.HasFlag(Permissions.Edit);
        }

        private bool HasDeletingPermission()
        {
            return permissions.HasFlag(Permissions.Delete);
        }

    }
}
