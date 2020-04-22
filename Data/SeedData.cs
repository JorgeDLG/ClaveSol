using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClaveSol.Models;
using System;
using System.Linq;

namespace ClaveSol.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ClaveSolDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ClaveSolDbContext>>()))
            {
                // Look for any movies.
                if (context.User.Any())
                {
                    return;   // DB has been seeded
                }

                context.User.AddRange(
                    new User
                    {
                        Name = "Ana",
                        Surname = "Perez",
                        Mail = "ana@mail.com",
                        Premium = false,

                        //NOTA El ID en IdentityDB es un Hash! OwnerID = mail?

                        OwnerID = "1"
                    },
                    new User
                    {
                        Name = "Paco",
                        Surname = "Perez",
                        Mail = "paco@mail.com",
                        Premium = false,
                        OwnerID = "2"
                    },
                    new User
                    {
                        Name = "Mario",
                        Surname = "Garcia",
                        Mail = "mario@mail.com",
                        Premium = true,
                        OwnerID = "3"
                    },
                    new User
                    {
                        Name = "Arturo",
                        Surname = "Sanchez",
                        Mail = "arturo@mail.com",
                        Premium = true,
                        OwnerID = "4"
                    }
                );
                context.SaveChanges();
            }

            /*using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.AspNetUsers.Any())
                {
                    return;   // DB has been seeded
                }

                context.User.AddRange(
                    new User
                    {
                        Name = "Ana",
                        Surname = "Perez",
                        Mail = "ana@mail.com",
                        Premium = false,
                        OwnerID = "1"
                    },
                    new User
                    {
                        Name = "Paco",
                        Surname = "Perez",
                        Mail = "paco@mail.com",
                        Premium = false,
                        OwnerID = "2"
                    },
                    new User
                    {
                        Name = "Mario",
                        Surname = "Garcia",
                        Mail = "mario@mail.com",
                        Premium = true,
                        OwnerID = "3"
                    },
                    new User
                    {
                        Name = "Arturo",
                        Surname = "Sanchez",
                        Mail = "arturo@mail.com",
                        Premium = true,
                        OwnerID = "4"
                    }
                );
                context.SaveChanges();
            }*/
        }
    }

}


