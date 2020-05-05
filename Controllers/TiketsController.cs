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
    public class TiketsController : Controller
    {
        private readonly ClaveSolDbContext _context;

        public TiketsController(ClaveSolDbContext context)
        {
            _context = context;
        }

        // GET: Tikets
        public async Task<IActionResult> Index()
        {
            var claveSolDbContext = _context.Tiket.Include(t => t.Operator).Include(t => t.Order);
            return View(await claveSolDbContext.ToListAsync());
        }

        // GET: Tikets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tiket = await _context.Tiket
                .Include(t => t.Operator)
                .Include(t => t.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tiket == null)
            {
                return NotFound();
            }

            return View(tiket);
        }

        // GET: Tikets/Create
        public IActionResult Create()
        {
            ViewData["OperatorId"] = new SelectList(_context.Operator, "Id", "Id");
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "State");
            return View();
        }

        // POST: Tikets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Category,Title,Body,OperatorId,OrderId")] Tiket tiket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tiket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OperatorId"] = new SelectList(_context.Operator, "Id", "Id", tiket.OperatorId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "State", tiket.OrderId);
            return View(tiket);
        }

        // GET: Tikets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tiket = await _context.Tiket.FindAsync(id);
            if (tiket == null)
            {
                return NotFound();
            }
            ViewData["OperatorId"] = new SelectList(_context.Operator, "Id", "Id", tiket.OperatorId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "State", tiket.OrderId);
            return View(tiket);
        }

        // POST: Tikets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Category,Title,Body,OperatorId,OrderId")] Tiket tiket)
        {
            if (id != tiket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tiket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TiketExists(tiket.Id))
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
            ViewData["OperatorId"] = new SelectList(_context.Operator, "Id", "Id", tiket.OperatorId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "State", tiket.OrderId);
            return View(tiket);
        }

        // GET: Tikets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tiket = await _context.Tiket
                .Include(t => t.Operator)
                .Include(t => t.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tiket == null)
            {
                return NotFound();
            }

            return View(tiket);
        }

        // POST: Tikets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tiket = await _context.Tiket.FindAsync(id);
            _context.Tiket.Remove(tiket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TiketExists(int id)
        {
            return _context.Tiket.Any(e => e.Id == id);
        }
    }
}
