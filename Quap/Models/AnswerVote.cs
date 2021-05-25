using System;
using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public class AnswerVote: BaseModel
    {
        [Required]
        public Guid? voterId { get; set; }
        public User voter { get; set; }

        [Required]
        public Guid? answerId { get; set; }
        public Answer answer { get; set; }

        [Required]
        [MaxLength(50)]
        public string voteType { get; set; }
    }
}