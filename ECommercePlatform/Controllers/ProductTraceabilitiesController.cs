using System; // Import core system base classes
using System.Collections.Generic; // Import generic collection support
using System.Linq; // Import language-integrated query functionality
using System.Threading.Tasks; // Import asynchronous programming support
using Microsoft.AspNetCore.Mvc; // Import ASP.NET Core MVC framework components
using Microsoft.AspNetCore.Mvc.Rendering; // Import helpers for rendering UI elements like dropdowns
using Microsoft.EntityFrameworkCore; // Import Entity Framework Core for database operations
using ECommercePlatform.Data; // Import the application's database context
using ECommercePlatform.Models; // Import the domain data models
using Microsoft.AspNetCore.Authorization; // Import security and role-based access control

namespace ECommercePlatform.Controllers // Define the namespace for the controllers
{ // Start of namespace
    public class ProductTraceabilitiesController : Controller // Define the controller for managing product origin and batch data
    { // Start of class
        private readonly ApplicationDbContext _context; // Declare private field for the database context

        public ProductTraceabilitiesController(ApplicationDbContext context) // Constructor with dependency injection for the DB context
        { // Start of constructor
            _context = context; // Assign the injected context to the private field
        } // End of constructor

        // GET: ProductTraceabilities
        public async Task<IActionResult> Index() // Action to list all traceability records
        { // Start of Index method
            var applicationDbContext = _context.ProductTraceability.Include(p => p.Products); // Query traceability data and include related Product info
            return View(await applicationDbContext.ToListAsync()); // Execute query and return the list view
        } // End of Index method

        // GET: ProductTraceabilities/Details/5
        public async Task<IActionResult> Details(int? id) // Action to view details of a specific traceability entry
        { // Start of Details method
            if (id == null) // Check if the ID parameter is missing
            { // Start of null check
                return NotFound(); // Return 404 if ID is null
            } // End of null check

            var productTraceability = await _context.ProductTraceability // Query the database for the specific record
                .Include(p => p.Products) // Include the related product navigation property
                .FirstOrDefaultAsync(m => m.ProductTraceabilityId == id); // Locate the record by its primary key
            if (productTraceability == null) // Check if the record exists
            { // Start of existence check
                return NotFound(); // Return 404 if no record matches the ID
            } // End of existence check

            return View(productTraceability); // Return the record details to the view
        } // End of Details method

        [Authorize(Roles = "Admin, Supplier")] // Restrict access to Admin and Supplier roles only
        // GET: ProductTraceabilities/Create
        public IActionResult Create() // Action to show the creation form
        { // Start of Create GET
            ViewData["ProductsId"] = new SelectList(_context.Set<Products>(), "ProductsId", "ProductsId"); // Provide product IDs for the dropdown
            return View(); // Return the empty creation view
        } // End of Create GET

        // POST: ProductTraceabilities/Create
        [Authorize(Roles = "Admin, Supplier")] // Restrict processing to Admin and Supplier roles
        [HttpPost] // Restrict method to HTTP POST requests
        [ValidateAntiForgeryToken] // Security check to prevent CSRF attacks
        public async Task<IActionResult> Create([Bind("ProductTraceabilityId,ProductsId,Origin,BatchNumber,HarvestDate,Certifications")] ProductTraceability productTraceability) // Process record creation
        { // Start of Create POST
            if (ModelState.IsValid) // Verify that the submitted data meets model requirements
            { // Start of validation check
                _context.Add(productTraceability); // Add the new entity to the context
                await _context.SaveChangesAsync(); // Commit the new record to the database
                return RedirectToAction(nameof(Index)); // Redirect back to the index list
            } // End of validation check
            ViewData["ProductsId"] = new SelectList(_context.Set<Products>(), "ProductsId", "ProductsId", productTraceability.ProductsId); // Re-populate dropdown on failure
            return View(productTraceability); // Return the view with the current data and errors
        } // End of Create POST

