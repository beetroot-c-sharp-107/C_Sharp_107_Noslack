using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace backend.Models
{
    public class ChatDbContext : DbContext
    {
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(x => x.LastSeen)
                .HasConversion(
                    v => v.ToLocalTime(),
                    v => v
                );
        }
    }
}
