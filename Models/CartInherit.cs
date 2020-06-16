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
    public class CartInherit : Order
    {
        private readonly ClaveSolDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private Order order {get; set;}
        private CartInherit(ClaveSolDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;

            //init
            var claimUser = _signInManager.Context.User;
            string ideUserId = _signInManager.UserManager.GetUserId(claimUser);
            User user = _context.User.Where(x => x.OwnerID == ideUserId).FirstOrDefault();

            order = new Order{
                Date = System.DateTime.Now,
                nLines = 0,
                State = "Cart",
                User = user
            };
            Id = order.Id;

        }

        public CartInherit GetCartOrd()
        {
            ISession session = _signInManager.Context.Session;

            string carTid = session.GetString("CartId") ?? Guid.NewGuid().ToString(); 
            session.SetString("CartId",carTid);

            return new CartInherit(_context, _signInManager){Id = order.Id};
        }
    }
}