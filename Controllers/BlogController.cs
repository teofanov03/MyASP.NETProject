using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBlog.Data;
using TodoBlog.Models;

namespace TodoBlog.Controllers
{
    [Authorize] 
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public BlogController(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _db.Posts
                                 .OrderByDescending(p => p.CreatedAt)
                                 .ToListAsync();
            return View(posts);
        }

        public IActionResult Create() => View(new Post());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (!ModelState.IsValid)
                return View(post);
            post.UserId = _userManager.GetUserId(User);

           
            post.AuthorUserName = User.Identity.Name;

           
            post.CreatedAt = DateTime.UtcNow;

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var post = await _db.Posts.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (post == null) return NotFound(); 
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post model)
        {
            var userId = _userManager.GetUserId(User);
            var existingPost = await _db.Posts.AsNoTracking()
                                              .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (existingPost == null) return Unauthorized();

            if (ModelState.IsValid)
            {
                model.Id = id;
                model.UserId = userId; 
                model.CreatedAt = existingPost.CreatedAt;
                _db.Update(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var post = await _db.Posts.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (post == null) return NotFound(); 
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var post = await _db.Posts.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            if (post == null) return Unauthorized(); 

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _db.Posts
                                .Include(p => p.Comments) 
                                .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return NotFound();

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int postId, string body)
        {
            var post = await _db.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null) return NotFound();

            var comment = new Comment
            {
                PostId = postId,
                AuthorUserName = User.Identity.Name ?? "Anonymous", 
                Body = body,
                CreatedAt = DateTime.UtcNow,
                UserId = _userManager.GetUserId(User)
            };

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = postId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _db.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            if (comment.UserId != _userManager.GetUserId(User))
                return Forbid(); 

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = comment.PostId });
        }
        public async Task<IActionResult> EditComment(int id)
        {
            var comment = await _db.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            if (comment.UserId != _userManager.GetUserId(User))
                return Forbid();

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(int id, string body)
        {
            var comment = await _db.Comments.FindAsync(id);
            if (comment == null) return NotFound();
            if (comment.UserId != _userManager.GetUserId(User)) return Forbid();

            comment.Body = body;
            _db.Comments.Update(comment);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = comment.PostId });
        }
    }
}
