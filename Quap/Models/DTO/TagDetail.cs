using System;

namespace Quap.Models.DTO
{
    public class TagDetail
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int totalQuestions { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }
    }
}