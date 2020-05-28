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
    public class Shop_InsController : Controller
    {
        private readonly ClaveSolDbContext _context;

        public Shop_InsController(ClaveSolDbContext context)
        {
            _context = context;
        }

        // GET: Shop_Ins
        public async Task<IActionResult> Index()
        {
            var claveSolDbContext = _context.Shop_Ins.Include(s => s.Instrument).Include(s => s.Shop);
            return View(await claveSolDbContext.ToListAsync());
        }

        // GET: Shop_Ins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Ins = await _context.Shop_Ins
                .Include(s => s.Instrument)
                .Include(s => s.Shop)
                .FirstOrDefaultAsync(m => m.ShopId == id);
            if (shop_Ins == null)
            {
                return NotFound();
            }

            return View(shop_Ins);
        }

        // GET: Shop_Ins/Create
        public IActionResult Create()
        {
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand");
            ViewData["ShopId"] = new SelectList(_context.Shop, "Id", "City");
            return View();
        }

        // POST: Shop_Ins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShopId,InstrumentId")] Shop_Ins shop_Ins)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop_Ins);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", shop_Ins.InstrumentId);
            ViewData["ShopId"] = new SelectList(_context.Shop, "Id", "City", shop_Ins.ShopId);
            return View(shop_Ins);
        }

        // GET: Shop_Ins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Ins = await _context.Shop_Ins.FindAsync(id);
            if (shop_Ins == null)
            {
                return NotFound();
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", shop_Ins.InstrumentId);
            ViewData["ShopId"] = new SelectList(_context.Shop, "Id", "City", shop_Ins.ShopId);
            return View(shop_Ins);
        }

        // POST: Shop_Ins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShopId,InstrumentId")] Shop_Ins shop_Ins)
        {
            if (id != shop_Ins.ShopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop_Ins);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Shop_InsExists(shop_Ins.ShopId))
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
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", shop_Ins.InstrumentId);
            ViewData["ShopId"] = new SelectList(_context.Shop, "Id", "City", shop_Ins.ShopId);
            return View(shop_Ins);
        }

        // GET: Shop_Ins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Ins = await _context.Shop_Ins
                .Include(s => s.Instrument)
                .Include(s => s.Shop)
                .FirstOrDefaultAsync(m => m.ShopId == id);
            if (shop_Ins == null)
            {
                return NotFound();
            }

            return View(shop_Ins);
        }

        // POST: Shop_Ins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shop_Ins = await _context.Shop_Ins.FindAsync(id);
            _context.Shop_Ins.Remove(shop_Ins);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Shop_InsExists(int id)
        {
            return _context.Shop_Ins.Any(e => e.ShopId == id);
        }
    }
}
