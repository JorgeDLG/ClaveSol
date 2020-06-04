using System.Net;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClaveSol.Data;
using ClaveSol.Models;

namespace ClaveSol.Controllers
{
    public class CartController : Controller
    {
        private readonly ClaveSolDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CartController(ClaveSolDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }
        public ActionResult Index()
        {
            ISession session = _signInManager.Context.Session;

            if(session.GetInt32("cartId") == null)
            {
                return View(_context.LineOrder.ToList());
                //return StatusCode(406);
                //return RedirectToAction("emptyCart");
            }
            
            int cartId = (int)session.GetInt32("cartId"); //OrderID
            var lineas = _context.LineOrder.Where(a => a.OrderId == cartId);

            ViewBag.cartId = cartId;

            return View(lineas.ToList());
        }
        public ActionResult addToCart(int id) //Product obj as param?, Quantity?
        {
            ISession session = _signInManager.Context.Session;
            User user;
            Order cartOrder;

            if(session.GetInt32("cartId") == null)
            {
               user = retrieveUser(); 
               cartOrder = new Order{
                Date = System.DateTime.Now,
                nLines = 0,
                State = "Cart", //Mark order AS CART
                User = user
               };

               _context.Order.Add(cartOrder);
               _context.SaveChanges();
               session.SetInt32("cartId",cartOrder.Id);
            }

            Instrument instrument = _context.Instrument.Find(id);
            LineOrder lineOr = new LineOrder{
               Order = _context.Order.Find((int)session.GetInt32("cartId")),  
               OrderId = (int)session.GetInt32("cartId"),
               Instrument = instrument,
               InstrumentId = instrument.Id,
               Name = instrument.Name,
               Quantity = 1,
               UnitaryPrice = instrument.Price,
               TotalPrice = instrument.Price
            };
            try
            {
                _context.LineOrder.Add(lineOr);
                _context.SaveChanges();
            }
            catch (System.Exception)
            {throw;}
            
            return RedirectToAction("Index","Cart");
        }

        [HttpGet]
        public ActionResult deleteLine(int? id)
        {
            if(id == null)
                return StatusCode(400); //bad request

            LineOrder linea = _context.LineOrder.Find(id);
            if(linea == null)
                return StatusCode(404); //not found
            
            //return View(linea);

            //Temporal until SureDelete View Created:

            _context.LineOrder.Remove(linea);
            _context.SaveChanges();
            return RedirectToAction("Index","Cart");
        }

        [HttpPost]
        [ActionName("deleteLine")]
        [ValidateAntiForgeryToken]
        public ActionResult deleteLinePost(int? id)
        {
            LineOrder linea = _context.LineOrder.Find(id);
            _context.LineOrder.Remove(linea);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public User retrieveUser()
        {
            try
            {
                var claimUser = _signInManager.Context.User; //%if no LOGIN throw EXCEP
                string ideUserId = _signInManager.UserManager.GetUserId(claimUser);
                return _context.User.Where(x => x.OwnerID == ideUserId).FirstOrDefault();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public ActionResult emptyCart()
        {
            return View();
        }
    }
}