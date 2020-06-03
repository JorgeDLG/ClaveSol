using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClaveSol.Data;
using ClaveSol.Models;

namespace ClaveSol.Models
{
    public class Cart
    {
        private readonly ClaveSolDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        private Cart(ClaveSolDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public string cartId {get; set;}
        public List<LineOrder> LineOrders {get; set;}

        public Cart GetCart()
        {
            ISession session = _signInManager.Context.Session;
            var context = _context;

            string carTid = session.GetString("CartId") ?? Guid.NewGuid().ToString(); 
            session.SetString("CartId",carTid);

            return new Cart(context, _signInManager){cartId = carTid};
        }

        public void addToCart(Instrument instrument, int amount = 1)
        {
            LineOrder lineOr = GetLineOrder(instrument); 

            if (lineOr == null)
            {
               lineOr = new LineOrder //Order,OrderId no set (link when pay?)
               {
                   Name = instrument.Name,
                   Quantity = amount,
                   UnitaryPrice = instrument.Price,
                   TotalPrice = instrument.Price,
                   IntrumentId = instrument.Id,
                   Instrument = instrument
               };
               _context.LineOrder.Add(lineOr); //No store on db? volatile LineOrders until Pay?

            }else
            {
                lineOr.Quantity++;
            }
            _context.SaveChanges();
            //LineOrders.Add(lineOr);
        }

        public int removeFromCart(Instrument instrument)
        {
            LineOrder lineOr = GetLineOrder(instrument); 

            int localAmount = 0;

            if (lineOr != null)
            {
                if (lineOr.Quantity > 1)
                {
                    lineOr.Quantity--;
                    localAmount = lineOr.Quantity;
                }
                else
                {
                    _context.LineOrder.Remove(lineOr);
                }
            }
            _context.SaveChanges();
            return localAmount;
        }

        // public List<LineOrder> GetLineOrders()
        // {
        //     return LineOrders ??

        // }
        public LineOrder GetLineOrder(Instrument ins)
        {
            var lineOrds = from lOrds in _context.LineOrder
                                    select lOrds;

            return lineOrds.Where(s => s.IntrumentId == ins.Id).FirstOrDefault();
        }

    }
}