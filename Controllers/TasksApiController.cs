using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBlog.Data;
using TodoBlog.Models;

namespace TodoBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class TasksApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public TasksApiController(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var userId = _userManager.GetUserId(User);

            IEnumerable<TaskItem> tasks = await _db.Tasks
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.IsDone)
                .ThenBy(t => t.DueDate)
                .ToListAsync(); 

            return Ok(tasks);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var userId = _userManager.GetUserId(User);
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return NotFound();
            return Ok(task);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = _userManager.GetUserId(User);
            model.UserId = userId;

            _db.Tasks.Add(model);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = model.Id }, model);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updated)
        {
            var userId = _userManager.GetUserId(User);
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return NotFound();

            task.Title = updated.Title;
            task.Description = updated.Description;
            task.DueDate = updated.DueDate;
            task.IsDone = updated.IsDone;

            await _db.SaveChangesAsync();
            return Ok(task);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = _userManager.GetUserId(User);
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return NotFound();

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
            return NoContent();
        }

       
        [HttpPost("toggle/{id}")]
        public async Task<IActionResult> ToggleTask(int id)
        {
            var userId = _userManager.GetUserId(User);
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return NotFound();

            task.IsDone = !task.IsDone;
            await _db.SaveChangesAsync();

            return Ok(task);
        }
    }
}
