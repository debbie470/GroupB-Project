using ECommercePlatform.Data; // Data access namespace for database context
using ECommercePlatform.Models; // Namespace for application domain models
using Microsoft.AspNetCore.Authorization; // Security and role-based access control
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC framework components
using Microsoft.AspNetCore.Mvc.Rendering; // Helpers for UI components like dropdowns
using Microsoft.EntityFrameworkCore; // Entity Framework Core for ORM functionality
using System; // Base system types
using System.Collections.Generic; // Generic collection types
using System.Linq; // LINQ for data querying
using System.Security.Claims; // Claims-based identity management
using System.Threading.Tasks; // Asynchronous programming support

namespace ECommercePlatform.Controllers // Controller organization namespace
{ // Start of namespace
    public class ProductsController : Controller // Controller class for product management
    { // Start of class
        private readonly ApplicationDbContext _context; // Database context field

        public ProductsController(ApplicationDbContext context) // Constructor with dependency injection
        { // Start of constructor
            _context = context; // Initialize database context
        } // End of constructor

        // GET: Products
        // Action method to list products with optional keyword searching
        public async Task<IActionResult> Index(string searchString)
        {
            // Check if the current user is logged in as a Supplier
            if (User.IsInRole("Supplier"))
            {
                // Retrieve the unique ID of the authenticated user
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Return 401 if the user identity cannot be determined
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Fetch the supplier profile associated with the current user
                var supplier = await _context.Suppliers
                    .FirstOrDefaultAsync(s => s.UserId == userId);

                // Return 404 if the user is a Supplier but has no profile record
                if (supplier == null)
                {
                    return NotFound();
                }

                // Initialize a query filtered to only show this supplier's products
                var supplierProducts = _context.Products
                    .Where(p => p.SuppliersId == supplier.SuppliersId);

                // Apply a name-based filter if a search keyword was provided
                if (!string.IsNullOrEmpty(searchString))
                {
                    supplierProducts = supplierProducts
                        .Where(p => p.ProductName.Contains(searchString));
                }

                // Execute the query, including supplier details, and convert to a list
                var result = await supplierProducts
                    .Include(p => p.Suppliers)
                    .ToListAsync();

                // Return the filtered list to the view
                return View(result);
            }
            // Logic for non-supplier users (Admins or Customers)
            else
            {
                // Initialize a query for all products in the system
                var products = _context.Products.AsQueryable();

                // Apply a name-based filter if a search keyword was provided
                if (!string.IsNullOrEmpty(searchString))
                {
                    products = products
                        .Where(p => p.ProductName.Contains(searchString));
                }

                // Execute the query, including supplier details, and convert to a list
                var result = await products
                    .Include(p => p.Suppliers)
                    .ToListAsync();

                // Return the full or searched list to the view
                return View(result);
            }
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id) // Show specific product info
        { // Start of Details method
            if (id == null) // Validate ID input
            { // Start check
                return NotFound(); // Return 404
            } // End check

            var products = await _context.Products // Query products
                .Include(p => p.Suppliers) // Include supplier information
                .FirstOrDefaultAsync(m => m.ProductsId == id); // Find matching product
            if (products == null) // Check existence
            { // Start check
                return NotFound(); // Return 404
            } // End check

            return View(products); // Return product details view
        } // End of Details method

        // GET: Products/Create
        public IActionResult Create() // Show create product form
        { // Start method

            return View(); // Return empty form view
        } // End method

        // POST: Products/Create
        [HttpPost] // Restrict to HTTP POST
        [ValidateAntiForgeryToken] // Security token check
        public async Task<IActionResult> Create([Bind("ProductsId,ProductName,Stock,Price,ImagePath")] Products products) // Handle product creation
        { // Start of Create POST
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Identify current user
            if (userId == null) // Authentication check
            { // Start check
                return Unauthorized(); // Return 401
            } // End check

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.UserId == userId); // Get supplier profile
            if (supplier == null) // Profile existence check
            { // Start check
                return NotFound(); // Return 404
            } // End check
            products.SuppliersId = supplier.SuppliersId; // Assign ownership to supplier
            ModelState.Remove("SupplierId"); // Clear validation for manual input field

