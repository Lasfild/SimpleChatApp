using SimpleChatApp.DataAccess.Models;
using SimpleChatApp.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleChatApp.BusinessLogic.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;

        public ChatService(IChatRepository chatRepository, IMessageRepository messageRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
        }

        public async Task<List<Chat>> GetAllChatsAsync()
        {
            return (await _chatRepository.GetAllChatsAsync()).ToList();
        }

        public async Task<Chat> GetChatByIdAsync(int id)
        {
            return await _chatRepository.GetChatAsync(id);
        }

        public async Task CreateChatAsync(Chat chat)
        {
            if (chat == null)
            {
                throw new ArgumentNullException(nameof(chat), "Chat cannot be null.");
            }

            await _chatRepository.AddChatAsync(chat);
        }

        public async Task DeleteChatAsync(int id, int userId)
        {
            var chat = await _chatRepository.GetChatAsync(id);
            if (chat != null && chat.UserId == userId)
            {
                await _chatRepository.DeleteChatAsync(id);
            }
        }

        public async Task<List<Chat>> SearchChatsAsync(string searchTerm)
        {
            return (await _chatRepository.GetAllChatsAsync())
                .Where(c => c.Name.Contains(searchTerm))
                .ToList();
        }

        public async Task AddMessageAsync(Message message)
        {
            await _messageRepository.AddMessageAsync(message);
        }
    }
}
