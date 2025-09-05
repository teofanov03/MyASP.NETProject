using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoBlog.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;
        public DateTime? DueDate { get; set; }


        public string UserId { get; set; } = string.Empty;

        public IdentityUser? User { get; set; }
    }
}
