using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quick_Bite.Data;
using Quick_Bite.Models;
using static Quick_Bite.Models.Recipe;
using Microsoft.AspNetCore.Hosting; // Add this for IWebHostEnvironment
using System.IO;
using Microsoft.AspNetCore.Authorization; // Add this for Path operations

namespace Quick_Bite.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly QuickContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RecipeController(QuickContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Recipe with search and filters
        public async Task<IActionResult> Index(string searchString, MealType? mealType, Category? dietaryCategory)
        {
            var recipes = _context.Recipes.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                recipes = recipes.Where(r => r.RecipeName.Contains(searchString));

            if (mealType.HasValue)
                recipes = recipes.Where(r => r.MealCategory == mealType.Value);

            if (dietaryCategory.HasValue)
                recipes = recipes.Where(r => r.DietaryCategory == dietaryCategory.Value);

            ViewBag.MealTypes = Enum.GetValues<MealType>();
            ViewBag.DietaryCategories = Enum.GetValues<Category>();

            return View(await recipes.ToListAsync());
        }

        // GET: Recipe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(m => m.Id == id);

            return recipe == null ? NotFound() : View(recipe);
        }

        // GET: Recipe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recipe recipe, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Validate image file
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(imageFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("imageFile", "Only image files are allowed (jpg, jpeg, png, gif)");
                        return View(recipe);
                    }

                    if (imageFile.Length > 5 * 1024 * 1024) // 5MB limit
                    {
                        ModelState.AddModelError("imageFile", "Image cannot be larger than 5MB");
                        return View(recipe);
                    }

                    // Create recipes directory if it doesn't exist
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/recipes");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate unique filename
                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Save file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    recipe.ImageUrl = fileName;
                }

                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var recipe = await _context.Recipes.FindAsync(id);
            return recipe == null ? NotFound() : View(recipe);
        }

        // POST: Recipe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing recipe from the database
                    var existingRecipe = await _context.Recipes.FindAsync(id);
                    if (existingRecipe == null)
                    {
                        return NotFound();
                    }

                    // Update the properties manually
                    existingRecipe.RecipeName = recipe.RecipeName;
                    existingRecipe.Ingredients = recipe.Ingredients;
                    existingRecipe.Instructions = recipe.Instructions;
                    existingRecipe.MealCategory = recipe.MealCategory;
                    existingRecipe.DietaryCategory = recipe.DietaryCategory;
                    existingRecipe.PrepTime = recipe.PrepTime;
                    existingRecipe.CookTime = recipe.CookTime;
                    existingRecipe.Servings = recipe.Servings;
                    existingRecipe.DateAdded = recipe.DateAdded;

                    // Mark the entity as modified
                    _context.Update(existingRecipe);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(recipe);
        }
       private bool RecipeExists(int id) => _context.Recipes.Any(e => e.Id == id);
    }
}
