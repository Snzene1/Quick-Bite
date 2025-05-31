using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quick_Bite.Data;
using Quick_Bite.Models;
using static Quick_Bite.Models.Drink;

namespace Quick_Bite.Controllers
{
    [Authorize]
    public class DrinkController : Controller
    {
        private readonly QuickContext _context;

        public DrinkController(QuickContext context)
        {
            _context = context;
        }

        // GET: Drink
        public async Task<IActionResult> Index(
      string searchString,
      DrinkType? drinkType,
      bool? isAlcoholic)
        {
            // Start with base query
            var drinks = _context.Drinks.AsQueryable();

            // Apply name filter if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                drinks = drinks.Where(d => d.RecipeName.Contains(searchString));
            }

            // Apply drink type filter if provided
            if (drinkType.HasValue)
            {
                drinks = drinks.Where(d => d.DrinkCategory == drinkType.Value);
            }

            // Apply alcoholic filter if provided
            if (isAlcoholic.HasValue)
            {
                drinks = drinks.Where(d => d.IsAlcoholic == isAlcoholic.Value);
            }

            // Pass enum values to view for dropdowns
            ViewBag.DrinkTypes = Enum.GetValues<DrinkType>();

            return View(await drinks.ToListAsync());
        }

        // GET: Drink/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drinks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drink == null)
            {
                return NotFound();
            }

            return View(drink);
        }

        // GET: Drink/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drink/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RecipeName,Ingredients,Instructions,DrinkCategory,PrepTime,Servings,IsAlcoholic,ServingTemperature")] Drink drink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(drink);
        }

        // GET: Drink/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }
            return View(drink);
        }

        // POST: Drink/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecipeName,Ingredients,Instructions,DrinkCategory,PrepTime,Servings,IsAlcoholic,ServingTemperature")] Drink drink)
        {
            if (id != drink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrinkExists(drink.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(drink);
        }

        // GET: Drink/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var drink = await _context.Drinks
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (drink == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(drink);
        //}

        //// POST: Drink/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var drink = await _context.Drinks.FindAsync(id);
        //    if (drink != null)
        //    {
        //        _context.Drinks.Remove(drink);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool DrinkExists(int id)
        {
            return _context.Drinks.Any(e => e.Id == id);
        }
    }
}
