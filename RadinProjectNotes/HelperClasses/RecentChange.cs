using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadinProjectNotes.HelperClasses
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
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return this.projectTitle == other.projectTitle;
        }
    }
}
