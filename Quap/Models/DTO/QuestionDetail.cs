using System;
using System.Collections.Generic;

namespace Quap.Models.DTO
{
    public class QuestionDetail
    {
        public Guid id { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }
        public string createdByUsername { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int votesCount { get; set; }
        public ICollection<string> tags { get; set; }
        public ICollection<AnswerDetail> answers { get; set; }
        public ICollection<QuestionComment> comments { get; set; }
        public string userVoteType { get; set; }
    }
}