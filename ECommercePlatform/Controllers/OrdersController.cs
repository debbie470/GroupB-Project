using ECommercePlatform.Data; // Imports the database context for data access
using ECommercePlatform.Models; // Imports the application domain models
using Microsoft.AspNetCore.Authorization; // Imports security attributes for access control
using Microsoft.AspNetCore.Mvc; // Imports core MVC framework functionality
using Microsoft.AspNetCore.Mvc.Rendering; // Imports helpers for UI rendering
using Microsoft.EntityFrameworkCore; // Imports Entity Framework Core for ORM operations
using System; // Imports base system types
using System.Collections.Generic; // Imports generic collection types
using System.Linq; // Imports LINQ for data querying
using System.Security.Claims; // Imports claims-based identity types
using System.Threading.Tasks; // Imports asynchronous programming support

namespace ECommercePlatform.Controllers // Defines the controller namespace
{ // Start of namespace
    public class OrdersController : Controller // Defines the controller for managing orders
    { // Start of class
        private readonly ApplicationDbContext _context; // Declares private field for database access

        public OrdersController(ApplicationDbContext context) // Constructor with dependency injection
        { // Start of constructor
            _context = context; // Assigns the injected context to the private field
        } // End of constructor

        // GET: Orders
        public async Task<IActionResult> Index() // Action to list orders based on user role
        { // Start of Index method
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Retrieves the current user's ID

            if (userId == null) // Checks if the user is not authenticated
            { // Start of null check
                return Unauthorized(); // Returns 401 Unauthorized status
            } // End of null check

            if (User.IsInRole("Admin")) // Logic for Admin users
            { // Start of Admin block
                var allorders = await _context.Orders // Queries all orders
                    .Include(o => o.OrderProducts) // Includes associated product relations
                    .ThenInclude(op => op.Products) // Includes specific product details
                    .ToListAsync(); // Executes the query asynchronously

                return View(allorders); // Returns the view with all orders
            } // End of Admin block
            else if (User.IsInRole("Supplier")) // Logic for Supplier users
            { // Start of Supplier block
                var supplierProducts = await _context.Products // Queries products
                    .Where(p => p.Suppliers.UserId == userId) // Filters products belonging to this supplier
                    .ToListAsync(); // Executes query

                var supplierProductIds = supplierProducts // Creates a list of IDs
                    .Select(p => p.ProductsId) // Selects only the product ID
                    .ToList(); // Converts to list

                var supplierOrders = await _context.OrderProducts // Queries order-product links
                    .Where(op => supplierProductIds.Contains(op.ProductsId)) // Filters for supplier's products
                    .Include(op => op.Orders) // Includes the parent order
                    .Include(op => op.Products) // Includes the product details
                    .ToListAsync(); // Executes query

                return View(supplierOrders.Select(op => op.Orders).Distinct().ToList()); // Returns unique orders containing supplier's products
            } // End of Supplier block
            else // Default logic for Customers
            { // Start of Customer block
                var userOrders = await _context.Orders // Queries orders
                    .Where(o => o.UserId == userId) // Filters for the current user's orders
                    .Include(o => o.OrderProducts) // Includes product relations
                    .ThenInclude(op => op.Products) // Includes product details
                    .ToListAsync(); // Executes query

                return View(userOrders); // Returns the view with user's specific orders
            } // End of Customer block
        } // End of Index method


        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id) // Action to view order details
        { // Start of Details method
            if (id == null) // Validates ID presence
            { // Start check
                return NotFound(); // Returns 404
            } // End check

            var orders = await _context.OrderProducts // Queries the order-product mapping
                .Where(op => op.OrdersId == id) // Filters by the specific order ID
                .Include(op => op.Orders) // Includes order metadata
                .Include(op => op.Products) // Includes product descriptions
                .ToListAsync(); // Executes query

            if (orders == null) // Checks if record exists
            { // Start check
                return NotFound(); // Returns 404
            } // End check

            return View(orders); // Displays order details view
        } // End of Details method


