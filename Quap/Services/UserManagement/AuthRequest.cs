using System.ComponentModel.DataAnnotations;

namespace Quap.Services.UserManagement
{
    public class AuthRequest
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}