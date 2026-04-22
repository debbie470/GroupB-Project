using System; // Imports fundamental system classes and base types
using System.Collections.Generic; // Imports generic collection types like List and Dictionary
using System.Linq; // Imports Language-Integrated Query for data manipulation
using System.Threading.Tasks; // Imports types for asynchronous programming
using Microsoft.AspNetCore.Mvc; // Imports core MVC framework classes
using Microsoft.AspNetCore.Mvc.Rendering; // Imports helpers for rendering UI elements like SelectLists
using Microsoft.EntityFrameworkCore; // Imports Entity Framework Core for database operations
using ECommercePlatform.Data; // Imports the application database context
using ECommercePlatform.Models; // Imports the domain data models
using Microsoft.AspNetCore.Authorization; // Imports security and role-based authorization attributes

namespace ECommercePlatform.Controllers // Defines the organizational scope for the controller
{ // Start of namespace block
    public class OrderProductsController : Controller // Defines the controller class for managing order-product relations
    { // Start of class block
        private readonly ApplicationDbContext _context; // Declares a private database context field

        public OrderProductsController(ApplicationDbContext context) // Constructor to inject the database context
        { // Start of constructor block
            _context = context; // Assigns the injected context to the private field
        } // End of constructor block

        // GET: OrderProducts
        public async Task<IActionResult> Index() // Action to list all products associated with orders
        { // Start of Index block
            var applicationDbContext = _context.OrderProducts.Include(o => o.Orders).Include(o => o.Products); // Loads related Order and Product data
            return View(await applicationDbContext.ToListAsync()); // Executes the query and returns the data to the view
        } // End of Index block

        // GET: OrderProducts/Details/5
        public async Task<IActionResult> Details(int? id) // Action to show details for a specific order-product entry
        { // Start of Details block
            if (id == null) // Checks if the provided ID is null
            { // Start of null check block
                return NotFound(); // Returns 404 error if ID is missing
            } // End of null check block

            var orderProducts = await _context.OrderProducts // Queries the OrderProducts table
                .Include(o => o.Orders) // Includes related Order entity
                .Include(o => o.Products) // Includes related Product entity
                .FirstOrDefaultAsync(m => m.OrderProductsId == id); // Finds the specific record matching the ID
            if (orderProducts == null) // Checks if the record was not found
            { // Start of record check block
                return NotFound(); // Returns 404 error if record doesn't exist
            } // End of record check block

            return View(orderProducts); // Returns the view with the found record
        } // End of Details block

        // GET: OrderProducts/Create
        public IActionResult Create() // Action to display the creation form
        { // Start of Create GET block
            ViewData["OrdersId"] = new SelectList(_context.Orders, "OrdersId", "OrdersId"); // Populates Order ID dropdown
            ViewData["ProductsId"] = new SelectList(_context.Products, "ProductsId", "ProductsId"); // Populates Product ID dropdown
            return View(); // Returns the empty creation view
        } // End of Create GET block

        // POST: OrderProducts/Create
        [HttpPost] // Restricts to HTTP POST requests
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public async Task<IActionResult> Create([Bind("OrderProductsId,ProductsId,OrdersId,Quantity")] OrderProducts orderProducts) // Handles record creation
        { // Start of Create POST block
            if (ModelState.IsValid) // Checks if submitted data passes validation
            { // Start of validation block
                _context.Add(orderProducts); // Tracks the new entity for insertion
                await _context.SaveChangesAsync(); // Commits changes to the database
                return RedirectToAction(nameof(Index)); // Redirects to the index page
            } // End of validation block
            ViewData["OrdersId"] = new SelectList(_context.Orders, "OrdersId", "OrdersId", orderProducts.OrdersId); // Re-populates Order dropdown on failure
            ViewData["ProductsId"] = new SelectList(_context.Products, "ProductsId", "ProductsId", orderProducts.ProductsId); // Re-populates Product dropdown on failure
            return View(orderProducts); // Returns form with current data and errors
        } // End of Create POST block

