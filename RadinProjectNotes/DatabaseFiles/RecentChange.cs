using ProtoBuf;
using System;

namespace RadinProjectNotes.DatabaseFiles
{
    [ProtoContract]
    public class RecentChange
    {
        [ProtoMember(1)]
        public string projectTitle;
        [ProtoMember(2)]
        public string username;
        [ProtoMember(3)]
        public long dateTimeTicks;

        public RecentChange()
        {

        }

        public RecentChange(string projectTitle, string username, DateTime date)
        {
            this.projectTitle = projectTitle;
            this.username = username;
            this.dateTimeTicks = date.Ticks;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return this.projectTitle.GetHashCode();
            }
        }

        public bool Equals(RecentChange other)
        {
            if (other is null) return false;
            if (ReferenceEquals(other, this)) return true;
            return this.projectTitle == other.projectTitle;
        }
    }
}
