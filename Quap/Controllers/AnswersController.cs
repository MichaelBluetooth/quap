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
    public class AnswersController : ControllerBase
    {
        private readonly QuapDbContext _ctx;
        private readonly IAnswerService _answerService;
        private readonly ILogger<AnswersController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthorizationService _authorizationService;


        public AnswersController(QuapDbContext ctx,
                                 IAnswerService answerService,
                                 ILogger<AnswersController> logger,
                                 ICurrentUserService currentUserService,
                                 IAuthorizationService authorizationService)
        {
            _ctx = ctx;
            _answerService = answerService;
            _logger = logger;
            _currentUserService = currentUserService;
            _authorizationService = authorizationService;
        }

        private AnswerDetail _getWithDetails(Guid id)
        {
            User currentUser = _currentUserService.CurrentUser;

            AnswerDetail detail = _ctx.Answers
                            .Include(q => q.createdBy)
                            .Include(q => q.comments)
                                .ThenInclude(c => c.commenter)
                            .Select(a => new AnswerDetail()
                            {
                                id = a.id,
                                accepted = a.accepted,
                                body = a.body,
                                votesCount = 0,
                                comments = a.comments,
                                createdByUsername = a.createdBy.username,
                                created = a.created,
                                lastModified = a.lastModified
                            })
                            .FirstOrDefault(q => q.id == id);

            detail.votesCount = (-1 * _ctx.AnswerVotes.Count(v => v.answerId == detail.id && v.voteType == VoteTypes.DOWNVOTE)) + _ctx.AnswerVotes.Count(v => v.answerId == detail.id && v.voteType == VoteTypes.UPVOTE);
            detail.userVoteType = _ctx.AnswerVotes.Any(v => v.answerId == detail.id && v.voterId == currentUser.id && v.voteType.Equals(VoteTypes.DOWNVOTE)) ? VoteTypes.DOWNVOTE :
                            _ctx.AnswerVotes.Any(v => v.answerId == detail.id && v.voterId == currentUser.id && v.voteType.Equals(VoteTypes.UPVOTE)) ? VoteTypes.UPVOTE : VoteTypes.NONE;

            return detail;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<AnswerDetail> GetById([FromRoute] Guid id)
        {
            AnswerDetail answer = _getWithDetails(id);
            if (null == answer)
            {
                return NotFound();
            }
            else
            {
                return Ok(answer);
            }
        }

        [HttpPost]
        public ActionResult<AnswerDetail> Post([FromBody] CreateOrUpdateAnswerRequest req)
        {
            Answer created = _answerService.answerQuestion(req);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, _getWithDetails(created.id));
        }

        [HttpPut]
        public async Task<ActionResult<AnswerDetail>> Put([FromRoute] Guid id, [FromBody] CreateOrUpdateAnswerRequest req)
        {
            Answer answer = _ctx.Answers.Find(id);
            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, answer, IsOwnerRequirement.name);
            if (authResult.Succeeded)
            {
                _answerService.updateAnswer(id, req);
                return Ok(_getWithDetails(id));
            }
            else
            {
                return StatusCode(403, "Only the owner of an answer may update it.");
            }
        }

        [HttpPost]
        [Route("comment")]
        public ActionResult<QuestionComment> PostComment([FromBody] CommentRequest req)
        {
            AnswerComment created = _answerService.comment(req);
            return Ok(created);
        }

        [HttpPost]
        [Route("vote")]
        public ActionResult<QuestionDetail> Vote([FromBody] VoteRequest req)
        {
            AnswerVote vote = _answerService.vote(req);
            return Ok(_getWithDetails(req.postId));
        }

        [HttpPost]
        [Route("accept")]
        public async Task<ActionResult<QuestionDetail>> Accept([FromBody] AnswerAcceptRequest req)
        {
            Question question = _ctx.Answers.Include(a => a.question).FirstOrDefault(a => a.id == req.answerId).question;
            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, question, IsOwnerRequirement.name);
            if (authResult.Succeeded)
            {
                Answer answer = _answerService.accept(req.answerId);
                return Ok(_getWithDetails(req.answerId));
            }
            else
            {
                return StatusCode(403, "Only the user who created the question may accept an answer for it.");
            }
        }

        [HttpPost]
        [Route("unaccept")]
        public async Task<ActionResult<QuestionDetail>> Unaccept([FromBody] AnswerAcceptRequest req)
        {
            Question question = _ctx.Answers.Include(a => a.question).FirstOrDefault(a => a.id == req.answerId).question;
            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, question, IsOwnerRequirement.name);
            if (authResult.Succeeded)
            {
                _answerService.unaccept(req.answerId);
                return Ok(_getWithDetails(req.answerId));
            }
            else
            {
                return StatusCode(403, "Only the user who created the question may unaccept an answer for it.");
            }
        }
    }
}
