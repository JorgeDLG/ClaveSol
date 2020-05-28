using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ClaveSol.Data;
using ClaveSol.Models;

namespace ClaveSol.Controllers
{
    public class nnManager : Controller
    {
        private readonly ClaveSolDbContext  _context;

        public nnManager(ClaveSolDbContext  context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //var claveSolDbContext = _context.Comment.Include(c => c.Instrument).Include(c => c.User);

            var nnModels = new nnModels
            {
                AttrIns = await _context.Attribut_Ins.ToListAsync(), 
                ListIns = await _context.List_Instrument.ToListAsync(), 
                ShopIns = await _context.Shop_Ins.ToListAsync()
            };
            return View(nnModels);
        } 
    }
}