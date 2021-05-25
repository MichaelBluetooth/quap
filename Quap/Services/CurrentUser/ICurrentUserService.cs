using System.Security.Claims;
using Quap.Models;

namespace Quap.Services
{
    public interface ICurrentUserService
    {
        ClaimsPrincipal CurrentUserClaims { get; }
        User CurrentUser { get; }
    }
}