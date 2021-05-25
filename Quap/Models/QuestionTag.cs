using System;
using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public class QuestionTag : BaseModel
    {
        [Required]
        public Guid questionId { get; set; }
        public Question question { get; set; }

        [Required]
        public Guid tagId { get; set; }
        public Tag tag { get; set; }
    }
}