using Microsoft.EntityFrameworkCore;
using SimpleChatApp.DataAccess.Models;
using System.Collections.Generic;

namespace SimpleChatApp.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
