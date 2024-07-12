using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimpleChatApp.DataAccess.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Chat")]
        public int ChatId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}