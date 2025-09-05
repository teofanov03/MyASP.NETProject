using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBlog.Data;
using TodoBlog.Models;

namespace TodoBlog.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public TasksController(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var items = await _db.Tasks
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.IsDone)
                .ThenBy(t => t.DueDate)
                .ToListAsync();
            return View(items);
        }

        public IActionResult Create() => View(new TaskItem());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem model)
        {
            if (!ModelState.IsValid) return View(model);
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized(); 
            }
            model.UserId = userId; 

            _db.Tasks.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (item == null) return NotFound();
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskItem model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = _userManager.GetUserId(User);
            var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == model.Id && t.UserId == userId);
            if (item == null) return NotFound();

          
            item.Title = model.Title;
            item.Description = model.Description;
            item.DueDate = model.DueDate;
            item.IsDone = model.IsDone;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (item == null) return NotFound();

            _db.Tasks.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (item == null) return NotFound();

            item.IsDone = !item.IsDone;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}