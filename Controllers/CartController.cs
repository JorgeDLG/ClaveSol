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

            //ViewBag.cartId = cartId;
            ViewBag.totalOrderPrice = updateTotalOrderPrice(lineas.ToList());

            // ViewBag.allAttr = from attr in _context.Attribut
            //     join attIns in _context.Attribut_Ins on attr.Id equals attIns.AttributId
            //     select attr;
            ViewBag.allAttr = _context.Attribut;
            //ViewBag.allAttrIns = _context.Attribut_Ins.ToList();

            return View(lineas.ToList());
        }
        public ActionResult addToCart(int id,string attribsValues=null)
        {
            ISession session = _signInManager.Context.Session;
            //OPTIMIZA Y TESTEA ESTA SHIT DE CODIGO !!
            Order cartOrder = retrieveCart(session);

            // if(session.GetInt32("cartId") == null) 
            // {
            //     cartOrder = retrieveCart(session);
            // }

            Instrument instrument = _context.Instrument.Find(id);

            LineOrder lineOr = GetLineOrder(id,cartOrder); //lineOr NULL

            try
            {
                if (lineOr != null && instrument == lineOr.Instrument) //lineOr.Instrument NULL
                {
                   lineOr.Quantity++; 
                   //UPDATE LINE ORDER ON DB
                   _context.LineOrder.Update(lineOr);
                   _context.SaveChanges();
                }else
                {
                    lineOr = new LineOrder{
                       Order = cartOrder,  
                       OrderId = (int)session.GetInt32("cartId"),
                       Instrument = instrument,
                       InstrumentId = instrument.Id,
                       Name = instrument.Name,
                       Quantity = 1,
                       UnitaryPrice = instrument.Price,
                       TotalPrice = instrument.Price
                    };

                    //Adding attribs to LineOrder
                    if (String.IsNullOrEmpty(attribsValues)) //Attribs passed
                    {   //Default Attribs
                        attribsValues = getDefValuesEachType(lineOr.InstrumentId);
                    }

                    insertAttribsIds(attribsValues,lineOr);

                    _context.LineOrder.Add(lineOr);
                    _context.SaveChanges();
                }
            }
            catch (System.Exception){throw;}


            int nLineOrders = countCartLines(session);

            //return RedirectToAction("Index","Cart");

            if (nLineOrders == -1)
            {
                return StatusCode(400);
            }else
            {
                updateOrder(nLineOrders);//$ FULL-SPAGUETTI 
                return StatusCode(200,nLineOrders);
            }
        }
        public string getDefValuesEachType(int? insId) //Stablish default Attr.Values (as string comma separated), depending the Instrument
        {
            string defValues = null;
            switch (insId)
            {
                case 1: //Bateria
                    defValues = "Azul,Plastico,Pedal Distorsion";
                break;
                case 2: //Oboe
                    defValues = "Madera";
                break;
                case 3: //Electric
                    defValues = "Azul,Plastico,Set Puas";
                break;
                case 4: //Piano
                    defValues = "Rojo,Madera";
                break;
                case 5: //G.Andaluza
                    defValues = "Madera,Set Puas";
                break;
                case 6: //Armonica
                    defValues = "Azul,Madera";
                break;
                case 7: //Flauta
                    defValues = "Rojo,Madera";
                break;
                default:
                    defValues = null;
                break;
            }
            return defValues;
        }
        public void insertAttribsIds(string attribsValues,LineOrder line) //attribsValues input "Rojo,Madera,Set Puas"
        {
            //attribsValues to AttribsId comma separated
            string attribsIds = "";
            string[] attsValues = attribsValues.Split(','); 

            foreach (var value in attsValues)
            {
                string attId = _context.Attribut.Where(a => a.Value == value).FirstOrDefault().Id.ToString();
                attribsIds += attId + ","; 
            }
            attribsIds = attribsIds.Remove(attribsIds.Length-1); //deleting last ',' 

            //set line.Attribs = attribsId
            line.AtributsId = attribsIds; //Attributes Id's comma separated for lineOrder
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

            //tmp until SureDelete View Created:

            _context.LineOrder.Remove(linea);
            _context.SaveChanges();

            ISession session = _signInManager.Context.Session;
            int nLineOrders = countCartLines(session);

            //return RedirectToAction("Index","Cart");

            if (nLineOrders == -1)
            {
                return StatusCode(400);
            }else
            {
                updateOrder(nLineOrders);//$ FULL-SPAGUETTI 
                return StatusCode(200,nLineOrders);
                //return RedirectToAction("Index","Cart");
            }
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

            try
            {
                var orders =  _context.Order.Where(o => o.UserId == user.Id);
                if (orders == null || orders.Where(o => o.State == "Cart").Count() == 0)
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
            catch (System.Exception){throw;}
        }
        public ActionResult getNlinesCart()
        {
            ISession session = _signInManager.Context.Session;
            int nLineOrders = countCartLines(session);

            if (nLineOrders == -1)
            {
                return StatusCode(400,"session var 'cartId' NOT SET on errOn[countCartLines()]"); //bad request
            }
            updateOrder(nLineOrders);//$ FULL-SPAGUETTI 
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
        public void updateOrder(int nlines)
        {
           User user = retrieveUser(); 
           Order order = _context.Order.Where(o => o.UserId == user.Id && o.State == "Cart").FirstOrDefault();
           order.nLines = nlines;

           var lineOrList = _context.LineOrder.Where(l => l.OrderId == order.Id).ToList();

            //BUG:LINEOR.TOTALPRICE NO TIENE EN CUENTA EL MULTIPLIACDOR POR QUANTITY
            // Guarda el TotalPrice bien con *Quantity
           //decimal totalOrderPrice = lineOrList.Sum(l => l.TotalPrice);  
           //order.Price = totalOrderPrice;
           ViewBag.totalOrderPrice = updateTotalOrderPrice(lineOrList);

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
        public decimal updateTotalOrderPrice(List<LineOrder> lines)
        {
            if (lines.Count == 0)
            {
                ISession session = _signInManager.Context.Session;
                Order ordeR = retrieveCart(session);
                ordeR.Price = 0;
                _context.Order.Update(ordeR);
                _context.SaveChanges();
                return 0;
                
            }
            List<decimal> linesTotalPrices = new List<decimal>();
            foreach (var line in lines)
            {
               line.TotalPrice = line.Quantity * line.UnitaryPrice;
               _context.LineOrder.Update(line);
               linesTotalPrices.Add(line.TotalPrice); 
            }

            decimal totalOrderPrice = linesTotalPrices.Sum();
            var order = _context.Order.Find(lines[0].OrderId); //CRASH null when lines=0
            order.Price = totalOrderPrice;
            _context.Order.Update(order);
            _context.SaveChanges();

            return order.Price;
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
        public LineOrder GetLineOrder(int insId, Order cartOrder)
        {
            var lineOrds = from lOrds in _context.LineOrder
                                    select lOrds;

            return lineOrds.Where(lo => lo.InstrumentId == insId && lo.OrderId == cartOrder.Id).FirstOrDefault();
        }
        public ActionResult emptyCart()
        {
            return View();
        }

        public ActionResult quantityBy1(int? id,int operation) 
        {
            ISession session = _signInManager.Context.Session;
            Order cartOrder = retrieveCart(session);
            Instrument instrument = _context.Instrument.Find(id);

            //LineOrder lineOr = GetLineOrder((int)id,cartOrder); //lineOr NULL
            LineOrder lineOr = _context.LineOrder.Find(id);


            if (operation < 0)
                lineOr.Quantity--;
            else
                lineOr.Quantity++;

            _context.LineOrder.Update(lineOr);
            _context.SaveChanges();            

            int nLineOrders = countCartLines(session);

            if (nLineOrders == -1)
            {
                return StatusCode(400);
            }else
            {
                updateOrder(nLineOrders);//$ FULL-SPAGUETTI 
                return StatusCode(200,nLineOrders);
            }
        }
    }
}