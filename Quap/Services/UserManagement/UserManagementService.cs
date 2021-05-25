using System.Linq;
using Quap.Models;

namespace Quap.Services.UserManagement
{
    public class UserManagementService : IUserManagementService
    {
        private readonly QuapDbContext _context;

        public UserManagementService(QuapDbContext context)
        {
            _context = context;
        }

        public User register(RegisterRequest req)
        {
            User newUser = new User()
            {
                username = req.username,
                email = req.email,
                password = BCrypt.Net.BCrypt.HashPassword(req.password)
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public bool userExists(RegisterRequest req)
        {
            return _context.Users.Any(u => u.username.Equals(req.username, System.StringComparison.CurrentCultureIgnoreCase) || 
            u.email.Equals(req.email, System.StringComparison.CurrentCultureIgnoreCase));
        }
    }
}