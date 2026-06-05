using System.Text.Json;
using CineScope.Models;

namespace CineScope.Services
{
    public class TmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TmdbService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["TmdbApi:ApiKey"]!;
        }

        
        public async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            var url = $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(query)}&language=sv-SE";

            var response = await _httpClient.GetStringAsync(url);
            var json = JsonDocument.Parse(response);
            var results = json.RootElement.GetProperty("results");

            var movies = new List<Movie>();

            foreach (var item in results.EnumerateArray().Take(10))
            {
                var posterPath = item.TryGetProperty("poster_path", out var p) ? p.GetString() : null;

                movies.Add(new Movie
                {
                    Title = item.GetProperty("title").GetString() ?? "",
                    Description = item.TryGetProperty("overview", out var o) ? o.GetString() ?? "" : "",
                    ReleaseYear = ParseYear(item),
                    Rating = item.TryGetProperty("vote_average", out var v) ? Math.Round(v.GetDouble(), 1) : 0,
                    PosterUrl = posterPath != null ? $"https://image.tmdb.org/t/p/w500{posterPath}" : "",
                    Genre = ""
                });
            }

            return movies;
        }

        private int ParseYear(JsonElement item)
        {
            if (item.TryGetProperty("release_date", out var date))
            {
                var dateStr = date.GetString();
                if (!string.IsNullOrEmpty(dateStr) && dateStr.Length >= 4)
                    return int.Parse(dateStr[..4]);
            }
            return 0;
        }
    }
}
