using System;
using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public class QuestionVote: BaseModel
    {
        [Required]
        public Guid? voterId { get; set; }
        public User voter { get; set; }

        [Required]
        public Guid? questionId { get; set; }
        public Question question { get; set; }

        [Required]
        [MaxLength(50)]
        public string voteType { get; set; }
    }
}