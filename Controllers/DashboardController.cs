using CineScope.Data;
using CineScope.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineScope.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly CineScopeDbContext _context;

        public DashboardController(CineScopeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel();

            viewModel.TotalMovies = await _context.Movies.CountAsync();
            viewModel.TotalReviews = await _context.Reviews.CountAsync();
            viewModel.TotalUsers = await _context.Users.CountAsync();

            if (viewModel.TotalReviews > 0)
            {
                var avg = await _context.Reviews.AverageAsync(r => r.Rating);
                viewModel.AverageRating = Math.Round(avg, 1);
            }

            viewModel.RecentMovies = await _context.Movies
                .OrderByDescending(m => m.Id)
                .Take(5)
                .ToListAsync();

            viewModel.RecentReviews = await _context.Reviews
                .Include(r => r.Movie)
                .OrderByDescending(r => r.Id)
                .Take(5)
                .ToListAsync();

            viewModel.TopGenres = _context.Movies
                .AsEnumerable()
                .Where(m => !string.IsNullOrEmpty(m.Genre))
                .GroupBy(m => m.Genre)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .ToDictionary(g => g.Key, g => g.Count());

            return View(viewModel);
        }
    }
}