using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceCore.Data;
using ECommerceCore.Models;

namespace ECommerceCore.Controllers
{
    public class LoyaltyRewardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoyaltyRewardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoyaltyRewards
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoyaltyRewards.ToListAsync());
        }

        // GET: LoyaltyRewards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loyaltyRewards = await _context.LoyaltyRewards
                .FirstOrDefaultAsync(m => m.LoyaltyRewardsId == id);
            if (loyaltyRewards == null)
            {
                return NotFound();
            }

            return View(loyaltyRewards);
        }

        // GET: LoyaltyRewards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoyaltyRewards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoyaltyRewardsId,UserId,PointsBalance,TierLevel,History")] LoyaltyRewards loyaltyRewards)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loyaltyRewards);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loyaltyRewards);
        }

        // GET: LoyaltyRewards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loyaltyRewards = await _context.LoyaltyRewards.FindAsync(id);
            if (loyaltyRewards == null)
            {
                return NotFound();
            }
            return View(loyaltyRewards);
        }

        // POST: LoyaltyRewards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoyaltyRewardsId,UserId,PointsBalance,TierLevel,History")] LoyaltyRewards loyaltyRewards)
        {
            if (id != loyaltyRewards.LoyaltyRewardsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loyaltyRewards);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoyaltyRewardsExists(loyaltyRewards.LoyaltyRewardsId))
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
            return View(loyaltyRewards);
        }

        // GET: LoyaltyRewards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loyaltyRewards = await _context.LoyaltyRewards
                .FirstOrDefaultAsync(m => m.LoyaltyRewardsId == id);
            if (loyaltyRewards == null)
            {
                return NotFound();
            }

            return View(loyaltyRewards);
        }

        // POST: LoyaltyRewards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loyaltyRewards = await _context.LoyaltyRewards.FindAsync(id);
            if (loyaltyRewards != null)
            {
                _context.LoyaltyRewards.Remove(loyaltyRewards);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoyaltyRewardsExists(int id)
        {
            return _context.LoyaltyRewards.Any(e => e.LoyaltyRewardsId == id);
        }
    }
}
