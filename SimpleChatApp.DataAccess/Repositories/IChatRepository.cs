using SimpleChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatApp.DataAccess.Repositories
{
    public interface IChatRepository
    {
        Task<Chat> GetChatAsync(int id);
        Task<IEnumerable<Chat>> GetAllChatsAsync();
        Task AddChatAsync(Chat chat);
        Task DeleteChatAsync(int id);
    }
}
