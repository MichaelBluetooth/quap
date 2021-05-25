using Microsoft.EntityFrameworkCore;

namespace Quap.Models
{
    public class QuapDbContext : DbContext
    {
        public QuapDbContext(DbContextOptions<QuapDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionComment> QuestionComments { get; set; }
        public DbSet<QuestionVote> QuestionVotes { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswerComment> AnswerComments { get; set; }
        public DbSet<AnswerVote> AnswerVotes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }
        public DbSet<User> Users { get; set; }
    }
}