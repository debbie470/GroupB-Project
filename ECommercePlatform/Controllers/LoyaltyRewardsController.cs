using System; // Import core system utilities
using System.Collections.Generic; // Import generic collection types
using System.Linq; // Import language-integrated query features
using System.Threading.Tasks; // Import asynchronous programming support
using Microsoft.AspNetCore.Mvc; // Import MVC framework components
using Microsoft.AspNetCore.Mvc.Rendering; // Import UI rendering helpers
using Microsoft.EntityFrameworkCore; // Import Entity Framework Core features
using ECommercePlatform.Data; // Import project database context
using ECommercePlatform.Models; // Import project data models
using Microsoft.AspNetCore.Authorization; // Import security and authorization features

namespace ECommercePlatform.Controllers // Define the namespace for organizational grouping
{ // Start of namespace block
    public class LoyaltyRewardsController : Controller // Define class inheriting from MVC Controller
    { // Start of class block
        private readonly ApplicationDbContext _context; // Declare private database context field

        public LoyaltyRewardsController(ApplicationDbContext context) // Constructor for dependency injection
        { // Start of constructor block
            _context = context; // Assign injected context to private field
        } // End of constructor block
        [Authorize]// you have to be a logged in user to view this page 
        public async Task<IActionResult> Index() // Method to handle the main rewards page
        { // Start of Index block
            return View(await _context.LoyaltyRewards.ToListAsync()); // Retrieve all rewards and return view
        } // End of Index block

        public async Task<IActionResult> Details(int? id) // Method to handle individual reward details
        { // Start of Details block
            if (id == null) // Check if the provided ID is null
            { // Start of null check block
                return NotFound(); // Return 404 error if ID is missing
            } // End of null check block

            var loyaltyRewards = await _context.LoyaltyRewards // Query the LoyaltyRewards table
                .FirstOrDefaultAsync(m => m.LoyaltyRewardsId == id); // Find the first match for the ID

            if (loyaltyRewards == null) // Check if no record was found
            { // Start of empty check block
                return NotFound(); // Return 404 error if record doesn't exist
            } // End of empty check block

            return View(loyaltyRewards); // Pass the found record to the Details view
        } // End of Details block

        public IActionResult Create() // Method to show the creation form
        { // Start of Create GET block
            return View(); // Return the empty creation view
        } // End of Create GET block

        [HttpPost] // Restrict method to HTTP POST requests
        [ValidateAntiForgeryToken] // Prevent Cross-Site Request Forgery (CSRF)
        public async Task<IActionResult> Create([Bind("LoyaltyRewardsId,PointsBalance,TierLevel,History")] LoyaltyRewards loyaltyRewards) // Process form submission
        { // Start of Create POST block
            if (ModelState.IsValid) // Check if submitted data meets model requirements
            { // Start of validation block
                _context.Add(loyaltyRewards); // Mark the new record for insertion
                await _context.SaveChangesAsync(); // Commit changes to the database
                return RedirectToAction(nameof(Index)); // Redirect user to the list page
            } // End of validation block
            return View(loyaltyRewards); // Return to form with errors if validation failed
        } // End of Create POST block

        public async Task<IActionResult> Edit(int? id) // Method to show the edit form
        { // Start of Edit GET block
            if (id == null) // Check if ID is provided
            { // Start of null check
                return NotFound(); // Return 404 if ID is missing
            } // End of null check

            var loyaltyRewards = await _context.LoyaltyRewards.FindAsync(id); // Find record by ID
            if (loyaltyRewards == null) // Check if record exists
            { // Start of existence check
                return NotFound(); // Return 404 if record is missing
            } // End of existence check
            return View(loyaltyRewards); // Pass record to the edit view
        } // End of Edit GET block

        [Authorize(Roles = "Supplier")] // Only allow users in the Supplier role to execute
        [HttpPost] // Restrict to HTTP POST requests
        [ValidateAntiForgeryToken] // Ensure request is legitimate
        public async Task<IActionResult> Edit(int id, [Bind("LoyaltyRewardsId,PointsBalance,TierLevel,History")] LoyaltyRewards loyaltyRewards) // Process updates
        { // Start of Edit POST block
            if (id != loyaltyRewards.LoyaltyRewardsId) // Verify ID in URL matches ID in data
            { // Start of mismatch check
                return NotFound(); // Return 404 if IDs don't match
            } // End of mismatch check

            if (ModelState.IsValid) // Check if updated data is valid
            { // Start of validation block
                try // Start error handling block for database updates
                { // Start try block
                    _context.Update(loyaltyRewards); // Mark the record as modified
                    await _context.SaveChangesAsync(); // Save updates to database
                } // End try block
                catch (DbUpdateConcurrencyException) // Catch errors if record was changed elsewhere
                { // Start catch block
                    if (!LoyaltyRewardsExists(loyaltyRewards.LoyaltyRewardsId)) // Check if record still exists
                    { // Start existence check
                        return NotFound(); // Return 404 if record was deleted
                    } // End existence check
                    else // If record exists but update failed
                    { // Start else block
                        throw; // Rethrow exception for global handler
                    } // End else block
                } // End catch block
                return RedirectToAction(nameof(Index)); // Redirect to list on success
            } // End validation block
            return View(loyaltyRewards); // Return to form with errors if invalid
        } // End of Edit POST block

        public async Task<IActionResult> Delete(int? id) // Method to show delete confirmation
        { // Start of Delete GET block
            if (id == null) // Verify ID is provided
            { // Start of null check
                return NotFound(); // Return 404 if ID is missing
            } // End of null check

            var loyaltyRewards = await _context.LoyaltyRewards // Query database
                .FirstOrDefaultAsync(m => m.LoyaltyRewardsId == id); // Find specific record
            if (loyaltyRewards == null) // Verify record existence
            { // Start of existence check
                return NotFound(); // Return 404 if record is missing
            } // End of existence check

            return View(loyaltyRewards); // Show the record on the delete confirmation page
        } // End of Delete GET block

        [Authorize(Roles = "Supplier")] // Restrict deletion to Supplier role
        [HttpPost, ActionName("Delete")] // Map this POST action to the "Delete" name
        [ValidateAntiForgeryToken] // Security check
        public async Task<IActionResult> DeleteConfirmed(int id) // Perform actual deletion
        { // Start of Delete POST block
            var loyaltyRewards = await _context.LoyaltyRewards.FindAsync(id); // Locate record in database
            if (loyaltyRewards != null) // If record is found
            { // Start of found block
                _context.LoyaltyRewards.Remove(loyaltyRewards); // Mark record for deletion
            } // End of found block

            await _context.SaveChangesAsync(); // Commit the deletion to the database
            return RedirectToAction(nameof(Index)); // Return to the rewards list
        } // End of Delete POST block

        private bool LoyaltyRewardsExists(int id) // Private helper to check record status
        { // Start of helper block
            return _context.LoyaltyRewards.Any(e => e.LoyaltyRewardsId == id); // Return true if record exists
        } // End of helper block
    } // End of class block
} // End of namespace block