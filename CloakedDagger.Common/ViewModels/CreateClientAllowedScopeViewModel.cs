using System;
using System.ComponentModel.DataAnnotations;

namespace CloakedDagger.Common.ViewModels
{
    public class CreateClientAllowedScopeViewModel
    {
        public Guid ClientId { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [MaxLength(250, ErrorMessage = "Name must not exceed 250 characters")]
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}