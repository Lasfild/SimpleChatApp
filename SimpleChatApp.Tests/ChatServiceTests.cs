using System;
using System.Threading.Tasks;
using Moq;
using SimpleChatApp.BusinessLogic.Services;
using SimpleChatApp.DataAccess.Models;
using SimpleChatApp.DataAccess.Repositories;
using Xunit;

namespace SimpleChatApp.Tests
{
    public class ChatServiceTests
    {
        private readonly ChatService _chatService;
        private readonly Mock<IChatRepository> _chatRepositoryMock;
        private readonly Mock<IMessageRepository> _messageRepositoryMock;

        public ChatServiceTests()
        {
            _chatRepositoryMock = new Mock<IChatRepository>();
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _chatService = new ChatService(_chatRepositoryMock.Object, _messageRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateChatAsync_ValidData_CreatesChat()
        {
            // Arrange
            int userId = 123;
            var chat = new Chat { UserId = userId };
            _chatRepositoryMock.Setup(r => r.AddChatAsync(chat)).Returns(Task.CompletedTask);

            // Act
            await _chatService.CreateChatAsync(chat);

            // Assert
            _chatRepositoryMock.Verify(r => r.AddChatAsync(chat), Times.Once);
        }
        [Fact]
        public async Task CreateChatAsync_ExceptionInRepository_ThrowsException()
        {
            // Arrange
            int userId = 123;
            var chat = new Chat { UserId = userId };
            _chatRepositoryMock.Setup(r => r.AddChatAsync(chat)).ThrowsAsync(new Exception("Repository exception"));

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await _chatService.CreateChatAsync(chat));
        }
    }
}
