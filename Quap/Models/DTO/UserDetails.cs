using System;
using System.Collections.Generic;

namespace Quap.Models.DTO
{
    public class UserDetails
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public DateTime created { get; set; }
        public ICollection<UserQuestionSummary> questions { get; set; }
        public ICollection<UserAnswerSummary> answers { get; set; }
    }
}