using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
        private string _description;
        private long _dateIssued;
        private long _dateDue;
        private Guid _guidCreatedByUser;
        private DueStatus _dueStatus;
        private List<string> _emailsToBeNotified;

        /// <summary>
        /// Create a new due item for a project.
        /// </summary>
        /// <param name="description">Description of the due item.</param>
        /// <param name="dueDate">Date the item is due.</param>
        /// <param name="createdBy">User who created this item.</param>
        /// <param name="emailsToBeNotified">List of emails to be notified if this item's due date is reached while still pending.</param>
        public DueItem(string description, DateTime dueDate, Guid guidCreatedByUser, List<string> emailsToBeNotified)
        {
            _description = description;
            _dateDue = dueDate.Ticks;
            _guidCreatedByUser = guidCreatedByUser;
            _emailsToBeNotified = emailsToBeNotified;

            _dateIssued = DateTime.UtcNow.Ticks;
            _dueStatus = DueStatus.Pending;
            _id = Guid.NewGuid();
        }

        [IgnoreDataMember]
        public Guid Id { get { return _id; } }
        [IgnoreDataMember]
        public string Description { get { return _description; } set { _description = value; } }
        /// <summary>
        /// Due date in UTC.
        /// </summary>
        [IgnoreDataMember]
        public DateTime DueDate { get { return new DateTime(_dateDue, DateTimeKind.Utc); } }
        /// <summary>
        /// Issued date in UTC.
        /// </summary>
        [IgnoreDataMember]
        public DateTime IssuedDate { get { return new DateTime(_dateIssued, DateTimeKind.Utc); } }
        [IgnoreDataMember]
        public Guid CreatedByUserId { get { return _guidCreatedByUser; } }
        [IgnoreDataMember]
        public List<string> EmailsToBeNotified { get {  return _emailsToBeNotified; } }
        [IgnoreDataMember]
        public DueStatus DueStatus { get { return _dueStatus; } }

        public void Edit(string description, DateTime dueDate,
            List<string> emailsToBeNotified, DueStatus dueStatus)
        {
            _description = description;
            _dueStatus = dueStatus;
            _emailsToBeNotified = emailsToBeNotified;
            _dateDue = dueDate.Ticks;
        }

        /// <summary>
        /// Check if the due item has expired (it is still pending and the due date has passed).
        /// </summary>
        /// <returns></returns>
        public bool HasExpired()
        {
            return _dueStatus == DueStatus.Pending && DateTime.Compare(DateTime.UtcNow, DueDate) > 0;
        }
    }
}
