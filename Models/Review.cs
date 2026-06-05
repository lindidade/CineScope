using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CineScope.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 10)]
        public int Rating { get; set; }

        [Required]
        [StringLength(500)]
        public string Comment { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public string UserId { get; set; } = "";
        public IdentityUser? User { get; set; }
    }
}