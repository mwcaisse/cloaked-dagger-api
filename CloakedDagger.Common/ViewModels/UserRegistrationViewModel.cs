using System.ComponentModel.DataAnnotations;

namespace CloakedDagger.Common.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(500)]
        public string Name { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string Username { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MinLength(8)]
        public string Password { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(250)]
        public string Email { get; set; }
    }
}