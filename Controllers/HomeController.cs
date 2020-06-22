using System.Globalization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
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

        public async Task<IActionResult> Index(string searchStr, int subcatId)
        {
            //Query Search string Filter
            ViewData["currentQuery"] = searchStr;
            var instruments = from s in _context.Instrument
                                    select s;

            if (!String.IsNullOrEmpty(searchStr))
            {
                instruments = instruments.Where(s => s.Name.Contains(searchStr)
                                                    || s.Brand.Contains(searchStr));
            }

            //SubCategories Filter
            @ViewBag.SubCats = _context.SubCategory.ToList();

            var subCats = from s in _context.SubCategory
                                    select s;

            if (subcatId != 0)
            {
                instruments = instruments.Where(s => s.SubCategoryId.Equals(subcatId));
            }

            return View(await instruments.AsNoTracking().ToListAsync());
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