        // GET: Orders/Create
        public IActionResult Create(int basketId) // Action to show order checkout form
        { // Start of Create GET
            ViewBag.basketId = basketId; // Passes the basket ID to the view via ViewBag
            return View(); // Returns the checkout view
        } // End of Create GET

        // POST: Orders/Create
        [HttpPost] // Restricts to POST requests
        [ValidateAntiForgeryToken] // Security check for form submission
        public async Task<IActionResult> Create([Bind("OrdersId,Delivery,Collection,DeliveryType,CollectionDate")] Orders orders, int basketId) // Processes checkout
        { // Start of Create POST
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Identifies current user

            // Get basket
            var basket = await _context.Basket // Locates the active basket
            .FirstOrDefaultAsync(x => x.BasketId == basketId && x.UserId == userId && x.Status); // Validates ownership and active status

            if (basket == null) // Checks if basket is valid
            { // Start check
                return NotFound(); // Returns 404
            } // End check

            // Get basket products
            var basketProducts = await _context.BasketProducts // Queries items in the basket
            .Where(x => x.BasketId == basketId) // Filters by basket ID
            .Include(x => x.Products) // Includes product details for pricing/stock
            .ToListAsync(); // Executes query

            if (!basketProducts.Any()) // Validates that basket is not empty
            { // Start check
                ModelState.AddModelError("", "Your basket is empty."); // Adds validation error
                ViewBag.BasketId = basketId; // Keeps basket ID for the view
                return View(orders); // Returns to checkout with error
            } // End check

            // Assign values
            orders.UserId = userId; // Links order to the user
            orders.OrderDate = DateOnly.FromDateTime(DateTime.Today); // Sets order date to today
            orders.OrderTrackingStatus = "Pending"; // Sets initial tracking status

            // Calculate subtotal
            decimal subtotal = 0.00m; // Initializes subtotal variable
            foreach (var basketProduct in basketProducts) // Iterates through items
            { // Start loop
                var productTotal = basketProduct.Products.Price * basketProduct.Quantity; // Calculates line item price
                subtotal += productTotal; // Adds to running subtotal
            } // End loop

            // Apply discount
            decimal discount = 0.00m; // Initializes discount variable
            var orderCount = await _context.Orders.CountAsync(x => x.UserId == userId); // Counts previous orders for loyalty

            if (orderCount >= 5) // Checks loyalty threshold
            { // Start check
                discount = subtotal * 0.10m; // Applies 10% discount
            } // End check

            orders.Subtotal = subtotal - discount; // Sets final order total

            // Save order
            _context.Orders.Add(orders); // Adds order to tracking
            await _context.SaveChangesAsync(); // Saves to DB to generate Order ID


            // Add loyalty points after successful order
            var loyaltyRewards = await _context.LoyaltyRewards // Checks for existing loyalty profile
                .FirstOrDefaultAsync(lr => lr.UserId == userId); // Filters by user ID

            if (loyaltyRewards == null) // Creates profile if missing
            { // Start check
                loyaltyRewards = new LoyaltyRewards // Initializes new loyalty object
                { // Start init
                    UserId = userId, // Links to user
                    PointsBalance = 0, // Starts at zero
                    TierLevel = "Bronze", // Default tier
                    History = "Initial Points" // Audit log entry
                }; // End init

                _context.LoyaltyRewards.Add(loyaltyRewards); // Adds to context
            } // End check

            // Add loyalty points (1 point per $ spent, for example)
            int earnedPoints = (int)orders.Subtotal; // Converts subtotal to integer points

            // Update loyalty points balance
            loyaltyRewards.PointsBalance += earnedPoints; // Increments balance
            // Save the updated loyalty points
            await _context.SaveChangesAsync(); // Saves loyalty changes



            // Create order products + update stock
            foreach (var basketProduct in basketProducts) // Iterates to convert basket items to order items
            { // Start loop
                if (basketProduct.Products.Stock < basketProduct.Quantity) // Checks inventory
                { // Start stock check
                    ModelState.AddModelError("", $"Not enough stock for {basketProduct.Products.ProductName}"); // Error if oversold
                    ViewBag.BasketId = basketId; // Keeps context
                    return View("OrderError"); // Returns specialized error view
                } // End stock check

                var orderProduct = new OrderProducts // Creates the relationship record
                { // Start init
                    OrdersId = orders.OrdersId, // Links to new order
                    ProductsId = basketProduct.ProductsId, // Links to product
                    Quantity = basketProduct.Quantity // Preserves quantity
                }; // End init

                _context.OrderProducts.Add(orderProduct); // Adds to context

                // Reduce stock
                basketProduct.Products.Stock -= basketProduct.Quantity; // Deducts from inventory
            } // End loop

            // Close basket
            basket.Status = false; // Deactivates the basket after checkout

            await _context.SaveChangesAsync(); // Commits all order items, stock updates, and basket status
            TempData["SuccessMessage"] = "Order placed successfully!"; // Sets success notification

            return RedirectToAction("Index", "Home"); // Redirects to home page

        } // End of Create POST

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id) // Action to show order edit form
        { // Start of Edit GET
            if (id == null) // Validates ID
            { // Start check
                return NotFound(); // Returns 404
            } // End check

            var orders = await _context.Orders.FindAsync(id); // Locates order
            if (orders == null) // Checks if exists
            { // Start check
                return NotFound(); // Returns 404
            } // End check
            return View(orders); // Returns edit view
        } // End of Edit GET

        // POST: Orders/Edit/5
        [Authorize(Roles = "Supplier")] // Only suppliers can modify order status
        [HttpPost] // Restricts to POST
        [ValidateAntiForgeryToken] // Security token check
        public async Task<IActionResult> Edit(int id, [Bind("OrdersId,UserId,Subtotal,Delivery,Collection,DeliveryType,OrderTrackingStatus,CollectionDate,OrderDate")] Orders orders) // Processes edits
        { // Start of Edit POST
            if (id != orders.OrdersId) // Checks ID consistency
            { // Start check
                return NotFound(); // Returns 404
            } // End check

            if (ModelState.IsValid) // Validates data
            { // Start validation
                try // Concurrency handling
                { // Start try
                    _context.Update(orders); // Updates record
                    await _context.SaveChangesAsync(); // Commits to DB
                } // End try
                catch (DbUpdateConcurrencyException) // Catches update conflicts
                { // Start catch
                    if (!OrdersExists(orders.OrdersId)) // Checks if record was deleted
                    { // Start check
                        return NotFound(); // Returns 404
                    } // End check
                    else // If exists but update failed
                    { // Start else
                        throw; // Rethrows exception
                    } // End else
                } // End catch
                return RedirectToAction(nameof(Index)); // Redirects to list
            } // End validation
            return View(orders); // Returns view with errors
        } // End of Edit POST

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id) // Action to show delete confirmation
        { // Start of Delete GET
            if (id == null) // Validates ID
            { // Start check
                return NotFound(); // Returns 404
            } // End check

            var orders = await _context.Orders // Queries orders
                .FirstOrDefaultAsync(m => m.OrdersId == id); // Finds specific order
            if (orders == null) // Checks if exists
            { // Start check
                return NotFound(); // Returns 404
            } // End check

            return View(orders); // Returns delete confirmation view
        } // End of Delete GET

        // POST: Orders/Delete/5
        [Authorize(Roles = "Supplier")] // Only suppliers can delete
        [HttpPost, ActionName("Delete")] // Maps to logical Delete action
        [ValidateAntiForgeryToken] // Security token check
        public async Task<IActionResult> DeleteConfirmed(int id) // Performs deletion
        { // Start of Delete POST
            var orders = await _context.Orders.FindAsync(id); // Locates order
            if (orders != null) // If found
            { // Start check
                _context.Orders.Remove(orders); // Marks for removal
            } // End check

            await _context.SaveChangesAsync(); // Commits deletion to DB
            return RedirectToAction(nameof(Index)); // Redirects to list
        } // End of Delete POST

        private bool OrdersExists(int id) // Private helper for existence check
        { // Start method
            return _context.Orders.Any(e => e.OrdersId == id); // Returns true if ID exists in DB
        } // End method
    } // End of class
} // End of namespace