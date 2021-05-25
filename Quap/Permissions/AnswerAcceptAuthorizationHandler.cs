using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Quap.Models;
using Quap.Services;
using Quap.Services.QandA;

namespace Quap.Permissions
{
    public class IsQuestionOwnerRequirement : IAuthorizationRequirement
    {
        public static string name = "IsQuestionOwner";
    }

    public class AnswerAcceptAuthorizationHandler : AuthorizationHandler<IsQuestionOwnerRequirement, AnswerAcceptRequest>
    {
        private readonly ICurrentUserService _currentUserSvc;
        private readonly QuapDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AnswerAcceptAuthorizationHandler(ICurrentUserService currentUserSvc, QuapDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _currentUserSvc = currentUserSvc;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   IsQuestionOwnerRequirement requirement,
                                                   AnswerAcceptRequest requestBody)
        {
            Question questionToAcceptAnswerFor = (from q in _db.Questions
                                                  join a in _db.Answers on requestBody.answerId equals a.id
                                                  where q.id == a.questionId
                                                  select q).FirstOrDefault();

            if (null != questionToAcceptAnswerFor && questionToAcceptAnswerFor.createdById == _currentUserSvc.CurrentUser.id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}