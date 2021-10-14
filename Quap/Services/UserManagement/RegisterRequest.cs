using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Quap.Models;

namespace Quap.Services.UserManagement
{
    public class RegisterRequest : IValidatableObject
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

        [Required]
        public string role { get; set; } = User.Roles.USER;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!User.Roles.ALL.Contains(this.role))
            {
                yield return new ValidationResult(
                    "The 'role' field must match one of the following: " + string.Join(',', User.Roles.ALL),
                    new[] { nameof(role) });
            }
        }
    }
}