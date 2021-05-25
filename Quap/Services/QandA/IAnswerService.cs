using System;
using Quap.Models;

namespace Quap.Services.QandA
{
    public interface IAnswerService
    {
        Answer answerQuestion(CreateOrUpdateAnswerRequest req);
        Answer updateAnswer(Guid id, CreateOrUpdateAnswerRequest req);
        AnswerComment comment(CommentRequest req);
        AnswerVote vote(VoteRequest req);
        Answer accept(Guid id);
        Answer unaccept(Guid id);
    }
}