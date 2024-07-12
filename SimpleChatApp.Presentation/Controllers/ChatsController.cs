using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimpleChatApp.BusinessLogic.Services;
using SimpleChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChatApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;
        private static int? _connectedChatId; // Static variable to store ChatId

        public ChatsController(IChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats()
        {
            var chats = await _chatService.GetAllChatsAsync();
            return Ok(chats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Chat>> GetChat(int id)
        {
            var chat = await _chatService.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return Ok(chat);
        }

        [HttpPost]
        public async Task<ActionResult<Chat>> PostChat(Chat chat)
        {
            try
            {
                await _chatService.CreateChatAsync(chat);
                return CreatedAtAction(nameof(GetChat), new { id = chat.Id }, chat);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create chat: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id, [FromBody] int userId)
        {
            try
            {
                var chat = await _chatService.GetChatByIdAsync(id);
                if (chat == null)
                {
                    return NotFound("Chat not found.");
                }

                if (chat.UserId != userId)
                {
                    return StatusCode(403, "There are no permissions to do the operation");
                }

                await _chatService.DeleteChatAsync(id, userId);
                await _hubContext.Clients.Group(id.ToString()).SendAsync("ChatDeleted", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete chat: {ex.Message}");
            }
        }


        [HttpPost("{id}/connect")]
        public async Task<IActionResult> ConnectToChat(int id, [FromBody] int userId)
        {
            var chat = await _chatService.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            _connectedChatId = id;

            await _hubContext.Groups.AddToGroupAsync(userId.ToString(), id.ToString());
            await _hubContext.Clients.Group(id.ToString()).SendAsync("UserConnected", userId);

            return Ok();
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            try
            {
                var chatId = _connectedChatId;

                if (chatId == null)
                {
                    return BadRequest("Must connect to a chat before sending a message.");
                }

                message.ChatId = chatId.Value;
                await _chatService.AddMessageAsync(message);

                await _hubContext.Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", message.UserId, message.Content);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send message: {ex.Message}");
            }
        }
    }
}