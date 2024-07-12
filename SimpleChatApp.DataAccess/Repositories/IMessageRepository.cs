using SimpleChatApp.DataAccess.Models;
using System.Threading.Tasks;

namespace SimpleChatApp.DataAccess.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
    }
}
