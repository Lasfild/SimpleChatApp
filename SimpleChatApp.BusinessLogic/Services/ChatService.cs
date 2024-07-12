using SimpleChatApp.DataAccess;
using SimpleChatApp.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task DeleteChatAsync(int id, int userId)
        {
            var chat = await _context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chat == null)
            {
                throw new KeyNotFoundException("Чат не найден");
            }

            if (chat.UserId != userId)
            {
                throw new UnauthorizedAccessException("Нет прав на выполнение операции");
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }

        public async Task AddMessageAsync(Message message)
        {
            var chat = await _context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == message.ChatId);

            if (chat != null)
            {
                chat.Messages.Add(message);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Chat with id {message.ChatId} not found.");
            }
        }

        public async Task<List<Chat>> SearchChatsAsync(string searchTerm)
        {
            return await _context.Chats
                .Include(c => c.Messages)
                .Where(c => c.Name.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
