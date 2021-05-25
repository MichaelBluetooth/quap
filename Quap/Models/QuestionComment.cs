using System;
using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public class QuestionComment : BaseModel
    {
        [Required]
        public Guid? questionId { get; set; }
        public Question question { get; set; }

        [Required]
        [MaxLength(512)]
        public string comment { get; set; }

        [Required]
        public Guid? commenterId { get; set; }
        public User commenter { get; set; }
    }
}