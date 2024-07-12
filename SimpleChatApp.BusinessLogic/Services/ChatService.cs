using SimpleChatApp.DataAccess;
using SimpleChatApp.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleChatApp.BusinessLogic.Services
{
    public interface IChatEventNotifier
    {
        Task NotifyChatDeletedAsync(int chatId);
    }

    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly IChatEventNotifier _chatEventNotifier;

        public ChatService(ApplicationDbContext context, IChatEventNotifier chatEventNotifier)
        {
            _context = context;
            _chatEventNotifier = chatEventNotifier;
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

            // Уведомление всех пользователей о закрытии чата
            await _chatEventNotifier.NotifyChatDeletedAsync(id);
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
