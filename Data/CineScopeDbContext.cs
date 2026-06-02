using Microsoft.EntityFrameworkCore;
using CineScope.Models;

namespace CineScope.Data
{
    public class CineScopeDbContext : DbContext
    {
        public CineScopeDbContext(DbContextOptions<CineScopeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
    }
}
