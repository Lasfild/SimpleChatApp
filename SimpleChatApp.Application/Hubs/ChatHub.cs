using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

public class ChatHub : Hub
{
    private static ConcurrentDictionary<string, int> connectedUsers = new ConcurrentDictionary<string, int>();

    public async Task JoinChat(int chatId, int userId)
    {
        connectedUsers[Context.ConnectionId] = chatId;
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        await Clients.Group(chatId.ToString()).SendAsync("UserConnected", userId);
    }

    public async Task SendMessage(int chatId, int userId, string message)
    {
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", userId, message);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        connectedUsers.TryRemove(Context.ConnectionId, out _);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task ChatDeleted(int chatId)
    {
        await Clients.Group(chatId.ToString()).SendAsync("ChatDeleted");
    }

    public int? GetConnectedChatId(string connectionId)
    {
        if (connectedUsers.TryGetValue(connectionId, out int chatId))
        {
            return chatId;
        }
        return null;
    }
}
