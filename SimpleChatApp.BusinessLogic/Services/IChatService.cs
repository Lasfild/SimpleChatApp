using SimpleChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChatApp.BusinessLogic.Services
{
    public interface IChatService
    {
        Task<List<Chat>> GetAllChatsAsync();
        Task<Chat> GetChatByIdAsync(int chatId);
        Task CreateChatAsync(Chat chat);
        Task DeleteChatAsync(int chatId, string userId);
    }
}
