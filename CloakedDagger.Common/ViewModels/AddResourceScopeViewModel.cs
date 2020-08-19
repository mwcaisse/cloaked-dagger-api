using System;
using System.ComponentModel.DataAnnotations;

namespace CloakedDagger.Common.ViewModels
{
    public class AddResourceScopeViewModel
    {
        public Guid ResourceId { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "ScopeName is required")]
        [MaxLength(250, ErrorMessage = "ScopeName must not exceed 250 characters")]
        public string ScopeName { get; set; }
        
        public string Description { get; set; }
    }
}