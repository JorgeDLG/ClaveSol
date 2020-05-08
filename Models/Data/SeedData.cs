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
{//UPDATE: Seed Users(ClaveSolDbContext) & Create Identity Users and Roles LINKING with it.
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

            var context = new ClaveSolDbContext(serviceProvider.
            GetRequiredService<DbContextOptions<ClaveSolDbContext>>());

            adminUsers[0, 0] = "admin";
            adminUsers[0, 1] = admin.UserName;//mail
            adminUsers[0, 2] = "False";
            adminUsers[0, 3] = admin.Id;

            SeedAppDB(context, adminUsers);

            //Loop 4 NormalUsers(Identity) generation,role addition & linked/generated to AppUsers. 
            string[] userNamesSeed = { "ana", "paco", "mario", "arturo" };
            string[,] normalUsers = new string[userNamesSeed.Length,4];
            for (int i = 0; i < userNamesSeed.Length; i++)
            {
                var normal = await EnsureUser(serviceProvider,
                     testUserPw, $"{userNamesSeed[i]}@mail.com");
                await EnsureRole(serviceProvider, normal.Id, "normal"/*Constants.ContactManagersRole*/);

                normalUsers[i, 0] = userNamesSeed[i];
                normalUsers[i, 1] = $"{userNamesSeed[i]}@mail.com";
                normalUsers[i, 2] = "False";
                normalUsers[i, 3] = normal.Id;
            }
            SeedAppDB(context, normalUsers);
        }
        private static async Task<IdentityUser> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            IdentityUser user = null;
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
                user = new IdentityUser
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
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            try
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    IR = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            catch (System.Exception)
            {
                throw;
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
        public static void SeedAppDB(ClaveSolDbContext context, string[,] IdentityUsers)
        {
            if (context.User.Any())
            {
                if (IdentityUsers[0,0] != "ana" && context.User.Count() != 1 || context.User.Count() >= 5) // last || prescindible?
                {
                    return;   // DB has been seeded
                }
            }

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

            context.SaveChanges();
        }
    }

}


