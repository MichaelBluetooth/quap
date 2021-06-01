using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quap.Models;
using Quap.Models.DTO;

namespace Quap.Services.QandA
{
    public class QuestionService : IQuestionService
    {
        private readonly QuapDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public QuestionService(QuapDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public Question createQuestion(CreateOrUpdateQuestionRequest req)
        {
            User currentUser = _currentUserService.CurrentUser;

            Question newQuestion = new Question()
            {
                createdById = currentUser.id,
                title = req.title,
                body = req.body,
                created = DateTime.UtcNow,
                lastModified = DateTime.UtcNow
            };
            newQuestion = _context.Questions.Add(newQuestion).Entity;

            foreach (string tag in req.tags)
            {
                Tag existing = _context.Tags.FirstOrDefault(t => t.name.ToLower().Equals(tag.ToLower()));
                if (null == existing)
                {
                    existing = _context.Tags.Add(new Tag()
                    {
                        name = tag
                    }).Entity;
                }

                _context.QuestionTags.Add(new QuestionTag()
                {
                    questionId = newQuestion.id,
                    tagId = existing.id
                });
            }


            _context.SaveChanges();

            return newQuestion;
        }

        public void deleteQuestion(Guid id)
        {
            _context.Questions.Remove(_context.Questions.Find(id));
        }

        public Question updateQuestion(Guid id, CreateOrUpdateQuestionRequest req)
        {
            Question updatedQuestion = _context.Questions.Find(id);
            updatedQuestion.title = req.title;
            updatedQuestion.body = req.body;
            updatedQuestion.lastModified = DateTime.UtcNow;

            foreach (QuestionTag tag in _context.QuestionTags.Where(t => t.questionId == updatedQuestion.id))
            {
                _context.QuestionTags.Remove(tag);
            }

            foreach (string tag in req.tags)
            {
                Tag existing = _context.Tags.FirstOrDefault(t => t.name.ToLower().Equals(tag.ToLower()));
                if (null == existing)
                {
                    existing = _context.Tags.Add(new Tag()
                    {
                        name = tag
                    }).Entity;
                }

                _context.QuestionTags.Add(new QuestionTag()
                {
                    questionId = updatedQuestion.id,
                    tagId = existing.id
                });
            }

            _context.SaveChanges();

            return updatedQuestion;
        }

        public QuestionComment comment(CommentRequest req)
        {
            User currentUser = _currentUserService.CurrentUser;

            QuestionComment comment = new QuestionComment()
            {
                comment = req.comment,
                commenterId = currentUser.id,
                lastModified = DateTime.UtcNow,
                created = DateTime.UtcNow,
                questionId = req.postId
            };
            comment = _context.QuestionComments.Add(comment).Entity;

            _context.SaveChanges();

            return comment;
        }

        public QuestionVote vote(VoteRequest req)
        {
            User currentUser = _currentUserService.CurrentUser;

            QuestionVote existing = _context.QuestionVotes.FirstOrDefault(v => v.voterId.Equals(currentUser.id));
            if (null != existing)
            {
                _context.QuestionVotes.Remove(existing);
            }

            QuestionVote newVote = null;
            if (req.voteType.Equals(VoteTypes.DOWNVOTE) || req.voteType.Equals(VoteTypes.UPVOTE))
            {
                newVote = _context.QuestionVotes.Add(new QuestionVote()
                {
                    voteType = req.voteType,
                    voterId = currentUser.id,
                    questionId = req.postId
                }).Entity;
            }

            _context.SaveChanges();

            return newVote;
        }

        public QuestionSearchResults search(int pageNumber = 1, int pageSize = 10, string text = "", string tag = "")
        {
            User currentUser = _currentUserService.CurrentUser;

            IQueryable<Question> query = _context.Questions
                .Include(q => q.answers)
                .Include(q => q.createdBy)
                .Include(q => q.tags)
                .ThenInclude(t => t.tag);


            if (!string.IsNullOrEmpty(text))
            {
                query = query.Where(q => q.title.Contains(text, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(q => q.tags.Any(t => t.tag.name.Equals(tag, StringComparison.CurrentCultureIgnoreCase)));
            }

            query = query.OrderByDescending(q => q.created);

            var count = query.Count();
            List<Question> questions = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            List<QuestionSummary> questionSummaries = questions.Select(q => new QuestionSummary()
            {
                id = q.id,
                title = q.title,
                created = q.created,
                lastModified = q.lastModified,
                createdByUsername = q.createdBy.username,
                createdByUserId = q.createdById.Value,
                // answersCount = q.answers.Count(),
                hasAcceptedAnswer = q.answers.Any(a => a.accepted),
                tags = q.tags.Select(t => t.tag.name).ToList()
            }).ToList();

            foreach (QuestionSummary summary in questionSummaries)
            {
                summary.answersCount = _context.Answers.Count(a => a.questionId == summary.id);
                summary.votesCount = (-1 * _context.QuestionVotes.Count(v => v.questionId == summary.id && v.voteType == VoteTypes.DOWNVOTE)) + _context.QuestionVotes.Count(v => v.questionId == summary.id && v.voteType == VoteTypes.UPVOTE);
                summary.userVoteType = _context.QuestionVotes.Any(v => v.questionId == summary.id && v.voterId == currentUser.id && v.voteType.Equals(VoteTypes.DOWNVOTE)) ? VoteTypes.DOWNVOTE :
                                    _context.QuestionVotes.Any(v => v.questionId == summary.id && v.voterId == currentUser.id && v.voteType.Equals(VoteTypes.UPVOTE)) ? VoteTypes.UPVOTE : VoteTypes.NONE;
            }

            return new QuestionSearchResults(questionSummaries, count, pageNumber, pageSize);
        }

        public bool isQuestionOwner(Guid id)
        {
            return _context.Questions.Find(id).createdById == _currentUserService.CurrentUser.id;
        }
    }
}