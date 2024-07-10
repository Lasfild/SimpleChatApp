using SimpleChatApp.DataAccess;
using SimpleChatApp.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SimpleChatApp.BusinessLogic.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Chat>> GetAllChatsAsync()
        {
            return await _context.Chats.Include(c => c.Messages).ToListAsync();
        }

        public async Task<Chat> GetChatByIdAsync(int id)
        {
            return await _context.Chats.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChatAsync(int id, string userId)
        {
            var chat = await _context.Chats.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == id);
            if (chat != null)
            {
                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
            }
        }
    }
}
