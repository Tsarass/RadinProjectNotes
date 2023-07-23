using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RadinProjectNotes.DueItems
{
    [Serializable]
    public enum DueStatus
    {
        Pending = 0,    // An item that is pending.
        Complete = 1,   // An item that is complete.
        Transferred = 2 // An item whose due date was transferred to a later date (will never complete)
    }

    /// <summary>
    /// Represents an item with a description that is due on a specific date.
    /// </summary>
    [Serializable]
    public class DueItem
    {
        private Guid _id;
        private string _projectCode;
        private string _description;
        private long _dateIssued;
        private long _dateDue;
        private User _createdBy;
        private DueStatus _dueStatus;
        private List<string> _emailsToBeNotified;

        /// <summary>
        /// Create a new due item for a project.
        /// </summary>
        /// <param name="projectCode">The code for the project.</param>
        /// <param name="description">Description of the due item.</param>
        /// <param name="dueDate">Date the item is due.</param>
        /// <param name="createdBy">User who created this item.</param>
        /// <param name="emailsToBeNotified">List of emails to be notified if this item's due date is reached while still pending.</param>
        public DueItem(string projectCode, string description, DateTime dueDate, User createdBy, List<string> emailsToBeNotified)
        {
            _projectCode = projectCode;
            _description = description;
            _dateDue = dueDate.Ticks;
            _createdBy = createdBy;
            _emailsToBeNotified = emailsToBeNotified;

            _dateIssued = DateTime.UtcNow.Ticks;
            _dueStatus = DueStatus.Pending;
            _id = Guid.NewGuid();
        }

        [IgnoreDataMember]
        public Guid Id { get { return _id; } }
        [IgnoreDataMember]
        public string ProjectCode { get { return _projectCode; } }
        [IgnoreDataMember]
        public string Description { get { return _description; } }
        [IgnoreDataMember]
        public DateTime DueDate { get { return new DateTime(_dateDue, DateTimeKind.Utc); } }
        [IgnoreDataMember]
        public DateTime IssuedDate { get { return new DateTime(_dateIssued, DateTimeKind.Utc); } }
        [IgnoreDataMember]
        public User CreatedBy { get { return _createdBy; } }
        [IgnoreDataMember]
        public List<string> EmailsToBeNotified { get {  return _emailsToBeNotified; } }
        [IgnoreDataMember]
        public DueStatus DueStatus { get { return _dueStatus; } }
    }
}
