using System;

namespace Quap.Models.DTO
{
    public class UserAnswerSummary
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public DateTime created { get; set; }
        public Guid questionId { get; set; }
    }
}