            if (ModelState.IsValid) // Check data validity
            { // Start validation
                _context.Add(products); // Mark for insertion
                await _context.SaveChangesAsync(); // Commit to database

                return RedirectToAction(nameof(Index)); // Redirect to product list
            } // End validation
            ViewData["SuppliersId"] = new SelectList(_context.Suppliers, "SuppliersId", "SuppliersId", products.SuppliersId); // Repopulate list on error
            return View(products); // Return form with errors
        } // End of Create POST

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id) // Show edit form
        { // Start of Edit GET
            if (id == null) // ID validation
            { // Start check
                return NotFound(); // Return 404
            } // End check

            var products = await _context.Products.FindAsync(id); // Find existing record
            if (products == null) // Check existence
            { // Start check
                return NotFound(); // Return 404
            } // End check
            ViewData["SuppliersId"] = new SelectList(_context.Suppliers, "SuppliersId", "SuppliersId", products.SuppliersId); // Populate supplier dropdown
            return View(products); // Return populated edit view
        } // End of Edit GET

        // POST: Products/Edit/5
        [Authorize(Roles = "Supplier")] // Restrict to Supplier role
        [HttpPost] // Restrict to POST
        [ValidateAntiForgeryToken] // Security token validation
        public async Task<IActionResult> Edit(int id, [Bind("ProductsId,SuppliersId,ProductName,Stock,Price,ImagePath")] Products products) // Update product
        { // Start of Edit POST
            if (id != products.ProductsId) // Verify ID consistency
            { // Start check
                return NotFound(); // Return 404
            } // End check

            if (ModelState.IsValid) // Data validation
            { // Start validation
                try // Handle update exceptions
                { // Start try
                    _context.Update(products); // Mark modified
                    await _context.SaveChangesAsync(); // Commit changes
                } // End try
                catch (DbUpdateConcurrencyException) // Catch concurrency errors
                { // Start catch
                    if (!ProductsExists(products.ProductsId)) // Check if deleted
                    { // Start check
                        return NotFound(); // Return 404
                    } // End check
                    else // If update failed for other reasons
                    { // Start else
                        throw; // Rethrow exception
                    } // End else
                } // End catch
                return RedirectToAction(nameof(Index)); // Return to list
            } // End validation
            ViewData["SuppliersId"] = new SelectList(_context.Suppliers, "SuppliersId", "SuppliersId", products.SuppliersId); // Reset dropdown on error
            return View(products); // Return form with errors
        } // End of Edit POST

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id) // Show delete confirmation
        { // Start of Delete GET
            if (id == null) // ID check
            { // Start check
                return NotFound(); // Return 404
            } // End check

            var products = await _context.Products // Query products
                .Include(p => p.Suppliers) // Include owner info
                .FirstOrDefaultAsync(m => m.ProductsId == id); // Find specific product
            if (products == null) // Existence check
            { // Start check
                return NotFound(); // Return 404
            } // End check

            return View(products); // Return confirmation view
        } // End of Delete GET

        // POST: Products/Delete/5
        [Authorize(Roles = "Supplier")] // Only suppliers can delete
        [HttpPost, ActionName("Delete")] // Map to delete action
        [ValidateAntiForgeryToken] // Security check
        public async Task<IActionResult> DeleteConfirmed(int id) // Execute deletion
        { // Start of Delete POST
            var products = await _context.Products.FindAsync(id); // Locate product
            if (products != null) // If found
            { // Start check
                _context.Products.Remove(products); // Mark for removal
            } // End check

            await _context.SaveChangesAsync(); // Commit deletion
            return RedirectToAction(nameof(Index)); // Redirect to list
        } // End of Delete POST

        private bool ProductsExists(int id) // Private helper
        { // Start method
            return _context.Products.Any(e => e.ProductsId == id); // Check if ID exists in database
        } // End method
    } // End of class
} // End of namespace