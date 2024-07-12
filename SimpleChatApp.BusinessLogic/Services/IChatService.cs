using SimpleChatApp.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChatApp.BusinessLogic.Services
{
    public interface IChatService
    {
        Task<List<Chat>> GetAllChatsAsync();
        Task<Chat> GetChatByIdAsync(int id);
        Task CreateChatAsync(Chat chat);
        Task DeleteChatAsync(int id, int userId);
        Task<List<Chat>> SearchChatsAsync(string searchTerm);
        Task AddMessageAsync(Message message);
    }
}
