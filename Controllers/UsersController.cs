using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClaveSol.Data;
using ClaveSol.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace ClaveSol.Controllers
{
    public class UsersController : Controller
    {
        private readonly ClaveSolDbContext _context;
        private readonly ApplicationDbContext _IDcontext;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ClaveSolDbContext context, ApplicationDbContext IDcontext, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _IDcontext = IDcontext;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Mail,Premium,OwnerID,Status")] User user,string pass, string role)
        {
            user.Status = 0;
            if (ModelState.IsValid)
            {
                _context.Add(user);

                await IdentityLink( pass, role, user,'c');

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Mail,Premium,OwnerID,Status")] User user, string pass, string role)
        {
            user.Status = 0;
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await IdentityLink( null, role, user,'e');
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);

            await IdentityLink( null, null, user,'d');
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
        public async Task IdentityLink(string pass,string role,User user,char operation)
        {
            //IdentityUser idUser;

            switch (operation)
            {
                case 'c': //Create
                    var idUser = new IdentityUser { UserName = user.Mail, Email = user.Mail };
                    var result = await _userManager.CreateAsync(idUser, pass);
                    var result2 = _userManager.AddToRoleAsync(idUser, role);
                    user.OwnerID = idUser.Id;

                    //let login
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(idUser);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = idUser.Id, code = code },
                        protocol: Request.Scheme);
                    idUser.EmailConfirmed = true;

                break;

                case 'e': //Edit
                    var idUser2 = await _userManager.FindByIdAsync(user.OwnerID);
                    bool isAdmin = await _userManager.IsInRoleAsync(idUser2,"admin");

                    if (isAdmin && role == "normal")
                    {
                        await _userManager.RemoveFromRoleAsync(idUser2,"admin");
                        await _userManager.AddToRoleAsync(idUser2,"normal");
                    }
                    else if(!isAdmin && role == "admin")
                    {
                        await _userManager.RemoveFromRoleAsync(idUser2,"normal");
                        await _userManager.AddToRoleAsync(idUser2,"admin");
                    }
                    idUser2.Email = user.Mail;
                    idUser2.UserName = user.Mail;

                    idUser2.EmailConfirmed = true;
                    await _userManager.UpdateAsync(idUser2);
                break;

                case 'd': //Delete
                    var idUser3 = await _userManager.FindByIdAsync(user.OwnerID);
                    var res = await _userManager.DeleteAsync(idUser3);
                break;
                default:
                    Console.WriteLine("Unexpeted operation in switch case");
                break;
            }
        }
    }
}
