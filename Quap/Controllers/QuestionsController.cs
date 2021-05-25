﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quap.Models;
using Quap.Models.DTO;
using Quap.Permissions;
using Quap.Services;
using Quap.Services.QandA;

namespace Quap.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly QuapDbContext _ctx;
        private readonly IQuestionService _questionService;
        private readonly ILogger<QuestionsController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthorizationService _authorizationService;

        public QuestionsController(QuapDbContext ctx, IQuestionService questionService, ILogger<QuestionsController> logger, ICurrentUserService currentUserService, IAuthorizationService authorizationService)
        {
            _ctx = ctx;
            _questionService = questionService;
            _logger = logger;
            _currentUserService = currentUserService;
            _authorizationService = authorizationService;
        }

        private QuestionDetail _getWithDetails(Guid id)
        {
            User currentUser = _currentUserService.CurrentUser;

            QuestionDetail detail = _ctx.Questions
                            .Include(q => q.answers)
                                .ThenInclude(a => a.createdBy)
                            .Include(q => q.answers)
                                .ThenInclude(a => a.comments)
                            .Include(q => q.createdBy)
                            .Include(q => q.comments)
                            .Include(q => q.tags)
                                .ThenInclude(t => t.tag)
                            .Select(q => new QuestionDetail()
                            {
                                id = q.id,
                                createdByUsername = q.createdBy.username,
                                created = q.created,
                                lastModified = q.lastModified,
                                tags = q.tags.Select(t => t.tag.name).ToList(),
                                comments = q.comments,
                                title = q.title,
                                body = q.body,
                                // votesCount = q.votes.Count(),
                                // userVoteType = q.votes.Any(v => v.voterId == currentUser.id && v.voteType.Equals(VoteTypes.DOWNVOTE)) ? VoteTypes.DOWNVOTE :
                                //     q.votes.Any(v => v.voterId == currentUser.id && v.voteType.Equals(VoteTypes.UPVOTE)) ? VoteTypes.UPVOTE : VoteTypes.NONE,
                                answers = q.answers.Select(a => new AnswerDetail()
                                {
                                    id = a.id,
                                    accepted = a.accepted,
                                    body = a.body,
                                    votesCount = 0,
                                    comments = a.comments,
                                    createdByUsername = a.createdBy.username,
                                    created = a.created,
                                    lastModified = a.lastModified
                                }).OrderBy(a => a.accepted).ToList(),
                            })
                            .FirstOrDefault(q => q.id == id);

            detail.votesCount = (-1 * _ctx.QuestionVotes.Count(v => v.questionId == detail.id && v.voteType == VoteTypes.DOWNVOTE)) + _ctx.QuestionVotes.Count(v => v.questionId == detail.id && v.voteType == VoteTypes.UPVOTE);
            detail.userVoteType = _ctx.QuestionVotes.Any(v => v.questionId == detail.id && v.voterId == currentUser.id && v.voteType.Equals(VoteTypes.DOWNVOTE)) ? VoteTypes.DOWNVOTE :
                                _ctx.QuestionVotes.Any(v => v.questionId == detail.id && v.voterId == currentUser.id && v.voteType.Equals(VoteTypes.UPVOTE)) ? VoteTypes.UPVOTE : VoteTypes.NONE;

            foreach (AnswerDetail aDetail in detail.answers)
            {
                aDetail.votesCount = (-1 * _ctx.AnswerVotes.Count(v => v.answerId == aDetail.id && v.voteType == VoteTypes.DOWNVOTE)) + _ctx.AnswerVotes.Count(v => v.answerId == aDetail.id && v.voteType == VoteTypes.UPVOTE);
                aDetail.userVoteType = _ctx.AnswerVotes.Any(v => v.answerId == aDetail.id && v.voterId == currentUser.id && v.voteType.Equals(VoteTypes.DOWNVOTE)) ? VoteTypes.DOWNVOTE :
                                _ctx.AnswerVotes.Any(v => v.answerId == aDetail.id && v.voterId == currentUser.id && v.voteType.Equals(VoteTypes.UPVOTE)) ? VoteTypes.UPVOTE : VoteTypes.NONE;

            }

            return detail;
        }

        [HttpGet]
        public ActionResult<QuestionSearchResults> Get([FromQuery] string text = "", [FromQuery] string tag = "", [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
        {
            return _questionService.search(pageNumber, pageSize, text, tag);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<QuestionDetail> GetById([FromRoute] Guid id)
        {
            QuestionDetail question = _getWithDetails(id);

            if (null == question)
            {
                return NotFound();
            }
            else
            {
                return Ok(question);
            }
        }

        [HttpPost]
        public ActionResult<QuestionDetail> Post([FromBody] CreateOrUpdateQuestionRequest req)
        {
            Question created = _questionService.createQuestion(req);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, _getWithDetails(created.id));
        }

        [HttpPut]
        public async Task<ActionResult<QuestionDetail>> Put([FromRoute] Guid id, [FromBody] CreateOrUpdateQuestionRequest req)
        {
            Question question = _ctx.Questions.Find(id);
            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, question, IsOwnerRequirement.name);
            if (authResult.Succeeded)
            {
                Question updated = _questionService.updateQuestion(id, req);
                return Ok(_getWithDetails(updated.id));
            }
            else
            {
                return StatusCode(403, "Only the owner of the question may update it.");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<QuestionDetail>> Delete([FromRoute] Guid id)
        {
            Question question = _ctx.Questions.Find(id);
            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, question, IsOwnerRequirement.name);
            if (authResult.Succeeded)
            {
                _questionService.deleteQuestion(id);
                return NoContent();
            }
            else
            {
                return StatusCode(403, "Only the owner of the question may delete it.");
            }
        }

        [HttpPost]
        [Route("comment")]
        public ActionResult<QuestionComment> PostComment([FromBody] CommentRequest req)
        {
            QuestionComment created = _questionService.comment(req);
            return Ok(created);
        }

        [HttpPost]
        [Route("vote")]
        public ActionResult<QuestionDetail> Vote([FromBody] VoteRequest req)
        {
            QuestionVote vote = _questionService.vote(req);
            return Ok(_getWithDetails(req.postId));
        }
    }
}
