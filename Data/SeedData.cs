using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClaveSol.Models;
using System;
using System.Collections;
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

            string[,] adminUsers = new string[1, 4];
            var admin = await EnsureUser(serviceProvider, testUserPw, "admin@mail.com");
            await EnsureRole(serviceProvider, admin.Id, "admin"/*Constants.ContactAdministratorsRole*/);

            var context = new AppDbContext(serviceProvider.
            GetRequiredService<DbContextOptions<AppDbContext>>());

            adminUsers[0, 0] = "admin";
            adminUsers[0, 1] = admin.UserName;//mail
            adminUsers[0, 2] = "False";
            adminUsers[0, 3] = admin.Id;

            SeedAppDB(context, adminUsers);

            //Loop 4 NormalUsers(Identity) generation,role addition & linked/generated to AppUsers. 
            string[] userNamesSeed = { "ana", "paco", "mario", "arturo" };
            string[,] normalUsers = null;
            //foreach (var uSeed in userNamesSeed)
            //{
            //    var normal = await EnsureUser(serviceProvider, uSeed + "123", $"{uSeed}@mail.com");
            //    await EnsureRole(serviceProvider, normal.Id, "normal"/*Constants.ContactManagersRole*/);
            //}
            for (int i = 0; i < userNamesSeed.Length; i++)
            {
                var normal = await EnsureUser(serviceProvider,
                     userNamesSeed[i] + "123", $"{userNamesSeed[i]}@mail.com");
                await EnsureRole(serviceProvider, normal.Id, "normal"/*Constants.ContactManagersRole*/);

                normalUsers[i, 0] = userNamesSeed[i];
                normalUsers[i, 1] = $"{userNamesSeed[i]}@mail.com";
                normalUsers[i, 2] = "False";
                normalUsers[i, 3] = normal.Id;
            }
            SeedAppDB(context, normalUsers);
        }
        private static async Task<appIdentityUser> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<appIdentityUser>>();

            appIdentityUser user = null;
            try
            {
                user = await userManager.FindByNameAsync(UserName);
            }
            catch (System.Exception)
            {
                throw;
            }

            if (user == null)
            {
                user = new appIdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                    //FullName ?
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }
            return user;
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

            try
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    IR = await roleManager.CreateAsync(new appIdentityRole(role));
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            var userManager = serviceProvider.GetService<UserManager<appIdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        public static void SeedAppDB(AppDbContext context, string[,] IdentityUsers)
        {
            // Look for any movies.
            if (context.User.Any())
            {
                return;   // DB has been seeded
            }

            //User[] AppUsers = new User[IdentityUsers.Count];

            //for (int i = 0; i < AppUsers.Length; i++)
            //{
            //   AppUsers[i].Name = IdentityUsers[i].UserName; 
            //}
            //Seeding User table (ApDbContext)
            try
            {
                for (int i = 0; i < IdentityUsers.GetLength(0); i++)
                {
                    context.User.Add(
                        new User
                        {
                            Name = IdentityUsers[i, 0],
                            Surname = "Perez",
                            Mail = IdentityUsers[i, 1],
                            Premium = Convert.ToBoolean(IdentityUsers[i, 2]),
                            OwnerID = IdentityUsers[i, 3]
                        }
                    );
                }

            }
            catch (System.Exception)
            {
                throw;
            }

            //context.User.AddRange(
            //    new User
            //    {
            //        Name = "Ana",
            //        Surname = "Perez",
            //        Mail = "ana@mail.com",
            //        Premium = false,
            //        //OwnerID = IdentityID

            //    },
            //    new User
            //    {
            //        Name = "Paco",
            //        Surname = "Perez",
            //        Mail = "paco@mail.com",
            //        Premium = false,
            //        //OwnerID = IdentityID
            //    },
            //    new User
            //    {
            //        Name = "Mario",
            //        Surname = "Garcia",
            //        Mail = "mario@mail.com",
            //        Premium = true,
            //        //OwnerID = IdentityID
            //    },
            //    new User
            //    {
            //        Name = "Arturo",
            //        Surname = "Sanchez",
            //        Mail = "arturo@mail.com",
            //        Premium = true,
            //        //OwnerID = IdentityID
            //    }
            //);
            context.SaveChanges();
        }
    }

}


