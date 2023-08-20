using DueItems;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace DueItems
{
    [ProtoContract]
    public enum DueStatus
    {
        Pending = 0,    // An item that is pending.
        Complete = 1,   // An item that is complete.
        Transferred = 2 // An item whose due date was transferred to a later date (will never complete)
    }

    /// <summary>
    /// Represents an item with a description that is due on a specific date.
    /// </summary>
    [ProtoContract]
    public class DueItem
    {
        [ProtoMember(1)]
        private Guid _id;
        [ProtoMember(2)]
        private string _description;
        [ProtoMember(3)]
        private long _dateIssued;
        [ProtoMember(4)]
        private long _dateDue;
        [ProtoMember(5)]
        private Guid _guidCreatedByUser;
        [ProtoMember(6)]
        private DueStatus _dueStatus;
        [ProtoMember(7)]
        private List<string> _emailsToBeNotified;
        [ProtoMember(8)]
        private List<DueItemHistoryAction> _historyActions;

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

            // Add a history action for the creation of this due item.
            _historyActions = new List<DueItemHistoryAction>();
            DueItemState stateAtCreation = new DueItemState(_description, dueDate, _dueStatus, _emailsToBeNotified);
            _historyActions.Add(new DueItemHistoryAction(guidCreatedByUser, DueItemHistoryActionType.Created, stateAtCreation));
        }

        [ProtoIgnore]
        public Guid Id { get { return _id; } }
        [ProtoIgnore]
        public string Description { get { return _description; } set { _description = value; } }
        /// <summary>
        /// Due date in UTC.
        /// </summary>
        [ProtoIgnore]
        public DateTime DueDate { get { return new DateTime(_dateDue, DateTimeKind.Utc); } }
        /// <summary>
        /// Issued date in UTC.
        /// </summary>
        [ProtoIgnore]
        public DateTime IssuedDate { get { return new DateTime(_dateIssued, DateTimeKind.Utc); } }
        [ProtoIgnore]
        public Guid CreatedByUserId { get { return _guidCreatedByUser; } }
        [ProtoIgnore]
        public List<string> EmailsToBeNotified { get {  return _emailsToBeNotified; } }
        [ProtoIgnore]
        public DueStatus DueStatus { get { return _dueStatus; } }
        [ProtoIgnore]
        public List<DueItemHistoryAction> HistoryActions { get { return _historyActions; } }

        public void Edit(Guid userId, DueItemState newDueItemState)
        {
            _description = newDueItemState.Description;
            _dueStatus = newDueItemState.DueStatus;
            _emailsToBeNotified = newDueItemState.EmailsToBeNotified;
            _dateDue = newDueItemState.DueDate.Ticks;

            // Add a history action for the editing of this due item.
            _historyActions.Add(new DueItemHistoryAction(userId, DueItemHistoryActionType.Edited, newDueItemState));
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
