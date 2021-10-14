using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Quap.Models
{
    public class Answer : Post
    {
        public bool accepted { get; set; }

        public Guid? questionId { get; set; }
        public Question question { get; set; }

        public ICollection<AnswerComment> comments { get; set; } = new Collection<AnswerComment>();
        public ICollection<AnswerVote> votes { get; set; } = new Collection<AnswerVote>();

        public Guid getOwnerId()
        {
            return createdById.Value;
        }
    }
}