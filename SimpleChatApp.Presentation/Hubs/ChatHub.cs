using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SimpleChatApp.Presentation.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string chatId, string user, string message)
        {
            await Clients.Group(chatId).SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
            await Clients.Group(chatId).SendAsync("UserJoined", Context.ConnectionId);
        }

        public async Task LeaveChat(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
            await Clients.Group(chatId).SendAsync("UserLeft", Context.ConnectionId);
        }
    }
}
