using System;
using System.Collections.Generic;
using Quap.Permissions;

namespace Quap.Models
{
    public class Answer : Post, IOwnerable
    {
        public bool accepted { get; set; }

        public Guid? questionId { get; set; }
        public Question question { get; set; }

        public ICollection<AnswerComment> comments { get; set; }
        public ICollection<AnswerVote> votes { get; set; }

        public Guid getOwnerId()
        {
            return createdById.Value;
        }
    }
}