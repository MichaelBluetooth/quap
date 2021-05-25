using System.ComponentModel.DataAnnotations;

namespace Quap.Services.UserManagement
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(10)]
        public string username { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
        
        [Required]
        [MinLength(8)]
        public string password { get; set; }
    }
}