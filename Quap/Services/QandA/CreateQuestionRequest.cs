using System.Collections.Generic;

namespace Quap.Services.QandA
{
    public class CreateOrUpdateQuestionRequest
    {
        public string title { get; set; }
        public string body { get; set; }
        public ICollection<string> tags { get; set; }
    }
}