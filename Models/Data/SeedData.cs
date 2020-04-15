using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClaveSol.Data;
using System;
using System.Linq;

namespace ClaveSol.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ClaveSolContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ClaveSolContext>>()))
            {
                // Look for any movies.
                if (context.User.Any())
                {
                    return;   // DB has been seeded
                }

                context.User.AddRange(
                    new User
                    {
                        Name = "John",
                        Surname = "Doe",
                        Mail = "johndoe@mail.com",
                        Premium = true 
                    },
                    new User
                    {
                        Name = "Ana",
                        Surname = "Pérez Martinez",
                        Mail = "anaPerez@mail.com",
                        Premium = false 
                    },
                    new User
                    {
                        Name = "Pepe",
                        Surname = "Smith",
                        Mail = "pepe00@mail.com",
                        Premium = true 
                    },
                    new User
                    {
                        Name = "Kim",
                        Surname = "Morrison",
                        Mail = "kim_morrison@mail.com",
                        Premium = false 
                    }
                );
                context.SaveChanges();
            }
        }
    }
}