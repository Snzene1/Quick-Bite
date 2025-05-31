using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quick_Bite.Models
{
    public class Drink
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

        public enum DrinkType
        {
            Cocktail,
            Beer,
            Wine,
            NonAlcoholic,
            HotBeverage,
            Smoothie
        }

        [Display(Name = "Drink Type")]
        public DrinkType DrinkCategory { get; set; }

        [Display(Name = "Preparation Time (mins)")]
        [Range(0, 60, ErrorMessage = "Prep time must be between 0-60 minutes")]
        public int PrepTime { get; set; }

        [Display(Name = "Servings")]
        [Range(1, 20, ErrorMessage = "Servings must be between 1-20")]
        public int Servings { get; set; }

        [Display(Name = "Alcoholic?")]
        public bool IsAlcoholic { get; set; }

        public enum Temperature
        {
            Hot,
            Cold,
            RoomTemperature
        }

        [Display(Name = "Serving Temperature")]
        public Temperature ServingTemperature { get; set; }

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
                   $"Type: {DrinkCategory}\n" +
                   $"Alcoholic: {(IsAlcoholic ? "Yes" : "No")}\n" +
                   $"Serving Temperature: {ServingTemperature}\n" +
                   $"Prep Time: {PrepTime} mins\n" +
                   $"Servings: {Servings}\n\n" +
                   $"INGREDIENTS:\n{Ingredients}\n\n" +
                   $"INSTRUCTIONS:\n{Instructions}";
        }


    }
}