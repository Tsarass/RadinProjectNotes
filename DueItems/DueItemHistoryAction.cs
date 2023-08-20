using System;

namespace DueItems
{
    public enum DueItemHistoryActionType
    {
        Created,
        Edited
    }

    internal abstract class DueItemHistoryAction
    {
        private Guid _guidDoneByUser;

        public DueItemHistoryAction(Guid guidDoneByUser)
        {
            _guidDoneByUser = guidDoneByUser;
        }

        /// <summary>The id of the user who did this action.</summary>
        public Guid GuidDoneByUser { get { return _guidDoneByUser; } }
    }
}
