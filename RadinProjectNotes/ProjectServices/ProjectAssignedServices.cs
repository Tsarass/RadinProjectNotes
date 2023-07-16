using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RadinProjectNotes.ProjectServices
{


    /// <summary>
    /// Services assigned to a project.
    /// </summary>
    [Serializable]
    public class ProjectAssignedServices
    {
        public static ProjectAssignedServices CreateEmpty()
        {
            return new ProjectAssignedServices();
        }

        [IgnoreDataMember]
        private List<AssignedService> _assignedServices = new List<AssignedService>();

        public ProjectAssignedServices()
        { }

        public List<AssignedService> AssignedServices { get { return _assignedServices; } }

        public void AddAssignedService(AssignedService service)
        {
            if (service is null) return;
            if (_assignedServices.Contains(service)) return;

            _assignedServices.Add(service);
        }

        public void RemoveAssignedService(AssignedService service)
        {
            if (service is null) return;
            _assignedServices.Remove(service);
        }

        public bool ContainsService(string categoryTitle, string serviceDescription)
        {
            if (_assignedServices.Contains(new AssignedService(categoryTitle, serviceDescription)))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// A service assigned to a project with a category title and service description.
        /// </summary>
        [Serializable]
        
        public class AssignedService
        {
            public AssignedService(string categoryTitle, string description)
            {
                CategoryTitle = categoryTitle;
                ServiceDescription = description;
            }

            public string CategoryTitle { get; set; }
            public string ServiceDescription { get; set; }

            public override bool Equals(object obj)
            {
                AssignedService other = obj as AssignedService;
                if (other == null) return false;

                return CategoryTitle.Equals(other.CategoryTitle) && ServiceDescription.Equals(other.ServiceDescription);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }


}
