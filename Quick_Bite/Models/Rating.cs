using System.ComponentModel.DataAnnotations;

namespace Quick_Bite.Models
{
    // Models/Rating.cs
    public class Rating
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }

        public string? Comment { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Track IP address for basic duplicate checking (without user accounts)
        public string? IpAddress { get; set; }

        // Relationships (only one will be set per rating)
        public int? RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        public int? DrinkId { get; set; }
        public Drink? Drink { get; set; }

        public int? SideDishId { get; set; }
        public SideDish? SideDish { get; set; }
    }
}
