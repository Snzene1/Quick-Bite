using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quick_Bite.Data;
using Quick_Bite.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Quick_Bite.Controllers
{
    [Authorize]
    public class SideDishController : Controller
    {
        private readonly QuickContext _context;

        public SideDishController(QuickContext context)
        {
            _context = context;
        }

        // GET: SideDish with search and filters
        public async Task<IActionResult> Index(
            string searchString,
            SideDish.SideDishType? sideDishType,
            int? maxPrepTime)
        {
            var sideDishes = _context.SideDishes.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                sideDishes = sideDishes.Where(s => s.RecipeName.Contains(searchString));

            if (sideDishType.HasValue)
                sideDishes = sideDishes.Where(s => s.SideDishCategory == sideDishType.Value);

            if (maxPrepTime.HasValue)
                sideDishes = sideDishes.Where(s => s.PrepTime <= maxPrepTime.Value);

            ViewBag.SideDishTypes = Enum.GetValues<SideDish.SideDishType>();
            ViewBag.MaxPrepTime = maxPrepTime;

            return View(await sideDishes.ToListAsync());
        }

        // GET: SideDish/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sideDish = await _context.SideDishes
                .FirstOrDefaultAsync(s => s.Id == id);

            return sideDish == null ? NotFound() : View(sideDish);
        }

        // GET: SideDish/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SideDish/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SideDish sideDish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sideDish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sideDish);
        }

        // GET: SideDish/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sideDish = await _context.SideDishes.FindAsync(id);
            return sideDish == null ? NotFound() : View(sideDish);
        }

        // POST: SideDish/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SideDish sideDish)
        {
            if (id != sideDish.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sideDish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SideDishExists(sideDish.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sideDish);
        }

        // GET: SideDish/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null) return NotFound();

        //    var sideDish = await _context.SideDishes
        //        .FirstOrDefaultAsync(s => s.Id == id);

        //    return sideDish == null ? NotFound() : View(sideDish);
        //}

        //// POST: SideDish/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var sideDish = await _context.SideDishes.FindAsync(id);
        //    if (sideDish != null)
        //    {
        //        _context.SideDishes.Remove(sideDish);
        //        await _context.SaveChangesAsync();
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        private bool SideDishExists(int id) => _context.SideDishes.Any(e => e.Id == id);
    }
}