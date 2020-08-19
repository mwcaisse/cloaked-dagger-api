using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloakedDagger.Common.ViewModels
{
    public class ResourceViewModel
    {
        public Guid? ResourceId { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [MaxLength(250, ErrorMessage = "Name must not exceed 250 characters")]
        public string Name { get; set; }
        
        [MaxLength(1000, ErrorMessage = "Description must not exceed 1000 characters")]
        public string Description { get; set; }

        public IEnumerable<ResourceScopeViewModel> AvailableScopes { get; set; }
    }
}