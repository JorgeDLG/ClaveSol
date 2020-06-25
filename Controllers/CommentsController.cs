using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ClaveSol.Data;
using ClaveSol.Models;

namespace ClaveSol.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ClaveSolDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CommentsController(ClaveSolDbContext context , SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var claveSolDbContext = _context.Comment.Include(c => c.Instrument).Include(c => c.User);
            return View(await claveSolDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Instrument)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Mail");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Title,Body,Stars,Deleted,UserId,InstrumentId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", comment.InstrumentId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Mail", comment.UserId);
            return View(comment);
        }

        //AJAX Create Comment
        public Dictionary<string,string> createComment(int insId, string title, string body)
        {
            Dictionary<string,string> commentValues = new Dictionary<string, string>();
            User userLogged = retrieveUser();
            Comment comment;
            try
            {
                
             comment = new Comment{
                Date = System.DateTime.Now,
                Title = title,
                Body = body,
                Stars = 0,
                Deleted = false,
                User = userLogged,
                UserId = userLogged.Id,
                Instrument = _context.Instrument.Find(insId),
                InstrumentId = insId
                };
                _context.Add(comment);
                _context.SaveChanges();
            }
            catch (System.Exception){throw;}


            commentValues.Add("title",comment.Title);
            commentValues.Add("body",comment.Body);
            commentValues.Add("author",userLogged.Name);

            return commentValues;

            User retrieveUser() //%FUNC DUPLICATED ON CARTCONTROLLER!!
            {
                try
                {
                    var claimUser = _signInManager.Context.User; //%if no LOGIN throw EXCEP
                    string ideUserId = _signInManager.UserManager.GetUserId(claimUser);
                    return _context.User.Where(x => x.OwnerID == ideUserId).FirstOrDefault();
                }
                catch (System.Exception){throw;}
            }
        }


        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", comment.InstrumentId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Mail", comment.UserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Title,Body,Stars,Deleted,UserId,InstrumentId")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "Id", "Brand", comment.InstrumentId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Mail", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Instrument)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}
