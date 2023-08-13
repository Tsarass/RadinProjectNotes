using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
            if (_dueItems.FirstOrDefault(a => a.Id == item.Id) != null)
            {
                _dueItems.Remove(item);
            }
        }
    }
}
