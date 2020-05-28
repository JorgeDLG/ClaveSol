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
    public class List_InstrumentController : Controller
    {
        private readonly ClaveSolDbContext _context;

        public List_InstrumentController(ClaveSolDbContext context)
        {
            _context = context;
        }

        // GET: List_Instrument
        public async Task<IActionResult> Index()
        {
            var claveSolDbContext = _context.List_Instrument.Include(l => l.Instrument).Include(l => l.List);
            return View(await claveSolDbContext.ToListAsync());
        }

        // GET: List_Instrument/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list_Instrument = await _context.List_Instrument
                .Include(l => l.Instrument)
                .Include(l => l.List)
                .FirstOrDefaultAsync(m => m.ListId == id);
            if (list_Instrument == null)
            {
                return NotFound();
            }

            return View(list_Instrument);
        }

        // GET: List_Instrument/Create
        public IActionResult Create()
        {
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand");
            ViewData["ListId"] = new SelectList(_context.List, "Id", "Name");
            return View();
        }

        // POST: List_Instrument/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ListId,InstrumentId")] List_Instrument list_Instrument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(list_Instrument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", list_Instrument.InstrumentId);
            ViewData["ListId"] = new SelectList(_context.List, "Id", "Name", list_Instrument.ListId);
            return View(list_Instrument);
        }

        // GET: List_Instrument/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list_Instrument = await _context.List_Instrument.FindAsync(id);
            if (list_Instrument == null)
            {
                return NotFound();
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", list_Instrument.InstrumentId);
            ViewData["ListId"] = new SelectList(_context.List, "Id", "Name", list_Instrument.ListId);
            return View(list_Instrument);
        }

        // POST: List_Instrument/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ListId,InstrumentId")] List_Instrument list_Instrument)
        {
            if (id != list_Instrument.ListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(list_Instrument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!List_InstrumentExists(list_Instrument.ListId))
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
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", list_Instrument.InstrumentId);
            ViewData["ListId"] = new SelectList(_context.List, "Id", "Name", list_Instrument.ListId);
            return View(list_Instrument);
        }

        // GET: List_Instrument/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list_Instrument = await _context.List_Instrument
                .Include(l => l.Instrument)
                .Include(l => l.List)
                .FirstOrDefaultAsync(m => m.ListId == id);
            if (list_Instrument == null)
            {
                return NotFound();
            }

            return View(list_Instrument);
        }

        // POST: List_Instrument/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var list_Instrument = await _context.List_Instrument.FindAsync(id);
            _context.List_Instrument.Remove(list_Instrument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool List_InstrumentExists(int id)
        {
            return _context.List_Instrument.Any(e => e.ListId == id);
        }
    }
}
