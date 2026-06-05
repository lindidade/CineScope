namespace CineScope.Models
{
    public class DashboardViewModel
    {
        public int TotalMovies { get; set; }
        public int TotalUsers { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
      
        public List<Movie> RecentMovies { get; set; } = new List<Movie>();
        public List<Review> RecentReviews { get; set; } = new List<Review>();

        public Dictionary<string, int> TopGenres { get; set; } = new Dictionary<string, int>();
    }
}
