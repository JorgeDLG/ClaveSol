using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClaveSol.Data;
using ClaveSol.Models;

namespace ClaveSol.Controllers
{
    public class LineOrdersController : Controller
    {
        private readonly ClaveSolDbContext _context;

        public LineOrdersController(ClaveSolDbContext context)
        {
            _context = context;
        }

        // GET: LineOrders
        public async Task<IActionResult> Index()
        {
            var claveSolDbContext = _context.LineOrder.Include(l => l.Instrument).Include(l => l.Order);
            return View(await claveSolDbContext.ToListAsync());
        }

        // GET: LineOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineOrder = await _context.LineOrder
                .Include(l => l.Instrument)
                .Include(l => l.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineOrder == null)
            {
                return NotFound();
            }

            return View(lineOrder);
        }

        // GET: LineOrders/Create
        public IActionResult Create()
        {
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand");
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "State");
            return View();
        }

        // POST: LineOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Quantity,UnitaryPrice,TotalPrice,OrderId,InstrumentId")] LineOrder lineOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lineOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", lineOrder.InstrumentId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "State", lineOrder.OrderId);
            return View(lineOrder);
        }

        // GET: LineOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineOrder = await _context.LineOrder.FindAsync(id);
            if (lineOrder == null)
            {
                return NotFound();
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", lineOrder.InstrumentId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "State", lineOrder.OrderId);
            return View(lineOrder);
        }

        // POST: LineOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quantity,UnitaryPrice,TotalPrice,OrderId,InstrumentId")] LineOrder lineOrder)
        {
            if (id != lineOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lineOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineOrderExists(lineOrder.Id))
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
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", lineOrder.InstrumentId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "State", lineOrder.OrderId);
            return View(lineOrder);
        }

        // GET: LineOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineOrder = await _context.LineOrder
                .Include(l => l.Instrument)
                .Include(l => l.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineOrder == null)
            {
                return NotFound();
            }

            return View(lineOrder);
        }

        // POST: LineOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lineOrder = await _context.LineOrder.FindAsync(id);
            _context.LineOrder.Remove(lineOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LineOrderExists(int id)
        {
            return _context.LineOrder.Any(e => e.Id == id);
        }
    }
}
