using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleChatApp.DataAccess.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        public List<Message> Messages { get; set; }
    }
}
