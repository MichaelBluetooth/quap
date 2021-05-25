using System;
using System.ComponentModel.DataAnnotations;

namespace Quap.Services.QandA
{
    public class AnswerAcceptRequest
    {
        [Required]
        public Guid answerId { get; set; }
    }
}