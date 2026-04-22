using ECommercePlatform.Data; // Imports the database context for application data access
using Microsoft.AspNetCore.Authorization; // Imports attributes for securing controller actions
using Microsoft.AspNetCore.Authorization.Infrastructure; // Imports core infrastructure for auth policies
using Microsoft.AspNetCore.Mvc; // Imports the ASP.NET Core MVC framework components
using Microsoft.EntityFrameworkCore; // Imports Entity Framework Core for database querying
using System.Security.Claims; // Imports types for managing user identity and claims
using System.Threading.Tasks; // Imports support for asynchronous programming

namespace ECommerceCore.Controllers // Defines the namespace for the dashboard controller
{ // Start of namespace
    [Authorize(Roles = "Supplier")] // Restricts access to this controller to authenticated users in the "Supplier" role
    public class SupplierDashboardController : Controller // Defines the controller class for the supplier dashboard
    { // Start of class
        private readonly ApplicationDbContext _context; // Declares a private field for the database context

        public SupplierDashboardController(ApplicationDbContext context) // Constructor to inject the database context
        { // Start of constructor
            _context = context; // Assigns the injected context to the private field
        } // End of constructor

        public async Task<IActionResult> Index() // Asynchronous action to load the dashboard view
        { // Start of Index method
            var userld = User.FindFirstValue(ClaimTypes.NameIdentifier); // Retrieves the current user's unique identifier from claims

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.UserId == userld); // Finds the supplier profile linked to the user ID

            if (supplier == null) // Checks if the authenticated user has a valid supplier profile
            { // Start of null check
                return NotFound(); // Returns 404 error if the supplier record is missing
            } // End of null check

            var products = await _context.Products.Where(x => x.SuppliersId == supplier.SuppliersId).ToListAsync(); // Retrieves all products belonging to this supplier

            var orders = await _context.Orders // Begins a query for customer orders
                .Include(o => o.OrderProducts) // Includes the link between orders and products
                .ThenInclude(op => op.Products) // Includes product details within the order
                .Where(o => o.OrderProducts.Any(op => op.Products.SuppliersId == supplier.SuppliersId)) // Filters for orders containing this supplier's items
                .ToListAsync(); // Executes the query asynchronously

            ViewBag.TotalProducts = products.Count; // Stores total product count in ViewBag for display

            ViewBag.LowStockCount = products.Count(x => x.Stock <= 5); // Calculates and stores the count of items with 5 or fewer units remaining

            ViewBag.RecentOrders = orders; // Passes the filtered list of orders to the view

            return View(products); // Returns the view using the supplier's product list as the data model
        } // End of Index method
    } // End of class
} // End of namespace