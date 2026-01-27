using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CityPointRoomHire.Data;
using CityPointRoomHire.Models;

namespace CityPointRoomHire.Controllers
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
            var applicationDbContext = _context.BookingEquipment.Include(b => b.Booking).Include(b => b.Equipment);
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
                .Include(b => b.Booking)
                .Include(b => b.Equipment)
                .FirstOrDefaultAsync(m => m.BookingEquipmentId == id);
            if (bookingEquipment == null)
            {
                return NotFound();
            }

            return View(bookingEquipment);
        }

        // GET: BookingEquipments/Create
        public IActionResult Create()
        {
            ViewData["BookingId"] = new SelectList(_context.Booking, "BookingId", "BookingId");
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentId");
            return View();
        }

        // POST: BookingEquipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingEquipmentId,BookingId,EquipmentId,Quantity")] BookingEquipment bookingEquipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookingEquipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingId"] = new SelectList(_context.Booking, "BookingId", "BookingId", bookingEquipment.BookingId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentId", bookingEquipment.EquipmentId);
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
            ViewData["BookingId"] = new SelectList(_context.Booking, "BookingId", "BookingId", bookingEquipment.BookingId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentId", bookingEquipment.EquipmentId);
            return View(bookingEquipment);
        }

        // POST: BookingEquipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingEquipmentId,BookingId,EquipmentId,Quantity")] BookingEquipment bookingEquipment)
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
            ViewData["BookingId"] = new SelectList(_context.Booking, "BookingId", "BookingId", bookingEquipment.BookingId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentId", bookingEquipment.EquipmentId);
            return View(bookingEquipment);
        }

        // GET: BookingEquipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingEquipment = await _context.BookingEquipment
                .Include(b => b.Booking)
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
