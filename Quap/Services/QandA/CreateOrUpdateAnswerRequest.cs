using System;

namespace Quap.Services.QandA
{
    public class CreateOrUpdateAnswerRequest
    {
        public string body { get; set; }

        public Guid? questionId { get; set; }
    }
}