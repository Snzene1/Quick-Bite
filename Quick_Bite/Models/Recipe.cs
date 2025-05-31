using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quick_Bite.Models
{
    public class Recipe
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

        public enum MealType
        {
            Breakfast,
            Brunch,
            Lunch,
            Dinner,
            Dessert,
            Snack
        }

        [Display(Name = "Meal Type")]
        public MealType MealCategory { get; set; }

        public enum Category
        {
            Vegetarian,
            Vegan,
            MeatLover,
            Dairy,
            Spicy,
            GlutenFree
        }

        [Display(Name = "Dietary Category")]
        public Category DietaryCategory { get; set; }

        [Display(Name = "Preparation Time (mins)")]
        [Range(1, 500)]
        public int PrepTime { get; set; }

        [Display(Name = "Cooking Time (mins)")]
        [Range(0, 500)]
        public int CookTime { get; set; }

        [Display(Name = "Servings")]
        [Range(1, 50)]
        public int Servings { get; set; }

        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        [Display(Name = "Times Downloaded")]
        public int DownloadCount { get; set; } = 0;  // New property for tracking downloads

        [Display(Name = "Last Downloaded")]
        public DateTime? LastDownloaded { get; set; }  // Nullable for never-downloaded recipes

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        [NotMapped]
        public double AverageRating => Ratings.Any() ? Ratings.Average(r => r.Value) : 0;

        [NotMapped]
        public int RatingCount => Ratings.Count;
        // Helper method to format for download
        public string FormatForDownload()
        {
            return $"{RecipeName}\n\n" +
                   $"Meal Type: {MealCategory}\n" +
                   $"Dietary: {DietaryCategory}\n" +
                   $"Prep Time: {PrepTime} mins\n" +
                   $"Cook Time: {CookTime} mins\n" +
                   $"Servings: {Servings}\n\n" +
                   $"INGREDIENTS:\n{Ingredients}\n\n" +
                   $"INSTRUCTIONS:\n{Instructions}";
        }
    }
}