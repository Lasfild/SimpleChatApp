using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimpleChatApp.BusinessLogic.Services;
using SimpleChatApp.DataAccess.Models;
using SimpleChatApp.Presentation.Hubs;
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
        public async Task<ActionResult<Chat>> PostChat([FromBody] Chat chat)
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
        public async Task<IActionResult> DeleteChat(int id, [FromBody] string userId)
        {
            try
            {
                await _chatService.DeleteChatAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete chat: {ex.Message}");
            }
        }

        [HttpGet("{chatId}/messages")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(int chatId)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);
            if (chat == null)
            {
                return NotFound();
            }
            return Ok(chat.Messages);
        }
    }
}
