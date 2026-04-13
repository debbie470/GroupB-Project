using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommercePlatform.Data;
using ECommercePlatform.Models;

namespace ECommercePlatform.Controllers
{
    public class ProductTraceabilitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTraceabilitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductTraceabilities
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductTraceability.Include(p => p.Products);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductTraceabilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTraceability = await _context.ProductTraceability
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.ProductTraceabilityId == id);
            if (productTraceability == null)
            {
                return NotFound();
            }

            return View(productTraceability);
        }

        // GET: ProductTraceabilities/Create
        public IActionResult Create()
        {
            ViewData["ProductsId"] = new SelectList(_context.Set<Products>(), "ProductsId", "ProductsId");
            return View();
        }

        // POST: ProductTraceabilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductTraceabilityId,ProductsId,Origin,BatchNumber,HarvestDate,Certifications")] ProductTraceability productTraceability)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productTraceability);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductsId"] = new SelectList(_context.Set<Products>(), "ProductsId", "ProductsId", productTraceability.ProductsId);
            return View(productTraceability);
        }

        // GET: ProductTraceabilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTraceability = await _context.ProductTraceability.FindAsync(id);
            if (productTraceability == null)
            {
                return NotFound();
            }
            ViewData["ProductsId"] = new SelectList(_context.Set<Products>(), "ProductsId", "ProductsId", productTraceability.ProductsId);
            return View(productTraceability);
        }

        // POST: ProductTraceabilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductTraceabilityId,ProductsId,Origin,BatchNumber,HarvestDate,Certifications")] ProductTraceability productTraceability)
        {
            if (id != productTraceability.ProductTraceabilityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productTraceability);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTraceabilityExists(productTraceability.ProductTraceabilityId))
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
            ViewData["ProductsId"] = new SelectList(_context.Set<Products>(), "ProductsId", "ProductsId", productTraceability.ProductsId);
            return View(productTraceability);
        }

        // GET: ProductTraceabilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTraceability = await _context.ProductTraceability
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.ProductTraceabilityId == id);
            if (productTraceability == null)
            {
                return NotFound();
            }

            return View(productTraceability);
        }

        // POST: ProductTraceabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productTraceability = await _context.ProductTraceability.FindAsync(id);
            if (productTraceability != null)
            {
                _context.ProductTraceability.Remove(productTraceability);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTraceabilityExists(int id)
        {
            return _context.ProductTraceability.Any(e => e.ProductTraceabilityId == id);
        }
    }
}
