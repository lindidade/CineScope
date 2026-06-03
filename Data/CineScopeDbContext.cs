using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CineScope.Models;

namespace CineScope.Data
{
    public class CineScopeDbContext : IdentityDbContext<IdentityUser>
    {
        public CineScopeDbContext(DbContextOptions<CineScopeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
    }
}