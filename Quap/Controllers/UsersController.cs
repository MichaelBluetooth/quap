using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quap.Models;
using Quap.Models.DTO;
using Quap.Services.UserManagement;

namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IAuthService _auth;
        private readonly IUserManagementService _userManagementService;
        private readonly HttpContext _httpContext;

        public UsersController(ILogger<UsersController> logger, IAuthService auth, IUserManagementService userManagementService, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _auth = auth;
            _userManagementService = userManagementService;
            _httpContext = httpContext.HttpContext;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterRequest req)
        {
            if (_userManagementService.userExists(req))
            {
                return BadRequest($"User with username '{req.username}' or email '{req.email}' already exists.");
            }
            else
            {
                User newUser = _userManagementService.register(req);
                return Ok(newUser);
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDetails>> Login([FromBody] AuthRequest AuthRequest)
        {
            bool loggedIn = await _auth.login(AuthRequest);

            if (loggedIn)
            {
                return Ok(_userManagementService.getUserDetails(AuthRequest.username));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("logout")]
        public async Task<ActionResult> Logout()
        {
            await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // await _auth.logout();
            return Ok();
        }

        [HttpGet]
        [Route("{id}/details")]
        public ActionResult<UserDetails> UserDetails(Guid id)
        {
            return Ok(_userManagementService.getUserDetails(id));
        }

        [HttpGet]
        [Route("currentuser")]
        public ActionResult<UserDetails> CurrentUser(Guid id)
        {
            return Ok(_userManagementService.getUserDetails(User.Identity.Name));
        }
    }
}
