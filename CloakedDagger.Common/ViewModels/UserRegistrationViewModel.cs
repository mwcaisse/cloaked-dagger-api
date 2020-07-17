using System.ComponentModel.DataAnnotations;

namespace CloakedDagger.Common.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [MaxLength(500, ErrorMessage = "Name must not be more than 500 characters")]
        public string Name { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        [MaxLength(100, ErrorMessage = "Name must not be more than 100 characters")]
        public string Username { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [MaxLength(250, ErrorMessage = "Email must not be more than 250 characters")]
        public string Email { get; set; }
    }
}