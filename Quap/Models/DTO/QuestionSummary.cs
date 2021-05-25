using System;
using System.Collections.Generic;

namespace Quap.Models.DTO
{
    public class QuestionSummary
    {
        public Guid id { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }
        public string createdByUsername { get; set; }
        public string title { get; set; }
        public int answersCount { get; set; }
        public bool hasAcceptedAnswer { get; set; }
        public int votesCount { get; set; }
        public ICollection<string> tags { get; set; }
        public string userVoteType { get; set; }
    }
}