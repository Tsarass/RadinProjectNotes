using System;
using System.Collections.Generic;
using System.Linq;

namespace RadinProjectNotes.ProjectServices
{
    /// <summary>
    /// A service category with its subitems.
    /// </summary>
    [Serializable]
    public class ServiceCategory
    {
        private string _title;
        private List<string> _services;

        public ServiceCategory(string title, List<string> services)
        {
            _title = title;
            _services = services;
        }

        public string Title { get { return _title; } }
        public IReadOnlyList<string> Services { get { return _services.AsReadOnly(); } }

        /// <summary>
        /// Add a service to this category if it doesn't exist.
        /// </summary>
        /// <param name="service"></param>
        public void addService(string service)
        {
            var existingService = _services.FirstOrDefault(a => a.Equals(service));
            if (existingService is null)
            {
                _services.Add(service);
            }
        }

        /// <summary>
        /// Remove a service from this category if it exists.
        /// </summary>
        /// <param name="service"></param>
        public void removeService(string service) 
        {
            _services.Remove(service);
        }
    }
}
