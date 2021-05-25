using System;

namespace Quap.Services.QandA
{
    public class VoteRequest
    {
        public string voteType { get; set; }
        public Guid postId { get; set; }
    }
}