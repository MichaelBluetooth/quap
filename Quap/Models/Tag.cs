using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public class Tag : BaseModel
    {
        [Required]
        [MaxLength(150)]
        public string name { get; set; }

        [MaxLength(256)]
        public string description { get; set; }
    }
}