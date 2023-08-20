using System;
using System.Runtime.Serialization;

namespace ProjectEmailNotificationService
{
    /// <summary>
    /// Represents a due item that has already been processed.
    /// </summary>
    [Serializable]
    internal class ProcessedDueItem : IEquatable<ProcessedDueItem>
    {
        private string _projectCode;
        private Guid _dueItemId;

        public ProcessedDueItem(string projectCode, Guid dueItemId)
        {
            _projectCode = projectCode;
            _dueItemId = dueItemId;
        }

        [IgnoreDataMember]
        public string ProjectCode { get { return _projectCode; } }
        [IgnoreDataMember]
        public Guid DueItemId { get { return _dueItemId; } }

        public bool Equals(ProcessedDueItem other)
        {
            return this.ProjectCode.Equals(other.ProjectCode) && this.DueItemId.Equals(other.DueItemId);
        }
    }
}
