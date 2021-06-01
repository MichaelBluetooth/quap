using System;
using Quap.Models;
using Quap.Models.DTO;

namespace Quap.Services.QandA
{
    public interface IQuestionService
    {
        Question createQuestion(CreateOrUpdateQuestionRequest req);
        Question updateQuestion(Guid id, CreateOrUpdateQuestionRequest req);
        void deleteQuestion(Guid id);
        bool isQuestionOwner(Guid id);
        QuestionComment comment(CommentRequest req);
        QuestionVote vote(VoteRequest req);
        QuestionSearchResults search(int pageNumber = 1, int pageSize = 10, string text = "", string tag = "");
    }
}