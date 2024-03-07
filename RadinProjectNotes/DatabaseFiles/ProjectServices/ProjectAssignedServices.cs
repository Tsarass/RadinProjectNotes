using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RadinProjectNotes.DatabaseFiles.ProjectServices
{
    /// <summary>
    /// Services assigned to a project.
    /// </summary>
    [Serializable]
    [ProtoContract]
    public class ProjectAssignedServices
    {
        public static ProjectAssignedServices CreateEmpty()
        {
            return new ProjectAssignedServices();
        }

        /// <summary>Version string to match the assigned services to a service category list from the configuration.</summary>
        [IgnoreDataMember]
        [ProtoMember(2)]
        private string _versionString;

        [IgnoreDataMember]
        [ProtoMember(1)]
        private List<AssignedService> _assignedServices = new List<AssignedService>();

        public ProjectAssignedServices()
        { }

        [IgnoreDataMember]
        public bool IsEmpty { get { return _assignedServices.Count == 0; } }

        public List<AssignedService> AssignedServices { get { return _assignedServices; } }

        [IgnoreDataMember]
        public string versionString { get { return _versionString; } set { _versionString = value; } }

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
        [ProtoContract]
        public class AssignedService
        {
            public AssignedService()
            {
                // parameterless constructor for protobuf
            }

            public AssignedService(string categoryTitle, string description)
            {
                CategoryTitle = categoryTitle;
                ServiceDescription = description;
            }

            [ProtoMember(1)]
            public string CategoryTitle { get; set; }
            [ProtoMember(2)]
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
