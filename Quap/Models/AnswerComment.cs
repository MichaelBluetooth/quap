using System;
using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public class AnswerComment : BaseModel
    {
        [Required]
        public Guid? answerId { get; set; }
        public Answer answer { get; set; }

        [Required]
        [MaxLength(512)]
        public string comment { get; set; }

        [Required]
        public Guid? commenterId { get; set; }
        public User commenter { get; set; }
    }
}