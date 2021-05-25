using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quap.Models
{
    public class User : BaseModel
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
    }
}