using Microsoft.EntityFrameworkCore;
using SimpleChatApp.DataAccess.Models;
using System.Threading.Tasks;

namespace SimpleChatApp.DataAccess.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
