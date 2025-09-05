using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TodoBlog.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public Post? Post { get; set; }
        [Required]
        public string AuthorUserName { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }  = string.Empty; 
        public IdentityUser? User { get; set; }
    }
}
