using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace Quap.Models
{
    public class User : BaseModel, IValidatableObject
    {
        [Required]
        [MaxLength(256)]
        [MinLength(8)]
        public string username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string email { get; set; }

        [Required]
        [MaxLength(256)]
        [MinLength(8)]
        [JsonIgnore]
        public string password { get; set; }

        public ICollection<Question> questions { get; set; }
        public ICollection<Answer> answers { get; set; }

        [Required]
        [MaxLength(256)]
        public string role { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!User.Roles.ALL.Contains(this.role))
            {
                yield return new ValidationResult(
                    "The 'role' field must match one of the following: " + string.Join(',', User.Roles.ALL),
                    new[] { nameof(role) });
            }
        }

        public sealed class Roles
        {
            public static readonly string ADMIN = "Admin";
            public static readonly string MODERATOR = "Moderator";
            public static readonly string USER = "User";

            public static readonly IReadOnlyCollection<string> ALL = new Collection<string>(){
                ADMIN,
                MODERATOR,
                USER
            };
        }
    }
}