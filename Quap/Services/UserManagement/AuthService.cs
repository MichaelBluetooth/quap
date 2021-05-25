using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Quap.Models;

namespace Quap.Services.UserManagement
{
    public class AuthService : IAuthService
    {
        private readonly QuapDbContext _context;
        private readonly HttpContext _httpContext;

        public AuthService(QuapDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext.HttpContext;
        }

        public async Task<bool> login(AuthRequest authRequest)
        {
            User user = _context.Users.FirstOrDefault(u => u.username.Equals(authRequest.username));

            if (null != user && BCrypt.Net.BCrypt.Verify(authRequest.password, user.password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.username),
                    // new Claim(ClaimTypes.Role,  user.isAdmin ? "Administrator" : "General")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    RedirectUri = "https://localhost:5001/"
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task logout()
        {
            await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}