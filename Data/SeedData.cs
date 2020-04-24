using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClaveSol.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ClaveSol.Security;

namespace ClaveSol.Data
{//UPDATE: Seed Users(AppDbContext) & Create Identity Users and Roles LINKING with it.
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            // For sample purposes seed both with the same password.
            // Password is set with the following:
            // dotnet user-secrets set SeedUserPW <pw>
            // The admin user can do anything

            //Identity following this tutorial: https://bit.ly/3cKxRXz

            var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@mail.com");
            await EnsureRole(serviceProvider, adminID, "admin"/*Constants.ContactAdministratorsRole*/);

            var context = new AppDbContext(serviceProvider.
            GetRequiredService<DbContextOptions<AppDbContext>>());

            SeedAppDB(context, adminID);


            //Loop 4 NormalUsers(Identity) generation,role addition & linked/generated to AppUsers. 
            var normalID = await EnsureUser(serviceProvider, testUserPw, "normal@mail.com");
            await EnsureRole(serviceProvider, normalID, "normal"/*Constants.ContactManagersRole*/);

            SeedAppDB(context, normalUsers);

        }

        //Return User for push it to array?
        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<appIdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new appIdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                    //FullName ?
                };
                await userManager.CreateAsync(user, testUserPw);

                //Link with AppUser?
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<appIdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new appIdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        public static void SeedAppDB(AppDbContext context, string IdentityID)
        {
            // Look for any movies.
            if (context.User.Any())
            {
                return;   // DB has been seeded
            }

            //Seeding User table (ApDbContext)
            context.User.AddRange(
                new User
                {
                    Name = "Ana",
                    Surname = "Perez",
                    Mail = "ana@mail.com",
                    Premium = false,
                    OwnerID = IdentityID

                },
                new User
                {
                    Name = "Paco",
                    Surname = "Perez",
                    Mail = "paco@mail.com",
                    Premium = false,
                    OwnerID = IdentityID
                },
                new User
                {
                    Name = "Mario",
                    Surname = "Garcia",
                    Mail = "mario@mail.com",
                    Premium = true,
                    OwnerID = IdentityID
                },
                new User
                {
                    Name = "Arturo",
                    Surname = "Sanchez",
                    Mail = "arturo@mail.com",
                    Premium = true,
                    OwnerID = IdentityID
                }
            );
            context.SaveChanges();
        }
    }

}


