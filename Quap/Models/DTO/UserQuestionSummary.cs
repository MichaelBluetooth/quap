using System;

namespace Quap.Models.DTO
{
    public class UserQuestionSummary
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public DateTime created { get; set; }
    }
}