using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Quap.Models
{
    public class Question : Post
    {
        [Required]
        [MaxLength(150)]
        public string title { get; set; }

        public ICollection<QuestionTag> tags { get; set; } = new Collection<QuestionTag>();
        public ICollection<QuestionComment> comments { get; set; } = new Collection<QuestionComment>();
        public ICollection<Answer> answers { get; set; } = new Collection<Answer>();
        public ICollection<QuestionVote> votes { get; set; } = new Collection<QuestionVote>();
    }
}