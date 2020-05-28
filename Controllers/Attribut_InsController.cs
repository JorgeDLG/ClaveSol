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
    public class Attribut_InsController : Controller
    {
        private readonly ClaveSolDbContext _context;

        public Attribut_InsController(ClaveSolDbContext context)
        {
            _context = context;
        }

        // GET: Attribut_Ins
        public async Task<IActionResult> Index()
        {
            var claveSolDbContext = _context.Attribut_Ins.Include(a => a.Attribut).Include(a => a.Instrument);
            return View(await claveSolDbContext.ToListAsync());
        }

        // GET: Attribut_Ins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribut_Ins = await _context.Attribut_Ins
                .Include(a => a.Attribut)
                .Include(a => a.Instrument)
                .FirstOrDefaultAsync(m => m.AttributId == id);
            if (attribut_Ins == null)
            {
                return NotFound();
            }

            return View(attribut_Ins);
        }

        // GET: Attribut_Ins/Create
        public IActionResult Create()
        {
            ViewData["AttributId"] = new SelectList(_context.Attribut, "Id", "Type");
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand");
            return View();
        }

        // POST: Attribut_Ins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AttributId,InstrumentId")] Attribut_Ins attribut_Ins)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attribut_Ins);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttributId"] = new SelectList(_context.Attribut, "Id", "Type", attribut_Ins.AttributId);
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", attribut_Ins.InstrumentId);
            return View(attribut_Ins);
        }

        // GET: Attribut_Ins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribut_Ins = await _context.Attribut_Ins.FindAsync(id);
            if (attribut_Ins == null)
            {
                return NotFound();
            }
            ViewData["AttributId"] = new SelectList(_context.Attribut, "Id", "Type", attribut_Ins.AttributId);
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", attribut_Ins.InstrumentId);
            return View(attribut_Ins);
        }

        // POST: Attribut_Ins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AttributId,InstrumentId")] Attribut_Ins attribut_Ins)
        {
            if (id != attribut_Ins.AttributId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attribut_Ins);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Attribut_InsExists(attribut_Ins.AttributId))
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
            ViewData["AttributId"] = new SelectList(_context.Attribut, "Id", "Type", attribut_Ins.AttributId);
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", attribut_Ins.InstrumentId);
            return View(attribut_Ins);
        }

        // GET: Attribut_Ins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribut_Ins = await _context.Attribut_Ins
                .Include(a => a.Attribut)
                .Include(a => a.Instrument)
                .FirstOrDefaultAsync(m => m.AttributId == id);
            if (attribut_Ins == null)
            {
                return NotFound();
            }

            return View(attribut_Ins);
        }

        // POST: Attribut_Ins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attribut_Ins = await _context.Attribut_Ins.FindAsync(id);
            _context.Attribut_Ins.Remove(attribut_Ins);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Attribut_InsExists(int id)
        {
            return _context.Attribut_Ins.Any(e => e.AttributId == id);
        }
    }
}
