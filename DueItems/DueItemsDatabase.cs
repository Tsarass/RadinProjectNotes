using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RadinProjectNotes.DueItems
{
    /// <summary>
    /// Global database of due items.
    /// </summary>
    [Serializable]
    public class DueItemsDatabase
    {
        public static DueItemsDatabase CreateEmpty()
        {
            return new DueItemsDatabase();
        }

        private List<DueItem> _dueItems = new List<DueItem>();

        private DueItemsDatabase() { }
        
        [IgnoreDataMember]
        public List<DueItem> DueItems { get { return _dueItems; } }

        public void Add(DueItem item)
        {
            if (_dueItems.FirstOrDefault(a =>  a.Id == item.Id) is null)
            {
                _dueItems.Add(item);
            }
        }

        public void Remove(DueItem item)
        {
            Remove(item.Id);
        }

        public void Remove(Guid itemId)
        {
            var hit = FindById(itemId);
            if (hit != null)
            {
                _dueItems.Remove(hit);
            }
        }

        public void Edit(DueItem item, string description, DateTime dueDate,
            List<string> emailsToBeNotified, DueStatus dueStatus)
        {
            Edit(item.Id, description, dueDate, emailsToBeNotified, dueStatus);
        }

        public void Edit(Guid itemId, string description, DateTime dueDate,
            List<string> emailsToBeNotified, DueStatus dueStatus)
        {
            var hit = FindById(itemId);
            if (hit != null)
            {
                hit.Edit(description, dueDate, emailsToBeNotified, dueStatus);
            }
        }

        public DueItem FindById(Guid id)
        {
            return _dueItems.FirstOrDefault(a => a.Id == id);
        }
    }
}
