using System;
using System.ComponentModel.DataAnnotations;

namespace Quap.Services.QandA
{
    public class CommentRequest
    {
        [Required]
        public string comment { get; set; }

        public Guid? postId { get; set; }
    }
}