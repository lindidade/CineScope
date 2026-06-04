using CineScope.Data;
using CineScope.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CineScope.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly CineScopeDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewsController(CineScopeDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int movieId, int rating, string comment)
        {
            if (string.IsNullOrEmpty(comment) || rating < 1 || rating > 10)
                return RedirectToAction("Details", "Movies", new { id = movieId });

            var userId = _userManager.GetUserId(User);

            var review = new Review
            {
                MovieId = movieId,
                Rating = rating,
                Comment = comment,
                UserId = userId!,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        // POST: Reviews/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int movieId)
        {
            var userId = _userManager.GetUserId(User);
            var review = await _context.Reviews.FindAsync(id);

            if (review != null && (review.UserId == userId || User.IsInRole("Admin")))
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Movies", new { id = movieId });
        }
    }
}