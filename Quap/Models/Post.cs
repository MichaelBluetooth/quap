using System;
using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public abstract class Post : BaseModel
    {
        [Required]
        [MaxLength(30000)]
        public string body { get; set; }

        [Required]
        public Guid? createdById { get; set; }
        public User createdBy { get; set; }
    }
}