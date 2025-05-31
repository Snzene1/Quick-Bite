using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quick_Bite.Data;
using Quick_Bite.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick_Bite.Controllers
{
    public class DownloadController : Controller
    {
        private readonly QuickContext _context;

        public DownloadController(QuickContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> DownloadItem(int? recipeId, int? drinkId, int? sideDishId)
        {
            try
            {
                // Validate only one item is selected
                var itemCount = new[] { recipeId, drinkId, sideDishId }.Count(id => id != null);
                if (itemCount != 1)
                {
                    return BadRequest("Please select exactly one item to download");
                }

                if (recipeId.HasValue)
                {
                    var recipe = await _context.Recipes.FindAsync(recipeId);
                    if (recipe == null) return NotFound();

                    // Update download tracking
                    recipe.DownloadCount++;
                    recipe.LastDownloaded = DateTime.UtcNow;
                    await RecordDownload(recipeId: recipeId);
                    await _context.SaveChangesAsync();

                    return GenerateDownloadFile(recipe.RecipeName, recipe.FormatForDownload());
                }
                else if (drinkId.HasValue)
                {
                    var drink = await _context.Drinks.FindAsync(drinkId);
                    if (drink == null) return NotFound();

                    // Update download tracking
                    drink.DownloadCount++;
                    drink.LastDownloaded = DateTime.UtcNow;
                    await RecordDownload(drinkId: drinkId);
                    await _context.SaveChangesAsync();

                    return GenerateDownloadFile(drink.RecipeName, drink.FormatForDownload());
                }
                else if (sideDishId.HasValue)
                {
                    var sideDish = await _context.SideDishes.FindAsync(sideDishId);
                    if (sideDish == null) return NotFound();

                    // Update download tracking
                    sideDish.DownloadCount++;
                    sideDish.LastDownloaded = DateTime.UtcNow;
                    await RecordDownload(sideDishId: sideDishId);
                    await _context.SaveChangesAsync();

                    return GenerateDownloadFile(sideDish.RecipeName, sideDish.FormatForDownload());
                }

                return BadRequest("No valid item selected");
            }
            catch (Exception ex)
            {
                // Log the error (consider using ILogger)
                return StatusCode(500, "An error occurred while processing your download");
            }
        }

        private async Task RecordDownload(int? recipeId = null, int? drinkId = null, int? sideDishId = null)
        {
            var download = new Download
            {
                DownloadDate = DateTime.UtcNow,
                RecipeId = recipeId,
                DrinkId = drinkId,
                SideDishId = sideDishId
            };

            _context.Downloads.Add(download);
            await _context.SaveChangesAsync();
        }

        private IActionResult GenerateDownloadFile(string fileName, string content)
        {
            // Clean the filename
            var cleanFileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

            // Generate a text file
            var bytes = Encoding.UTF8.GetBytes(content);
            return File(bytes, "text/plain", $"{cleanFileName}.txt");
        }

        // GET: Download/History
        public async Task<IActionResult> History()
        {
            var downloads = await _context.Downloads
                .Include(d => d.Recipe)
                .Include(d => d.Drink)
                .Include(d => d.SideDish)
                .OrderByDescending(d => d.DownloadDate)
                .Take(50) // Show only recent 50 downloads
                .ToListAsync();

            return View(downloads);
        }
    }
}