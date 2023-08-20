using ProtoBuf;
using System;

namespace DueItems
{
    [ProtoContract]
    public enum DueItemHistoryActionType
    {
        Created,
        Edited
    }

    [ProtoContract]
    public class DueItemHistoryAction
    {
        [ProtoMember(1)]
        private Guid _guidCommittedByUser;
        [ProtoMember(2)]
        private DueItemHistoryActionType _actionType;
        [ProtoMember(3)]
        private DueItemState _dueItemStateAfterAction;
        [ProtoMember(4)]
        private long _actionTime;

        public DueItemHistoryAction()
        {
            // Parameterless constructor for protobuf.
        }

        /// <summary>
        /// Create a new history action for a due item.
        /// </summary>
        /// <param name="guidCommittedByUser">Guid of the user who committed this action.</param>
        /// <param name="actionType">Type of action committed.</param>
        /// <param name="dueItemAfterAction">Due item's state after the action.</param>
        public DueItemHistoryAction(Guid guidCommittedByUser, DueItemHistoryActionType actionType, DueItemState dueItemStateAfterAction)
        {
            _guidCommittedByUser = guidCommittedByUser;
            _actionType = actionType;
            _dueItemStateAfterAction = dueItemStateAfterAction;
            _actionTime = DateTime.UtcNow.Ticks;
        }

        /// <summary>The id of the user who committed this action.</summary>
        [ProtoIgnore]
        public Guid GuidCommittedByUser { get { return _guidCommittedByUser; } }
        /// <summary>Type of the action committed.</summary>
        [ProtoIgnore]
        public DueItemHistoryActionType ActionType { get {  return _actionType; } }
        /// <summary>Due item's state after the action.</summary>
        [ProtoIgnore]
        public DueItemState DueItemStateAfterAction { get { return _dueItemStateAfterAction; } }
        /// <summary>Action time in UTC.</summary>
        [ProtoIgnore]
        public DateTime ActionTime { get { return new DateTime(_actionTime, DateTimeKind.Utc); } }
    }
}
