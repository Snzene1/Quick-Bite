using Quick_Bite.Models;

// In HomeViewModel.cs
namespace Quick_Bite.Models.ViewModels  // This must match
{
    public class HomeViewModel
    {
        public List<Recipe> PopularRecipes { get; set; }
    }
}