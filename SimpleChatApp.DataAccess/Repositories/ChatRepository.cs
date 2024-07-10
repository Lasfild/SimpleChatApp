using Microsoft.EntityFrameworkCore;
using SimpleChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatApp.DataAccess.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Chat> GetChatAsync(int id)
        {
            return await _context.Chats.FindAsync(id);
        }

        public async Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            return await _context.Chats.ToListAsync();
        }

        public async Task AddChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChatAsync(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat != null)
            {
                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
            }
        }
    }
}
