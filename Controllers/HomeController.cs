﻿using System;
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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClaveSolDbContext _context;

        public HomeController(ILogger<HomeController> logger,ClaveSolDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchStr)
        {
            ViewData["CurrentFilter"] = searchStr;
            //var claveSolDbContext = _context.Instrument.Include(i => i.LineOrder).Include(i => i.SubCategory);
            var claveSolDbContext = from s in _context.Instrument
                                    select s;

            if (!String.IsNullOrEmpty(searchStr))
            {
                claveSolDbContext = claveSolDbContext.Where(s => s.Name.Contains(searchStr)
                                                    || s.Brand.Contains(searchStr));
            }


            return View(await claveSolDbContext.AsNoTracking().ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