        // GET: ProductTraceabilities/Edit/5
        public async Task<IActionResult> Edit(int? id) // Action to show the edit form for an existing record
        { // Start of Edit GET
            if (id == null) // Check if ID is provided
            { // Start of null check
                return NotFound(); // Return 404 if ID is missing
            } // End of null check

            var productTraceability = await _context.ProductTraceability.FindAsync(id); // Find the record by ID in the database
            if (productTraceability == null) // Check if the record exists
            { // Start of existence check
                return NotFound(); // Return 404 if record is missing
            } // End of existence check
            ViewData["ProductsId"] = new SelectList(_context.Set<Products>(), "ProductsId", "ProductsId", productTraceability.ProductsId); // Populate dropdown with current product selected
            return View(productTraceability); // Return the edit view with the record data
        } // End of Edit GET

        // POST: ProductTraceabilities/Edit/5
        [HttpPost] // Restrict method to HTTP POST requests
        [ValidateAntiForgeryToken] // Security check for form submission
        public async Task<IActionResult> Edit(int id, [Bind("ProductTraceabilityId,ProductsId,Origin,BatchNumber,HarvestDate,Certifications")] ProductTraceability productTraceability) // Process record updates
        { // Start of Edit POST
            if (id != productTraceability.ProductTraceabilityId) // Ensure the URL ID matches the data ID
            { // Start of ID mismatch check
                return NotFound(); // Return 404 if IDs do not match
            } // End of ID mismatch check

            if (ModelState.IsValid) // Check if the updated data is valid
            { // Start of validation block
                try // Begin block to handle database update exceptions
                { // Start try block
                    _context.Update(productTraceability); // Mark the record as modified
                    await _context.SaveChangesAsync(); // Save the updates to the database
                } // End try block
                catch (DbUpdateConcurrencyException) // Catch errors if record was modified elsewhere
                { // Start catch block
                    if (!ProductTraceabilityExists(productTraceability.ProductTraceabilityId)) // Check if record still exists
                    { // Start existence check
                        return NotFound(); // Return 404 if record was deleted
                    } // End existence check
                    else // If record exists but update failed
                    { // Start else block
                        throw; // Rethrow the exception for the global handler
                    } // End else block
                } // End catch block
                return RedirectToAction(nameof(Index)); // Redirect to index on success
            } // End of validation block
            ViewData["ProductsId"] = new SelectList(_context.Set<Products>(), "ProductsId", "ProductsId", productTraceability.ProductsId); // Re-populate dropdown on error
            return View(productTraceability); // Return view with errors
        } // End of Edit POST

        // GET: ProductTraceabilities/Delete/5
        public async Task<IActionResult> Delete(int? id) // Action to show the delete confirmation page
        { // Start of Delete GET
            if (id == null) // Check if ID is provided
            { // Start check
                return NotFound(); // Return 404 if ID missing
            } // End check

            var productTraceability = await _context.ProductTraceability // Query the database
                .Include(p => p.Products) // Include related product details
                .FirstOrDefaultAsync(m => m.ProductTraceabilityId == id); // Locate the record to delete
            if (productTraceability == null) // Check if record exists
            { // Start existence check
                return NotFound(); // Return 404 if missing
            } // End existence check

            return View(productTraceability); // Show the deletion confirmation view
        } // End of Delete GET

        // POST: ProductTraceabilities/Delete/5
        [HttpPost, ActionName("Delete")] // Map POST request to the "Delete" action name
        [ValidateAntiForgeryToken] // Security token validation
        public async Task<IActionResult> DeleteConfirmed(int id) // Action to perform actual deletion
        { // Start of Delete POST
            var productTraceability = await _context.ProductTraceability.FindAsync(id); // Locate the record in the database
            if (productTraceability != null) // If record exists
            { // Start found check
                _context.ProductTraceability.Remove(productTraceability); // Mark the record for removal
            } // End found check

            await _context.SaveChangesAsync(); // Commit the deletion to the database
            return RedirectToAction(nameof(Index)); // Redirect back to the index list
        } // End of Delete POST

        private bool ProductTraceabilityExists(int id) // Helper method to verify record existence
        { // Start of helper method
            return _context.ProductTraceability.Any(e => e.ProductTraceabilityId == id); // Return true if any record matches the ID
        } // End of helper method
    } // End of class
} // End of namespace