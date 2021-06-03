using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quap.Models;
using Quap.Models.DTO;

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
                password = BCrypt.Net.BCrypt.HashPassword(req.password),
                created = DateTime.UtcNow,
                lastModified = DateTime.UtcNow
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

        public UserDetails getUserDetails(Guid id)
        {
            return _context.Users
                .Include(u => u.questions)
                .Include(u => u.answers)
                    .ThenInclude(a => a.question)
                .Select(u => new UserDetails(){
                    id = u.id,
                    created = u.created,
                    email = u.email,
                    username = u.username,
                    answers = u.answers.Select(a => new UserAnswerSummary()
                    {
                        id = a.id,
                        title = a.question.title,
                        created = a.created,
                        questionId = a.questionId.Value
                    }).ToList(),
                    questions = u.questions.Select(q => new UserQuestionSummary()
                        {
                            id = q.id,
                            title = q.title,
                            created = q.created
                        }).ToList()
                })
                .FirstOrDefault(u => u.id == id);
        }

        public UserDetails getUserDetails(string username)
        {
            return getUserDetails(
                _context.Users.FirstOrDefault(u => u.username.Equals(username, StringComparison.CurrentCultureIgnoreCase)).id
            );
        }
    }
}