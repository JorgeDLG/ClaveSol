using System.Runtime.CompilerServices;
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
    public class InstrumentsController : Controller
    {
        private readonly ClaveSolDbContext _context;

        public InstrumentsController(ClaveSolDbContext context)
        {
            _context = context;
        }

        // GET: Instruments
        public async Task<IActionResult> Index()
        {
            var claveSolDbContext = _context.Instrument.Include(i => i.LineOrder).Include(i => i.SubCategory);

            //return View(await claveSolDbContext.ToListAsync());

            //Query to get Instruments with attrib Color=Red
            var insAttrRed = from ins in _context.Instrument
            join attr in _context.Attribut on ins.Id equals attr.Id into test 
            from attr2 in test
            where attr2.Id == 1
            select ins;

            return View(await insAttrRed.ToListAsync());

            //claveSolDbContext = _context.Instrument.Where(x => x.Attribut_Inss.Any(s => s.AttributId == Attribut.Id));
            // context.Clinics.Where(x => x.Doctors.Any(d => d.Specialties.Any(s => s.SpecialtyId == specialtyId)))
        }

        // GET: Instruments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument
                .Include(i => i.LineOrder)
                .Include(i => i.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instrument == null)
            {
                return NotFound();
            }

            //get Cat.Name of Ins
               var CatName = from c in _context.Category
               where c.Id == instrument.SubCategory.CategoryId
               select c.Name;
                ViewBag.catName = CatName.FirstOrDefault();

            return View(instrument);
        }

        // GET: Instruments/Create
        public IActionResult Create()
        {
            ViewData["LineOrderId"] = new SelectList(_context.LineOrder, "Id", "Name");
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Name");
            return View();
        }

        // POST: Instruments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Brand,Price,State,Description,MediaDir,SubCategoryId,LineOrderId,InstrumentId,shInsId")] Instrument instrument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instrument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LineOrderId"] = new SelectList(_context.LineOrder, "Id", "Name", instrument.LineOrderId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Name", instrument.SubCategoryId);
            return View(instrument);
        }

        // GET: Instruments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument.FindAsync(id);
            if (instrument == null)
            {
                return NotFound();
            }
            ViewData["LineOrderId"] = new SelectList(_context.LineOrder, "Id", "Name", instrument.LineOrderId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Name", instrument.SubCategoryId);
            return View(instrument);
        }

        // POST: Instruments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Brand,Price,State,Description,MediaDir,SubCategoryId,LineOrderId,InstrumentId,shInsId")] Instrument instrument)
        {
            if (id != instrument.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instrument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentExists(instrument.Id))
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
            ViewData["LineOrderId"] = new SelectList(_context.LineOrder, "Id", "Name", instrument.LineOrderId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Name", instrument.SubCategoryId);
            return View(instrument);
        }

        // GET: Instruments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument
                .Include(i => i.LineOrder)
                .Include(i => i.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instrument == null)
            {
                return NotFound();
            }

            return View(instrument);
        }

        // POST: Instruments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrument = await _context.Instrument.FindAsync(id);
            _context.Instrument.Remove(instrument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstrumentExists(int id)
        {
            return _context.Instrument.Any(e => e.Id == id);
        }
    }
}
