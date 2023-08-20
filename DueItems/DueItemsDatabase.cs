using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DueItems
{
    /// <summary>
    /// Global database of due items.
    /// </summary>
    [ProtoContract]
    public class DueItemsDatabase
    {
        public static DueItemsDatabase CreateEmpty()
        {
            return new DueItemsDatabase();
        }

        [ProtoMember(1)]
        private List<DueItem> _dueItems = new List<DueItem>();

        private DueItemsDatabase() { }

        [ProtoIgnore]
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

        /// <summary>
        /// Edit the state of this due item.
        /// </summary>
        /// <param name="userId">User who committed the new state.</param>
        /// <param name="item">Due item.</param>
        /// <param name="newDueItemState">New state for this due item.</param>
        public void Edit(Guid userId, DueItem item, DueItemState newDueItemState)
        {
            Edit(userId, item.Id, newDueItemState);
        }

        /// <summary>
        /// Edit the state of this due item.
        /// </summary>
        /// <param name="userId">User who committed the new state.</param>
        /// <param name="itemId">Due item id.</param>
        /// <param name="newDueItemState">New state for this due item.</param>
        public void Edit(Guid userId, Guid itemId, DueItemState newDueItemState)
        {
            var hit = FindById(itemId);
            if (hit != null)
            {
                hit.Edit(userId, newDueItemState);
            }
        }

        public DueItem FindById(Guid id)
        {
            return _dueItems.FirstOrDefault(a => a.Id == id);
        }
    }
}
