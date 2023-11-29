using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    public class ChatDbContext: DbContext
    {
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Messege> Messeges { get; set; }
        public DbSet<User> Users { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {
        }
    }
}
