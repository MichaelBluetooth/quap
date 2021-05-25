using System;
using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public abstract class BaseModel
    {
        [Key]
        public Guid id { get; set; }

        [Required]
        public DateTime created { get; set; }

        [Required]
        public DateTime lastModified { get; set; }
    }
}