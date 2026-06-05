using CineScope.Data;
using CineScope.Models;
using CineScope.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineScope.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TmdbController : Controller
    {
        private readonly TmdbService _tmdbService;
        private readonly CineScopeDbContext _context;

        public TmdbController(TmdbService tmdbService, CineScopeDbContext context)
        {
            _tmdbService = tmdbService;
            _context = context;
        }

        
        public IActionResult Search()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return View();

            var movies = await _tmdbService.SearchMoviesAsync(query);
            ViewData["Query"] = query;
            return View("Results", movies);
        }

        
        [HttpPost]
        public async Task<IActionResult> Import(string title, string description,
            int releaseYear, double rating, string posterUrl, string genre)
        {
            var movie = new Movie
            {
                Title = title,
                Description = description,
                ReleaseYear = releaseYear,
                Rating = rating,
                PosterUrl = posterUrl,
                Genre = string.IsNullOrEmpty(genre) ? "Okategoriserad" : genre,
                Duration = 0
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Movies");
        }
    }
}
