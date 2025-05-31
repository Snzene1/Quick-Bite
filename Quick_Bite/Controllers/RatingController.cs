using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quick_Bite.Data;
using Quick_Bite.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class RatingController : Controller
{
    private readonly QuickContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RatingController(QuickContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // POST: Rating/RateRecipe
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RateRecipe(int recipeId, int value)
    {
        return await ProcessRating(recipeId, null, null, value);
    }

    // POST: Rating/RateDrink
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RateDrink(int drinkId, int value)
    {
        return await ProcessRating(null, drinkId, null, value);
    }

    // POST: Rating/RateSideDish
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RateSideDish(int sideDishId, int value)
    {
        return await ProcessRating(null, null, sideDishId, value);
    }

    private async Task<IActionResult> ProcessRating(int? recipeId, int? drinkId, int? sideDishId, int value)
    {
        if (value < 1 || value > 5)
        {
            return BadRequest("Rating must be between 1 and 5");
        }

        var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        var rating = new Rating
        {
            RecipeId = recipeId,
            DrinkId = drinkId,
            SideDishId = sideDishId,
            Value = value,
            IpAddress = ipAddress,
            CreatedDate = DateTime.UtcNow
        };

        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();

        if (recipeId.HasValue)
        {
            return RedirectToAction("Details", "Recipes", new { id = recipeId.Value });
        }
        else if (drinkId.HasValue)
        {
            return RedirectToAction("Details", "Drinks", new { id = drinkId.Value });
        }
        else if (sideDishId.HasValue)
        {
            return RedirectToAction("Details", "SideDishes", new { id = sideDishId.Value });
        }

        return RedirectToAction("Index", "Home");
    }

    // Optional: View to display all ratings (for admin purposes)
    //public async Task<IActionResult> Index()
    //{
    //    var ratings = await _context.Ratings
    //        .Include(r => r.Recipe)
    //        .Include(r => r.Drink)
    //        .Include(r => r.SideDish)
    //        .ToListAsync();
    //    return View(ratings);
    //}
}