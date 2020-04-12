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

    public class UserController : Controller
    {
        private readonly MvcUserContext _context; //deps insertion?
        public UserController(MvcUserContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            var users = from m in _context.User
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.Contains(searchString));
            }

            return View(await users.ToListAsync());
        }
    }
}