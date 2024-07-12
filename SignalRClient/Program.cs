using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace SignalRClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/hubs/chat")
                .Build();

            connection.On<string, string>("ReceiveMessage", (userId, message) =>
            {
                Console.WriteLine($"Message from {userId}: {message}");
            });

            await connection.StartAsync();
            Console.WriteLine("Connection started");

            Console.WriteLine("Enter chat id:");
            int chatId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter user id:");
            int userId = int.Parse(Console.ReadLine());

            await connection.InvokeAsync("JoinChat", chatId, userId);

            while (true)
            {
                var message = Console.ReadLine();
                if (message.ToLower() == "exit") break;

                await connection.InvokeAsync("SendMessage", chatId, userId, message);
            }

            await connection.StopAsync();
        }
    }
}
