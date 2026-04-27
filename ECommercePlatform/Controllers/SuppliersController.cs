using System; // Imports base system functionality
using System.Collections.Generic; // Imports generic collection types
using System.Linq; // Imports LINQ for data querying
using System.Threading.Tasks; // Imports support for asynchronous tasks
using Microsoft.AspNetCore.Mvc; // Imports core MVC framework functionality
using Microsoft.AspNetCore.Mvc.Rendering; // Imports helpers for UI components
using Microsoft.EntityFrameworkCore; // Imports Entity Framework Core for ORM operations
using ECommercePlatform.Data; // Imports the application database context
using ECommercePlatform.Models; // Imports the domain data models
using Microsoft.AspNetCore.Authorization; // Imports security and authorization attributes

namespace ECommercePlatform.Controllers // Defines the namespace for the controller
{ // Start of namespace
    public class SuppliersController : Controller // Defines the controller for managing supplier profiles
    { // Start of class
        private readonly ApplicationDbContext _context; // Declares private field for database access

        public SuppliersController(ApplicationDbContext context) // Constructor with dependency injection
        { // Start of constructor
            _context = context; // Assigns the injected context to the private field
        } // End of constructor

        // GET: Suppliers
        // Action method to display a list of suppliers with an optional search filter
        public async Task<IActionResult> Index(string searchString)
        {
            // Initializes the query against the Suppliers table as IQueryable for deferred execution
            var suppliers = _context.Suppliers.AsQueryable();

            // Checks if a valid search string has been provided by the user
            if (!string.IsNullOrEmpty(searchString))
            {
                // Filters the supplier list to only include names containing the search term
                suppliers = suppliers.Where(s =>
                    s.SupplierName.Contains(searchString));
            }

            // Asynchronously executes the query, converts the results to a list, and returns the view
            return View(await suppliers.ToListAsync());
        }
        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id) // Action to view details of a specific supplier
        { // Start of Details method
            if (id == null) // Checks if the ID parameter is missing
            { // Start of null check
                return NotFound(); // Returns 404 if ID is null
            } // End of null check

            var suppliers = await _context.Suppliers // Queries the database
                .FirstOrDefaultAsync(m => m.SuppliersId == id); // Finds the specific supplier by ID
            if (suppliers == null) // Checks if the record exists
            { // Start existence check
                return NotFound(); // Returns 404 if record is missing
            } // End existence check

            return View(suppliers); // Returns the details view with the record
        } // End of Details method

        // GET: Suppliers/Create
        public IActionResult Create() // Action to display the new supplier form
        { // Start of Create GET
            return View(); // Returns the empty creation view
        } // End of Create GET

        // POST: Suppliers/Create
        [HttpPost] // Restricts method to POST requests
        [ValidateAntiForgeryToken] // Security check for form submission
        public async Task<IActionResult> Create([Bind("SuppliersId,SupplierName,SupplierEmail,SupplierInformation")] Suppliers suppliers) // Processes creation
        { // Start of Create POST
            if (ModelState.IsValid) // Checks if the submitted data passes validation
            { // Start validation check
                _context.Add(suppliers); // Marks the new entity for insertion
                await _context.SaveChangesAsync(); // Commits the record to the database
                return RedirectToAction(nameof(Index)); // Redirects to the index list
            } // End validation check
            return View(suppliers); // Returns form with errors if validation failed
        } // End of Create POST

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id) // Action to show the edit form
        { // Start of Edit GET
            if (id == null) // Validates ID input
            { // Start check
                return NotFound(); // Returns 404
            } // End check

            var suppliers = await _context.Suppliers.FindAsync(id); // Locates the record by ID
            if (suppliers == null) // Checks record existence
            { // Start check
                return NotFound(); // Returns 404
            } // End check
            return View(suppliers); // Returns the edit view populated with data
        } // End of Edit GET

        // POST: Suppliers/Edit/5
        [Authorize(Roles = "Supplier")] // Restricts updates to users in the Supplier role
        [HttpPost] // Restricts to POST requests
        [ValidateAntiForgeryToken] // Security token validation
        public async Task<IActionResult> Edit(int id, [Bind("SuppliersId,SupplierName,SupplierEmail,SupplierInformation")] Suppliers suppliers) // Processes updates
        { // Start of Edit POST
            if (id != suppliers.SuppliersId) // Verifies ID consistency
            { // Start mismatch check
                return NotFound(); // Returns 404
            } // End mismatch check

            if (ModelState.IsValid) // Checks data integrity
            { // Start validation
                try // Handles database update exceptions
                { // Start try
                    _context.Update(suppliers); // Marks record as modified
                    await _context.SaveChangesAsync(); // Commits changes to the database
                } // End try
                catch (DbUpdateConcurrencyException) // Catches conflicts if record was modified elsewhere
                { // Start catch
                    if (!SuppliersExists(suppliers.SuppliersId)) // Checks if record still exists
                    { // Start check
                        return NotFound(); // Returns 404
                    } // End check
                    else // If record exists but update failed
                    { // Start else
                        throw; // Rethrows the exception
                    } // End else
                } // End catch
                return RedirectToAction(nameof(Index)); // Redirects to list on success
            } // End validation
            return View(suppliers); // Returns view with errors if invalid
        } // End of Edit POST

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id) // Action to show delete confirmation
        { // Start of Delete GET
            if (id == null) // Validates ID
            { // Start check
                return NotFound(); // Returns 404
            } // End check

            var suppliers = await _context.Suppliers // Queries database
                .FirstOrDefaultAsync(m => m.SuppliersId == id); // Finds specific supplier
            if (suppliers == null) // Existence check
            { // Start check
                return NotFound(); // Returns 404
            } // End check

            return View(suppliers); // Returns the delete confirmation view
        } // End of Delete GET

        // POST: Suppliers/Delete/5
        [Authorize(Roles = "Supplier")] // Only Suppliers can delete records
        [HttpPost, ActionName("Delete")] // Maps POST request to logical Delete action
        [ValidateAntiForgeryToken] // Security token check
        public async Task<IActionResult> DeleteConfirmed(int id) // Performs actual deletion
        { // Start of Delete POST
            var suppliers = await _context.Suppliers.FindAsync(id); // Locates the record
            if (suppliers != null) // If found
            { // Start check
                _context.Suppliers.Remove(suppliers); // Marks for deletion
            } // End check

            await _context.SaveChangesAsync(); // Commits deletion to the database
            return RedirectToAction(nameof(Index)); // Returns to the list
        } // End of Delete POST

        private bool SuppliersExists(int id) // Private helper for existence verification
        { // Start method
            return _context.Suppliers.Any(e => e.SuppliersId == id); // Returns true if ID exists in database
        } // End method
    } // End of class
} // End of namespace