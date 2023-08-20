using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProjectEmailNotificationService
{
    /// <summary>
    /// Database for all due items that have been already processed.
    /// </summary>
    [Serializable]
    internal class ProcessedDueItemsDatabase
    {
        internal static ProcessedDueItemsDatabase CreateEmpty()
        {
            return new ProcessedDueItemsDatabase();
        }

        private List<ProcessedDueItem> _processedDueItems = new List<ProcessedDueItem>();

        private ProcessedDueItemsDatabase()
        { }

        [IgnoreDataMember]
        public List<ProcessedDueItem> ProcessedDueItems { get { return _processedDueItems; } }


        internal void Add(ProcessedDueItem item)
        {
            _processedDueItems.Add(item);
        }

        internal bool Contains(string projectCode, Guid dueItemId)
        {
            return _processedDueItems.Contains(new ProcessedDueItem(projectCode, dueItemId));
        }
    }
}
