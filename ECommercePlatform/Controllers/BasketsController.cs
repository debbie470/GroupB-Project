// Project-specific namespace for data access and domain 
using ECommercePlatform.Data;
using ECommercePlatform.Models;
// ASP>NET Core MVC framework, security, and UI helper libraries
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
// Entity Framework Core for database ORM operations 
using Microsoft.EntityFrameworkCore;
// System libraries for core logic, collections, LINQ, security claims, and async tasks 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommercePlatform.Controllers
{
    // Controller to manage user shopping and pricing logic 
    public class BasketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BasketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Baskets
        //Retrives the active basket, calculate costs, and apply loyalty discounts 
        public async Task<IActionResult> Index()
        {
            // Retrive current user ID from claims 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // find an active basket or create a new one for the user 
        
            var basket = await _context.Basket
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status);
            if (basket == null)
            {
                basket = new Basket
                {
                    Status = true,
                    UserId = userId,
                    BasketCreatedAt = DateTime.UtcNow,

                };
                _context.Basket.Add(basket);
                await _context.SaveChangesAsync();
            }
            // fetch products linked to this specific basket 
            var basketProducts = await _context.BasketProducts
                .Where(x => x.BasketId == basket.BasketId)
                .Include(x => x.Basket)
                .Include(x => x.Products)
                .ToListAsync();
            // Calculate total cost before discounts 

            decimal subtotal = 0m;
            foreach (var basketProduct in basketProducts)
            {
                var productTotal = basketProduct.Products.Price * basketProduct.Quantity;
                subtotal += productTotal;
            }
            // Apply 10% discount if the user has completed 5 or more orders 
            var orderCount = await _context.Orders.CountAsync(x => x.UserId == userId);
            decimal discount = 0m;
            if (orderCount >= 5)
            {
                discount = subtotal * 0.10m;
            }
            //Final total calculation and passing data to the view 
            decimal total = subtotal - discount;
            ViewBag.Subtotal = subtotal;
            ViewBag.Discount = discount;
            ViewBag.Total = total;
            ViewBag.orderCount = orderCount;

            return View(basketProducts);

        }

        // GET: Baskets/Details/5
        // Shows metadat for a specific basket record
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Basket
                .FirstOrDefaultAsync(m => m.BasketId == id);
            if (basket == null)
            {
                return NotFound();
            }

            return View(basket);
        }

        // GET: Baskets/Create
        //Renders the form to create a new basket record 
        public IActionResult Create()
        {
            return View();
        }

        // POST: Baskets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Saves a new basket record to the database 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BasketId,Status,BasketCreatedAt,UserId")] Basket basket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(basket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(basket);
        }

        // GET: Baskets/Edit/5
        //Renders the edit form for an exsiting basket
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Basket.FindAsync(id);
            if (basket == null)
            {
                return NotFound();
            }
            return View(basket);
        }

        // POST: Baskets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Updates basket properties and handles concurrency checks 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BasketId,Status,BasketCreatedAt,UserId")] Basket basket)
        {
            if (id != basket.BasketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasketExists(basket.BasketId))
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
            return View(basket);
        }

        // GET: Baskets/Delete/5
        // Displays confirmation for basket deletation 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Basket
                .FirstOrDefaultAsync(m => m.BasketId == id);
            if (basket == null)
            {
                return NotFound();
            }

            return View(basket);
        }

        // POST: Baskets/Delete/5
        //Delets a basket record from the database 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var basket = await _context.Basket.FindAsync(id);
            if (basket != null)
            {
                _context.Basket.Remove(basket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Helper: Check if basket exists by ID 

        private bool BasketExists(int id)
        {
            return _context.Basket.Any(e => e.BasketId == id);
        }
    }
}
