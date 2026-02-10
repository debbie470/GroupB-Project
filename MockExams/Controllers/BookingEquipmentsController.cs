using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MockExams.Data;
using MockExams.Models;

namespace MockExams.Controllers
{
    public class BookingEquipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingEquipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookingEquipments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookingEquipment.Include(b => b.Equipment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BookingEquipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingEquipment = await _context.BookingEquipment
                .Include(b => b.Equipment)
                .FirstOrDefaultAsync(m => m.BookingEquipmentId == id);
            if (bookingEquipment == null)
            {
                return NotFound();
            }

            return View(bookingEquipment);
        }
        [Authorize(Roles = "Admin,Customer" )]
        // GET: BookingEquipments/Create
        public IActionResult Create(int EquipmentId)
        {
            ViewBag.EquipmentId = EquipmentId;
            return View();
        }

        // POST: BookingEquipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("BookingEquipmentId,EquipmentId,Quantity,IsAvailable")] BookingEquipment bookingEquipment)
        {
          var equipment = await _context.Equipment.FindAsync(bookingEquipment.EquipmentId);
            if (equipment == null)
            {
                return View(bookingEquipment);
            }
            bookingEquipment.Equipment = equipment;
            ModelState.Remove("Equipment");

            if (ModelState.IsValid)
            {
                _context.Add(bookingEquipment);
                await _context.SaveChangesAsync();
                equipment.IsAvailable = false;
                return RedirectToAction(nameof(Index));
            }
            ViewBag.EquipmentId = bookingEquipment;
            return View(bookingEquipment);
        }

        // GET: BookingEquipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingEquipment = await _context.BookingEquipment.FindAsync(id);
            if (bookingEquipment == null)
            {
                return NotFound();
            }
            ViewData["EquipmentId"] = new SelectList(_context.Set<Equipment>(), "EquipmentId", "EquipmentId", bookingEquipment.EquipmentId);
            return View(bookingEquipment);
        }

        // POST: BookingEquipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("BookingEquipmentId,EquipmentId,Quantity,IsAvailable")] BookingEquipment bookingEquipment)
        {
            if (id != bookingEquipment.BookingEquipmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingEquipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingEquipmentExists(bookingEquipment.BookingEquipmentId))
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
            ViewData["EquipmentId"] = new SelectList(_context.Set<Equipment>(), "EquipmentId", "EquipmentId", bookingEquipment.EquipmentId);
            return View(bookingEquipment);
        }
        [Authorize(Roles = "Staff")]
        // GET: BookingEquipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingEquipment = await _context.BookingEquipment
                .Include(b => b.Equipment)
                .FirstOrDefaultAsync(m => m.BookingEquipmentId == id);
            if (bookingEquipment == null)
            {
                return NotFound();
            }

            return View(bookingEquipment);
        }

        // POST: BookingEquipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookingEquipment = await _context.BookingEquipment.FindAsync(id);
            if (bookingEquipment != null)
            {
                _context.BookingEquipment.Remove(bookingEquipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingEquipmentExists(int id)
        {
            return _context.BookingEquipment.Any(e => e.BookingEquipmentId == id);
        }
    }
}
