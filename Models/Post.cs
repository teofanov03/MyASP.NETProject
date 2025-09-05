using Microsoft.AspNetCore.Identity;

namespace TodoBlog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string AuthorUserName { get; set; } = string.Empty;
        
        public List<Comment> Comments { get; set; } = new();
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }
    }
}
