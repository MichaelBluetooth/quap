using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Quap.Services;

namespace Quap.Permissions
{
    public class IsOwnerRequirement : IAuthorizationRequirement
    {
        public static string name = "IsOwner";
    }

    public class IsOwnerAuthorizationHandler : AuthorizationHandler<IsQuestionOwnerRequirement, IOwnerable>
    {
        private readonly ICurrentUserService _currentUserSvc;

        public IsOwnerAuthorizationHandler(ICurrentUserService currentUserSvc)
        {
            _currentUserSvc = currentUserSvc;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   IsQuestionOwnerRequirement requirement,
                                                   IOwnerable ownerable)
        {
            if (_currentUserSvc.CurrentUser.id == ownerable.getOwnerId())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}