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
            SeedAppTables(context);
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

        private static void SeedAppTables(ClaveSolDbContext cnt)
        {
            if (cnt.Attribut.Any() || cnt.Instrument.Any() || cnt.Category.Any() || cnt.SubCategory.Any() ||
                cnt.List.Any() || cnt.Attribut_Ins.Any() || cnt.List_Instrument.Any() || cnt.Shop_Ins.Any()
            )
                return; //already seeded
            
            try
            {
                List<Attribut> attributsList = new List<Attribut>();
                attributsList.Add(new Attribut {Type = "Color",Value = "Rojo"}); //1
                attributsList.Add(new Attribut {Type = "Color",Value = "Verde"}); //2
                attributsList.Add(new Attribut {Type = "Color",Value = "Azul"}); //3
                attributsList.Add(new Attribut {Type = "Material",Value = "Madera"}); //4
                attributsList.Add(new Attribut {Type = "Material",Value = "Plástico"}); //5
                attributsList.Add(new Attribut {Type = "Material",Value = "Metal"}); //6
                attributsList.Add(new Attribut {Type = "Accesorio",Value = "Set Puas"}); //7
                attributsList.Add(new Attribut {Type = "Accesorio",Value = "Pedal Distorsión"}); //8
                cnt.Attribut.AddRange(attributsList);
                cnt.SaveChanges(); 

                List<Category> CatList = new List<Category>();
                CatList.Add(new Category {Name="Viento"});
                CatList.Add(new Category {Name="Cuerda"});
                CatList.Add(new Category {Name="Percusión"});
                cnt.Category.AddRange(CatList);
                cnt.SaveChanges(); 

                List<SubCategory> SubCatList = new List<SubCategory>();
                SubCatList.Add(new SubCategory {Name="Flautas",CategoryId=1});
                SubCatList.Add(new SubCategory {Name="Armonicas",CategoryId=1});
                SubCatList.Add(new SubCategory {Name="Guitarras",CategoryId=2});
                SubCatList.Add(new SubCategory {Name="Pianos",CategoryId=3});
                SubCatList.Add(new SubCategory {Name="Oboes",CategoryId=1});
                SubCatList.Add(new SubCategory {Name="Baterias",CategoryId=3});
                cnt.SubCategory.AddRange(SubCatList);
                cnt.SaveChanges(); 

                List<Instrument> insList = new List<Instrument>();
                insList.Add(new Instrument {Name="Flauta India",Brand="Native",Price=200,State="Disponible",SubCategoryId=1,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Armonica X",Brand="FXX",Price=50,State="Disponible",SubCategoryId=2,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Guitarra Andaluza X",Brand="El Cordobes",Price=300,State="Disponible",SubCategoryId=3,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Piano Y",Brand="Yamaha",Price=1000,State="Disponible",SubCategoryId=4,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Electric 900",Brand="Hendrix",Price=660,State="Disponible",SubCategoryId=3,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Oboe RK",Brand="Obs",Price=850,State="Disponible",SubCategoryId=5,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Bateria 10T",Brand="FXX",Price=700,State="Disponible",SubCategoryId=6,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                cnt.Instrument.AddRange(insList);
                cnt.SaveChanges(); 

                List<List> Lists = new List<List>();
                Lists.Add(new List {Name="Deseos",Deleted=false,UserId=1});
                Lists.Add(new List {Name="Deseos",Deleted=false,UserId=2});
                Lists.Add(new List {Name="Deseos",Deleted=false,UserId=3});
                cnt.SubCategory.AddRange(SubCatList);
                cnt.SaveChanges(); 

                List<Shop> Shops = new List<Shop>();
                Shops.Add(new Shop {City="Alicante"});
                Shops.Add(new Shop {City="Valencia"});
                Shops.Add(new Shop {City="Murcia"});
                cnt.SubCategory.AddRange(SubCatList);
                cnt.SaveChanges(); 

                // N-N TABLES

                List<Attribut_Ins> Attr_Inss = new List<Attribut_Ins>();
                    //cosas rojas
                Attr_Inss.Add(new Attribut_Ins {AttributId=1,InstrumentId=1});
                Attr_Inss.Add(new Attribut_Ins {AttributId=1,InstrumentId=2});
                Attr_Inss.Add(new Attribut_Ins {AttributId=1,InstrumentId=4});
                    //cosas verdes
                Attr_Inss.Add(new Attribut_Ins {AttributId=2,InstrumentId=2});
                Attr_Inss.Add(new Attribut_Ins {AttributId=2,InstrumentId=5});
                Attr_Inss.Add(new Attribut_Ins {AttributId=2,InstrumentId=7});
                    //cosas azules
                Attr_Inss.Add(new Attribut_Ins {AttributId=3,InstrumentId=2});
                Attr_Inss.Add(new Attribut_Ins {AttributId=3,InstrumentId=5});
                Attr_Inss.Add(new Attribut_Ins {AttributId=3,InstrumentId=7});  
                    //cosas de madera
                Attr_Inss.Add(new Attribut_Ins {AttributId=4,InstrumentId=1});
                Attr_Inss.Add(new Attribut_Ins {AttributId=4,InstrumentId=2});
                Attr_Inss.Add(new Attribut_Ins {AttributId=4,InstrumentId=3});
                Attr_Inss.Add(new Attribut_Ins {AttributId=4,InstrumentId=4});
                Attr_Inss.Add(new Attribut_Ins {AttributId=4,InstrumentId=6});
                    //cosas de plastico
                Attr_Inss.Add(new Attribut_Ins {AttributId=5,InstrumentId=2});
                Attr_Inss.Add(new Attribut_Ins {AttributId=5,InstrumentId=5});
                Attr_Inss.Add(new Attribut_Ins {AttributId=5,InstrumentId=7});
                    //cosas de metal
                Attr_Inss.Add(new Attribut_Ins {AttributId=6,InstrumentId=2});
                Attr_Inss.Add(new Attribut_Ins {AttributId=6,InstrumentId=5});
                Attr_Inss.Add(new Attribut_Ins {AttributId=6,InstrumentId=6});
                Attr_Inss.Add(new Attribut_Ins {AttributId=6,InstrumentId=7});
                    //con set puas
                Attr_Inss.Add(new Attribut_Ins {AttributId=7,InstrumentId=3});
                Attr_Inss.Add(new Attribut_Ins {AttributId=7,InstrumentId=5});
                    //con set puas
                Attr_Inss.Add(new Attribut_Ins {AttributId=8,InstrumentId=5});
                Attr_Inss.Add(new Attribut_Ins {AttributId=8,InstrumentId=7});
                cnt.Attribut_Ins.AddRange(Attr_Inss);
                cnt.SaveChanges(); 

                List<List_Instrument> list_ListIns = new List<List_Instrument>();
                list_ListIns.Add(new List_Instrument {ListId=1,InstrumentId=1});
                list_ListIns.Add(new List_Instrument {ListId=1,InstrumentId=2});
                list_ListIns.Add(new List_Instrument {ListId=1,InstrumentId=3});
                list_ListIns.Add(new List_Instrument {ListId=2,InstrumentId=4});
                list_ListIns.Add(new List_Instrument {ListId=2,InstrumentId=5});
                list_ListIns.Add(new List_Instrument {ListId=2,InstrumentId=6});
                list_ListIns.Add(new List_Instrument {ListId=3,InstrumentId=7});
                list_ListIns.Add(new List_Instrument {ListId=3,InstrumentId=1});
                list_ListIns.Add(new List_Instrument {ListId=3,InstrumentId=5});
                cnt.List_Instrument.AddRange(list_ListIns);
                cnt.SaveChanges(); 

                List<Shop_Ins> list_ShopIns = new List<Shop_Ins>();
                list_ShopIns.Add(new Shop_Ins {ShopId=1,InstrumentId=1});
                list_ShopIns.Add(new Shop_Ins {ShopId=1,InstrumentId=2});
                list_ShopIns.Add(new Shop_Ins {ShopId=1,InstrumentId=3});
                list_ShopIns.Add(new Shop_Ins {ShopId=1,InstrumentId=4});
                list_ShopIns.Add(new Shop_Ins {ShopId=1,InstrumentId=5});
                list_ShopIns.Add(new Shop_Ins {ShopId=1,InstrumentId=6});
                list_ShopIns.Add(new Shop_Ins {ShopId=1,InstrumentId=7});
                cnt.Shop_Ins.AddRange(list_ShopIns);
                cnt.SaveChanges(); 

            } //1.FLAUTA 2.ARMONICA 3.ANDALUZA 4.PIANO 5.ELECTRICA 6.OBOE 7.BATERIA
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}


