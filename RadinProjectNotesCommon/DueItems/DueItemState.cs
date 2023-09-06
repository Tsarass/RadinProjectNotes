using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadinProjectNotesCommon.DueItems
{
    [ProtoContract]
    public class DueItemState
    {
        [ProtoMember(1)]
        private string _description;
        [ProtoMember(2)]
        private long _dueDate;
        [ProtoMember(3)]
        private DueStatus _dueStatus;
        [ProtoMember(4)]
        private List<string> _emailsToBeNotified;

        public DueItemState()
        {
            // Parameterless constructor for protobuf.
        }

        /// <summary>
        /// Create a new state for a due item.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <param name="dueStatus"></param>
        /// <param name="emailsToBeNotified"></param>
        public DueItemState(string description, DateTime dueDate,
            DueStatus dueStatus, List<string> emailsToBeNotified)
        {
            _description = description;
            _dueDate = dueDate.Ticks;
            _dueStatus = dueStatus;
            _emailsToBeNotified = emailsToBeNotified;
        }

        [ProtoIgnore]
        public string Description { get { return _description; } }
        [ProtoIgnore]
        public DateTime DueDate { get { return new DateTime(_dueDate, DateTimeKind.Utc); } }
        [ProtoIgnore]
        public DueStatus DueStatus { get {  return _dueStatus; } }
        [ProtoIgnore]
        public List<string> EmailsToBeNotified { get {  return _emailsToBeNotified; } }
    }
}
