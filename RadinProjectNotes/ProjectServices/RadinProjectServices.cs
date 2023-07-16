using System;
using System.Collections.Generic;
using System.Linq;

namespace RadinProjectNotes.ProjectServices
{
    /// <summary>
    /// Services provided by the company as a list of categories with their subitems.
    /// </summary>
    [Serializable]
    public class RadinProjectServices
    {
        /// <summary>
        /// Create an empty instance of service categories.
        /// </summary>
        /// <returns></returns>
        public static RadinProjectServices CreateEmpty()
        {
            return new RadinProjectServices(new List<ServiceCategory>());
        }

        /// <summary>
        /// Available project services.
        /// </summary>
        private List<ServiceCategory> _serviceCategories;

        public RadinProjectServices(List<ServiceCategory> projectServiceCategories)
        {
            this._serviceCategories = projectServiceCategories;
        }

        /// <summary>
        /// Get the number of categories.
        /// </summary>
        /// <returns></returns>
        public int getCategoriesCount()
        {
            return _serviceCategories.Count;
        }

        /// <summary>
        /// Get the number of services under the supplied category.
        /// </summary>
        /// <returns></returns>
        public int getServicesCount(string categoryTitle)
        {
            var existingCategory = _serviceCategories.FirstOrDefault(c => c.Title == categoryTitle);
            if (existingCategory != null)
            {
                return existingCategory.Services.Count;
            }

            return 0;
        }

        /// <summary>
        /// Get all category titles.
        /// </summary>
        /// <returns></returns>
        public List<string> getCategoryTitles()
        {
            return _serviceCategories.Select(a => a.Title).ToList();
        }

        /// <summary>
        /// Get all services for a category with the supplied title.
        /// </summary>
        /// <param name="categoryTitle"></param>
        /// <returns></returns>
        public IReadOnlyList<string> getServicesForCategory(string categoryTitle)
        {
            return _serviceCategories.FirstOrDefault(a => a.Title == categoryTitle).Services;
        }

        /// <summary>
        /// Add a service category if one with the same title does not already exist.
        /// </summary>
        /// <param name="category"></param>
        public void addServiceCategory(ServiceCategory category)
        {
            if (category != null)
            {
                var existingCategory = _serviceCategories.FirstOrDefault(a => a.Title.Equals(category.Title));
                if (existingCategory is null)
                {
                    _serviceCategories.Add(category);
                }
            }
        }

        /// <summary>
        /// Remove a service category if it exists.
        /// </summary>
        /// <param name="categoryTitle"></param>
        public void removeServiceCategory(string categoryTitle) 
        { 
            var existingCategory = _serviceCategories.FirstOrDefault(a => a.Title.Equals(categoryTitle));
            if (existingCategory != null)
            {
                _serviceCategories.Remove(existingCategory);
            }
        }

        /// <summary>
        /// Add a service to a category with the supplied title if it exists.
        /// </summary>
        /// <param name="categoryTitle"></param>
        public void addServiceToCategory(string categoryTitle, string service)
        {
            if (string.IsNullOrEmpty(categoryTitle)) return;
            
            var existingCategory = _serviceCategories.FirstOrDefault(a => a.Title.Equals(categoryTitle));
            if (existingCategory != null)
            {
                existingCategory.addService(service);
            }
        }

        /// <summary>
        /// Remove a service from a category with the supplied title if it exists.
        /// </summary>
        /// <param name="categoryTitle"></param>
        public void removeServiceFromCategory(string categoryTitle, string service)
        {
            if (string.IsNullOrEmpty(categoryTitle)) return;

            var existingCategory = _serviceCategories.FirstOrDefault(a => a.Title.Equals(categoryTitle));
            if (existingCategory != null)
            {
                existingCategory.removeService(service);
            }
        }
    }
}
