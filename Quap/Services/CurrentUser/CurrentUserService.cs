using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Quap.Models;

namespace Quap.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly QuapDbContext _ctx;

        public CurrentUserService(IHttpContextAccessor httpContext, QuapDbContext ctx)
        {
            _httpContext = httpContext;
            _ctx = ctx;
        }

        public ClaimsPrincipal CurrentUserClaims => _httpContext.HttpContext?.User;

        public User CurrentUser => _ctx.Users.FirstOrDefault(u => u.username == _httpContext.HttpContext.User.Identity.Name);

        public bool isInRole(string role)
        {
            return this.CurrentUserClaims?.IsInRole(role) ?? false;
        }
    }
}