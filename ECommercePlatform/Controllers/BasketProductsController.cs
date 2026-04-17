// Project-specific namespace for data access and domain 
using ECommercePlatform.Data;  
using ECommercePlatform.Models;
// ASP>NET Core MVC framework, security, and UI helper libraries
using Microsoft.AspNetCore.Authorization;
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
    // Manages the association between shooping baskets and products 
    public class BasketProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BasketProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BasketProducts
        // Returns a list of all products in all baskets, including related Products and Basket entities 
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BasketProducts.Include(b => b.Basket).Include(b => b.Products);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BasketProducts/Details/5
        //Retrives specific details for a single basket-product record by ID 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketProducts = await _context.BasketProducts
                .Include(b => b.Basket)
                .Include(b => b.Products)
                .FirstOrDefaultAsync(m => m.BasketProductsId == id);
            if (basketProducts == null)
            {
                return NotFound();
            }

            return View(basketProducts);
        }

        // GET: BasketProducts/Create
        //Initializes the creation view with dropdown lists for Baskets and Products 
        public IActionResult Create()
        {
            ViewData["BasketId"] = new SelectList(_context.Set<Basket>(), "BasketId", "BasketId");
            ViewData["ProductsId"] = new SelectList(_context.Products, "ProductsId", "ProductsId");
            return View();
        }

        // POST: BasketProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Logic to add a produc to a user's active basket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ProductsId)
        {
            // Verify product existence 
            var product = await _context.Products
            .FirstOrDefaultAsync(x => x.ProductsId == ProductsId);

            if (product == null)
            {
                return NotFound();
            }
            // Identify the current authenticated user 

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }
            // Fetch an existing active basket or create a new one for the user

            var basket = await _context.Basket
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == true);

            if (basket == null)
            {
                basket = new Basket
                {
                    Status = true,
                    UserId = userId,
                    BasketCreatedAt = DateTime.UtcNow
                };

                _context.Basket.Add(basket);
                await _context.SaveChangesAsync();
            }
            // Check if the product already exists in the basket to increment quantity or add new records 
            var basketProduct = await _context.BasketProducts
            .FirstOrDefaultAsync(bp => bp.BasketId == basket.BasketId
            && bp.ProductsId == ProductsId);

            if (basketProduct != null)
            {
                basketProduct.Quantity++;
            }
            else
            {
                basketProduct = new BasketProducts
                {
                    BasketId = basket.BasketId,
                    ProductsId = ProductsId,
                    Quantity = 1
                };

                _context.BasketProducts.Add(basketProduct);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Baskets");
        }


        // GET: BasketProducts/Edit/5
        // Loads the edit form for a specific basket-product entry 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketProducts = await _context.BasketProducts.FindAsync(id);
            if (basketProducts == null)
            {
                return NotFound();
            }
            ViewData["BasketId"] = new SelectList(_context.Set<Basket>(), "BasketId", "BasketId", basketProducts.BasketId);
            ViewData["ProductsId"] = new SelectList(_context.Products, "ProductsId", "ProductsId", basketProducts.ProductsId);
            return View(basketProducts);
        }

        // POST: BasketProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Updates quantity or associations; restricted to user with the "Supplier" role 
        [Authorize(Roles = "Supplier")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BasketProductsId,BasketId,ProductsId,Quantity")] BasketProducts basketProducts)
        {
            if (id != basketProducts.BasketProductsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basketProducts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasketProductsExists(basketProducts.BasketProductsId))
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
            ViewData["BasketId"] = new SelectList(_context.Set<Basket>(), "BasketId", "BasketId", basketProducts.BasketId);
            ViewData["ProductsId"] = new SelectList(_context.Products, "ProductsId", "ProductsId", basketProducts.ProductsId);
            return View(basketProducts);
        }

        // GET: BasketProducts/Delete/5
        //display confirmation page for removing a product from a basket 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketProducts = await _context.BasketProducts
                .Include(b => b.Basket)
                .Include(b => b.Products)
                .FirstOrDefaultAsync(m => m.BasketProductsId == id);
            if (basketProducts == null)
            {
                return NotFound();
            }

            return View(basketProducts);
        }

        // POST: BasketProducts/Delete/5
        //Performs the actual deletion; restricted to "supplier" role 
        [Authorize(Roles = "Supplier")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var basketProducts = await _context.BasketProducts.FindAsync(id);
            if (basketProducts != null)
            {
                _context.BasketProducts.Remove(basketProducts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // Helper method to verify if a record exists 

        private bool BasketProductsExists(int id)
        {
            return _context.BasketProducts.Any(e => e.BasketProductsId == id);
        }
    }
}
