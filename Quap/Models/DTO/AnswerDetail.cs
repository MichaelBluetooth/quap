using System;
using System.Collections.Generic;

namespace Quap.Models.DTO
{
    public class AnswerDetail
    {
        public Guid id { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }
        public string createdByUsername { get; set; }
        public string body { get; set; }
        public int votesCount { get; set; }
        public string userVoteType { get; set; }
        public bool accepted { get; set; }
        public ICollection<AnswerComment> comments { get; set; }
    }
}