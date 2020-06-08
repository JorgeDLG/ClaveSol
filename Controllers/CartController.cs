using System.Net;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClaveSol.Data;
using ClaveSol.Models;

namespace ClaveSol.Controllers
{
    [Authorize]
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
            int cartId;

            if(session.GetInt32("cartId") == null)
            {
                var Cart = retrieveCart(session); //HERE set session."cartId"

                //return View(_context.LineOrder.ToList());
                //return StatusCode(406);
                //return RedirectToAction("emptyCart");
            }
            
            cartId = (int)session.GetInt32("cartId"); //OrderID
            var lineas = _context.LineOrder.Where(a => a.OrderId == cartId);

            ViewBag.cartId = cartId;

            return View(lineas.ToList());
        }
        public ActionResult addToCart(int id) //Product obj as param?, Quantity?
        {
            ISession session = _signInManager.Context.Session;
            //User user;
            Order cartOrder;

            if(session.GetInt32("cartId") == null) //retrieve CART
            {
                cartOrder = retrieveCart(session);

               /*user = retrieveUser(); 
               cartOrder = new Order{
                Date = System.DateTime.Now,
                nLines = 0,
                State = "Cart", //Mark order AS CART
                User = user
               };

               _context.Order.Add(cartOrder);
               _context.SaveChanges();
               session.SetInt32("cartId",cartOrder.Id);*/
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

            int nLineOrders = countCartLines(session);

            //return RedirectToAction("Index","Cart");

            if (nLineOrders == -1)
            {
                return StatusCode(400);
            }else
            {
                updateOrderNlines(nLineOrders);//$ FULL-SPAGUETTI 
                return StatusCode(200,nLineOrders);
            }
        }

        [HttpGet]
        public ActionResult deleteLine(int? id) //SELECT INSTRUMENT  ON TABLE (PASS ID)
        {
            if(id == null)
                return StatusCode(400); //bad request

            LineOrder linea = _context.LineOrder.Find(id);
            if(linea == null)
                return StatusCode(404); //not found
            
            //return View(linea);

            //tmp until SureDelete View Created:

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
        public Order retrieveCart(ISession session)
        {
            Order cart;
            User user = retrieveUser(); 

            var orders =  _context.Order.Where(o => o.UserId == user.Id);
            if (orders.Where(o => o.State == "Cart").Count() == 0)
            {
                cart = new Order{
                    Date = System.DateTime.Now,
                    nLines = 0,
                    State = "Cart", //Mark order AS CART
                    User = user
                };
                _context.Order.Add(cart);
                _context.SaveChanges();
            }else
            {
                if (orders.Where(o => o.State == "Cart").Count() == 1)
                {
                   cart = orders.Where(o => o.State == "Cart").FirstOrDefault(); 
                }else
                {
                    throw new System.ArgumentException("More that 1 Cart for this user", "original");
                }
            }
            session.SetInt32("cartId",cart.Id);
            return cart;

        }
        public ActionResult getNlinesCart()
        {
            ISession session = _signInManager.Context.Session;
            int nLineOrders = countCartLines(session);

            if (nLineOrders == -1)
            {
                return StatusCode(400,"session var 'cartId' NOT SET on errOn[countCartLines()]"); //bad request
            }
            updateOrderNlines(nLineOrders);//$ FULL-SPAGUETTI 
            return StatusCode(200,nLineOrders);
        }
        public int countCartLines(ISession session)
        {
            int nLines = -1; //cartId null , bad query ...
            var cart = retrieveCart(session);

            int? CartId = session.GetInt32("cartId");
            if (CartId != null)
            {
                nLines = _context.LineOrder.Where(s => s.OrderId == CartId).Count();
            }
            return nLines;
        }
        public void updateOrderNlines(int nlines)
        {
           User user = retrieveUser(); 
           Order order = _context.Order.Where(o => o.UserId == user.Id && o.State == "Cart").FirstOrDefault();
           order.nLines = nlines;
           try
           {
               _context.Order.Update(order);
               _context.SaveChanges();
           }
           catch (System.Exception)
           {
               
               throw;
           }
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