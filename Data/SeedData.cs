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
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<AppDbContext>>()))
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

                    },
                    new User
                    {
                        Name = "Paco",
                        Surname = "Perez",
                        Mail = "paco@mail.com",
                        Premium = false,
                    },
                    new User
                    {
                        Name = "Mario",
                        Surname = "Garcia",
                        Mail = "mario@mail.com",
                        Premium = true,
                    },
                    new User
                    {
                        Name = "Arturo",
                        Surname = "Sanchez",
                        Mail = "arturo@mail.com",
                        Premium = true,
                    }
                );
                context.SaveChanges();
            }
        }
    }

}


