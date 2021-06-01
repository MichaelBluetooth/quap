using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quap.Models;

namespace Quap.Services.QandA
{
    public class AnswerService : IAnswerService
    {
        private readonly QuapDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AnswerService(QuapDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public Quap.Models.Answer answerQuestion(CreateOrUpdateAnswerRequest req)
        {
            User currentUser = _currentUserService.CurrentUser;
            Quap.Models.Answer newAnswer = new Quap.Models.Answer()
            {
                body = req.body,
                questionId = req.questionId,
                created = DateTime.UtcNow,
                lastModified = DateTime.UtcNow,
                createdById = currentUser.id,
            };
            newAnswer = _context.Answers.Add(newAnswer).Entity;
            _context.SaveChanges();

            return newAnswer;
        }

        public Answer updateAnswer(Guid id, CreateOrUpdateAnswerRequest req)
        {
            Answer updatedAnswer = _context.Answers.Find(id);
            updatedAnswer.body = req.body;
            updatedAnswer.lastModified = DateTime.UtcNow;

            _context.SaveChanges();

            return updatedAnswer;
        }

        public AnswerComment comment(CommentRequest req)
        {
            User currentUser = _currentUserService.CurrentUser;

            AnswerComment comment = new AnswerComment()
            {
                comment = req.comment,
                commenterId = currentUser.id,
                lastModified = DateTime.UtcNow,
                created = DateTime.UtcNow,
                answerId = req.postId
            };
            comment = _context.AnswerComments.Add(comment).Entity;

            _context.SaveChanges();

            return comment;
        }

        public AnswerVote vote(VoteRequest req)
        {
            User currentUser = _currentUserService.CurrentUser;

            AnswerVote existing = _context.AnswerVotes.FirstOrDefault(v => v.voterId.Equals(currentUser.id));
            if (null != existing)
            {
                _context.AnswerVotes.Remove(existing);
            }

            AnswerVote newVote = null;
            if (req.voteType.Equals(VoteTypes.DOWNVOTE) || req.voteType.Equals(VoteTypes.UPVOTE))
            {
                newVote = _context.AnswerVotes.Add(new AnswerVote()
                {
                    voteType = req.voteType,
                    voterId = currentUser.id,
                    answerId = req.postId
                }).Entity;
            }

            _context.SaveChanges();

            return newVote;
        }

        public Answer accept(Guid id)
        {
            Answer acceptMe = _context.Answers.Find(id);

            foreach (Answer otherAcceptedAnswer in _context.Answers.Where(a => a.questionId == acceptMe.id && a.id != acceptMe.id && a.accepted))
            {
                otherAcceptedAnswer.accepted = false;
                _context.Answers.Update(otherAcceptedAnswer);
            }

            acceptMe.accepted = true;
            _context.Answers.Update(acceptMe);

            _context.SaveChanges();

            return acceptMe;
        }

        public Answer unaccept(Guid id)
        {
            Answer acceptMe = _context.Answers.Find(id);
            acceptMe.accepted = false;
            _context.Answers.Update(acceptMe);
            _context.SaveChanges();
            return acceptMe;
        }

        public bool isAnswerOwner(Guid id)
        {
            return _context.Answers.Find(id).createdById == _currentUserService.CurrentUser.id;
        }

        public bool isQuestionOwner(Guid id)
        {
            return _context.Answers.Include(a => a.question).FirstOrDefault(a => a.id == id).question.createdById == _currentUserService.CurrentUser.id;
        }
    }
}