        // GET: OrderProducts/Edit/5
        public async Task<IActionResult> Edit(int? id) // Action to display the edit form
        { // Start of Edit GET block
            if (id == null) // Checks if ID is null
            { // Start of null check block
                return NotFound(); // Returns 404 error
            } // End of null check block

            var orderProducts = await _context.OrderProducts.FindAsync(id); // Searches for record by primary key
            if (orderProducts == null) // Checks if record was found
            { // Start of record check block
                return NotFound(); // Returns 404 error
            } // End of record check block
            ViewData["OrdersId"] = new SelectList(_context.Orders, "OrdersId", "OrdersId", orderProducts.OrdersId); // Pre-selects current Order in dropdown
            ViewData["ProductsId"] = new SelectList(_context.Products, "ProductsId", "ProductsId", orderProducts.ProductsId); // Pre-selects current Product in dropdown
            return View(orderProducts); // Returns the edit view
        } // End of Edit GET block

        // POST: OrderProducts/Edit/5
        [Authorize(Roles = "Supplier")] // Restricts access to users in the Supplier role
        [HttpPost] // Restricts to HTTP POST requests
        [ValidateAntiForgeryToken] // Security token validation
        public async Task<IActionResult> Edit(int id, [Bind("OrderProductsId,ProductsId,OrdersId,Quantity")] OrderProducts orderProducts) // Handles record updates
        { // Start of Edit POST block
            if (id != orderProducts.OrderProductsId) // Verifies URL ID matches model ID
            { // Start of ID mismatch block
                return NotFound(); // Returns 404 error if IDs mismatch
            } // End of ID mismatch block

            if (ModelState.IsValid) // Validates data integrity
            { // Start of validation block
                try // Begins exception handling for concurrency
                { // Start try block
                    _context.Update(orderProducts); // Marks record as modified
                    await _context.SaveChangesAsync(); // Saves updates to database
                } // End try block
                catch (DbUpdateConcurrencyException) // Catches simultaneous update conflicts
                { // Start catch block
                    if (!OrderProductsExists(orderProducts.OrderProductsId)) // Checks if record was deleted by someone else
                    { // Start existence check block
                        return NotFound(); // Returns 404 error
                    } // End existence check block
                    else // If record still exists but update failed
                    { // Start else block
                        throw; // Rethrows the concurrency exception
                    } // End else block
                } // End catch block
                return RedirectToAction(nameof(Index)); // Redirects to index on success
            } // End of validation block
            ViewData["OrdersId"] = new SelectList(_context.Orders, "OrdersId", "OrdersId", orderProducts.OrdersId); // Re-populates Order dropdown on failure
            ViewData["ProductsId"] = new SelectList(_context.Products, "ProductsId", "ProductsId", orderProducts.ProductsId); // Re-populates Product dropdown on failure
            return View(orderProducts); // Returns view with errors
        } // End of Edit POST block

        // GET: OrderProducts/Delete/5
        public async Task<IActionResult> Delete(int? id) // Action to show delete confirmation page
        { // Start of Delete GET block
            if (id == null) // Checks for null ID
            { // Start of null check
                return NotFound(); // Returns 404
            } // End of null check

            var orderProducts = await _context.OrderProducts // Queries data
                .Include(o => o.Orders) // Includes order navigation property
                .Include(o => o.Products) // Includes product navigation property
                .FirstOrDefaultAsync(m => m.OrderProductsId == id); // Finds specific record
            if (orderProducts == null) // Checks if record exists
            { // Start of record check
                return NotFound(); // Returns 404
            } // End of record check

            return View(orderProducts); // Displays confirmation view
        } // End of Delete GET block

        // POST: OrderProducts/Delete/5
        [Authorize(Roles = "Supplier")] // Only Suppliers can delete
        [HttpPost, ActionName("Delete")] // Maps POST request to Logical Delete action
        [ValidateAntiForgeryToken] // Security token check
        public async Task<IActionResult> DeleteConfirmed(int id) // Executes actual deletion
        { // Start of Delete POST block
            var orderProducts = await _context.OrderProducts.FindAsync(id); // Locates the record
            if (orderProducts != null) // If record is found
            { // Start of found block
                _context.OrderProducts.Remove(orderProducts); // Marks for deletion
            } // End of found block

            await _context.SaveChangesAsync(); // Commits deletion to database
            return RedirectToAction(nameof(Index)); // Returns to the list
        } // End of Delete POST block

        private bool OrderProductsExists(int id) // Private helper to check existence
        { // Start of helper block
            return _context.OrderProducts.Any(e => e.OrderProductsId == id); // Returns true if any record matches ID
        } // End of helper block
    } // End of class block
} // End of namespace block