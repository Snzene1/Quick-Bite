using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quick_Bite.Models
{
    public class SideDish
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Recipe Name")]
        [StringLength(100)]
        public string? RecipeName { get; set; }

        [Display(Name = "Ingredients")]
        public string? Ingredients { get; set; }

        [Display(Name = "Instructions")]
        public string? Instructions { get; set; }

        public enum SideDishType
        {
            Salad,
            Bread,
            Dip,
            Soup,
            VegetableDish,
            PotatoDish,
            RiceDish
        }

        [Display(Name = "Side Dish Type")]
        public SideDishType SideDishCategory { get; set; }

        [Display(Name = "Preparation Time (mins)")]
        [Range(0, 120, ErrorMessage = "Prep time must be between 0-120 minutes")]
        public int PrepTime { get; set; }

        [Display(Name = "Cooking Time (mins)")]
        [Range(0, 180, ErrorMessage = "Cook time must be between 0-180 minutes")]
        public int CookTime { get; set; }

        [Display(Name = "Servings")]
        [Range(1, 20, ErrorMessage = "Servings must be between 1-20")]
        public int Servings { get; set; }

        [Display(Name = "Pairs Well With")]
        [StringLength(200, ErrorMessage = "Pairings cannot exceed 200 characters")]
        public string? Pairings { get; set; }

        // Download tracking properties
        [Display(Name = "Times Downloaded")]
        public int DownloadCount { get; set; } = 0;

        [Display(Name = "Last Downloaded")]
        public DateTime? LastDownloaded { get; set; }

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        [NotMapped]
        public double AverageRating => Ratings.Any() ? Ratings.Average(r => r.Value) : 0;

        [NotMapped]
        public int RatingCount => Ratings.Count;
        // Helper method for download content
        public string FormatForDownload()
        {
            return $"{RecipeName}\n\n" +
                   $"Type: {SideDishCategory}\n" +
                   $"Prep Time: {PrepTime} mins\n" +
                   $"Cook Time: {CookTime} mins\n" +
                   $"Servings: {Servings}\n" +
                   $"Pairs Well With: {Pairings}\n\n" +
                   $"INGREDIENTS:\n{Ingredients}\n\n" +
                   $"INSTRUCTIONS:\n{Instructions}";
        }
    }
}