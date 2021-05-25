using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public class Question : Post
    {
        [Required]
        [MaxLength(150)]
        public string title { get; set; }

        public ICollection<QuestionTag> tags { get; set; }
        public ICollection<QuestionComment> comments { get; set; }
        public ICollection<Answer> answers { get; set; }
        public ICollection<QuestionVote> votes { get; set; }
    }
}