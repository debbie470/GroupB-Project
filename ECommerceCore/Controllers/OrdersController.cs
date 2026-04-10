using ECommerceCore.Data;
using ECommerceCore.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerceCore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            if (User.IsInRole("Admin"))
            {
                var allorders = await _context.Orders
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Products)
                    .ToListAsync();

                return View(allorders);
            }
            else if (User.IsInRole("Supplier"))
            {
                var supplierProducts = await _context.Products
                    .Where(p => p.Suppliers.UserId == userId)
                    .ToListAsync();

                var supplierProductIds = supplierProducts
                    .Select(p => p.ProductsId)
                    .ToList();

                var supplierOrders = await _context.OrderProducts
                    .Where(op => supplierProductIds.Contains(op.ProductsId))
                    .Include(op => op.Orders)
                    .Include(op => op.Products)
                    .ToListAsync();

                return View(supplierOrders.Select(op => op.Orders).Distinct().ToList());
            }
            else
            {
                var userOrders = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Products)
                    .ToListAsync();

                return View(userOrders);
            }
        }




        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.OrderProducts
                .Where(op => op.OrdersId == id)
                .Include(op => op.Orders)
                .Include(op => op.Products)
                .ToListAsync();

            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create(int basketId)
        {
            ViewBag.basketId = basketId;
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrdersId,Delivery,Collection,DeliveryType,CollectionDate")] Orders orders, int basketId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get basket
            var basket = await _context.Basket
            .FirstOrDefaultAsync(x => x.BasketId == basketId && x.UserId == userId && x.Status);

            if (basket == null)
            {
                return NotFound();
            }

            // Get basket products
            var basketProducts = await _context.BasketProducts
            .Where(x => x.BasketId == basketId)
            .Include(x => x.Products)
            .ToListAsync();

            if (!basketProducts.Any())
            {
                ModelState.AddModelError("", "Your basket is empty.");
                ViewBag.BasketId = basketId;
                return View(orders);
            }

            // Assign values
            orders.UserId = userId;
            orders.OrderDate = DateOnly.FromDateTime(DateTime.Today);
            orders.OrderTrackingStatus = "Pending";

            // Calculate subtotal
            decimal subtotal = 0.00m;
            foreach (var basketProduct in basketProducts)
            {
                var productTotal = basketProduct.Products.Price * basketProduct.Quantity;
                subtotal += productTotal;
            }

            // Apply discount
            decimal discount = 0.00m;
            foreach (var basketProduct in basketProducts)
            {
                var productTotal = basketProduct.Products.Price * basketProduct.Quantity;
                subtotal += productTotal;
            }

            var orderCount = await _context.Orders.CountAsync(x => x.UserId == userId);

            if (orderCount >= 5)
            {
                discount = subtotal * 0.10m;
            }

            orders.Subtotal = subtotal - discount;

            // Save order
            _context.Orders.Add(orders);
            await _context.SaveChangesAsync();

            // Create order products + update stock
            foreach (var basketProduct in basketProducts)
            {
                if (basketProduct.Products.Stock < basketProduct.Quantity)
                {
                    ModelState.AddModelError("", $"Not enough stock for {basketProduct.Products.ProductName}");
                    ViewBag.BasketId = basketId;
                    return View("OrderError");
                }

                var orderProduct = new OrderProducts
                {
                    OrdersId = orders.OrdersId,
                    ProductsId = basketProduct.ProductsId,
                    Quantity = basketProduct.Quantity
                };

                _context.OrderProducts.Add(orderProduct);

                // Reduce stock
                basketProduct.Products.Stock -= basketProduct.Quantity;
            }

            // Close basket
            basket.Status = false;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrdersId,UserId,Subtotal,Delivery,Collection,DeliveryType,OrderTrackingStatus,CollectionDate,OrderDate")] Orders orders)
        {
            if (id != orders.OrdersId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.OrdersId))
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
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrdersId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            if (orders != null)
            {
                _context.Orders.Remove(orders);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrdersId == id);
        }
    }
}
