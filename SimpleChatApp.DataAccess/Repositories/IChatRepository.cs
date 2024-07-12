using SimpleChatApp.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChatApp.DataAccess.Repositories
{
    public interface IChatRepository
    {
        Task<Chat> GetChatAsync(int id);
        Task<IEnumerable<Chat>> GetAllChatsAsync();
        Task AddChatAsync(Chat chat);
        Task<Chat> GetChatByIdAsync(int chatId);
        Task DeleteChatAsync(int chatId);
    }
}
