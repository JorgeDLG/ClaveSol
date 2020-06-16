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
                cnt.List.Any() || cnt.Attribut_Ins.Any() || cnt.List_Instrument.Any() || cnt.Shop_Ins.Any() ||
                cnt.LineOrder.Any() || cnt.Order.Any()
            )
                return; //already seeded
            
            try
            {
                List<Attribut> attributsList = new List<Attribut>();
                attributsList.Add(new Attribut {Type = "Color",Value = "Rojo"}); //1
                attributsList.Add(new Attribut {Type = "Color",Value = "Verde"}); //2
                attributsList.Add(new Attribut {Type = "Color",Value = "Azul"}); //3
                attributsList.Add(new Attribut {Type = "Material",Value = "Madera"}); //4
                attributsList.Add(new Attribut {Type = "Material",Value = "Plastico"}); //5
                attributsList.Add(new Attribut {Type = "Material",Value = "Metal"}); //6
                attributsList.Add(new Attribut {Type = "Accesorio",Value = "Set Puas"}); //7
                attributsList.Add(new Attribut {Type = "Accesorio",Value = "Pedal Distorsion"}); //8
                cnt.Attribut.AddRange(attributsList);
                cnt.SaveChanges(); 

                List<Category> CatList = new List<Category>();
                CatList.Add(new Category {Name="Viento"});
                CatList.Add(new Category {Name="Cuerda"});
                CatList.Add(new Category {Name="Percusion"});
                cnt.Category.AddRange(CatList);
                cnt.SaveChanges(); 

                int idCatViento = getCatIdByName("Viento");
                int idCatCuerda = getCatIdByName("Cuerda");
                int idCatPercusion = getCatIdByName("Percusion");

                List<SubCategory> SubCatList = new List<SubCategory>();
                SubCatList.Add(new SubCategory {Name="Flautas",CategoryId=idCatViento});
                SubCatList.Add(new SubCategory {Name="Armonicas",CategoryId=idCatViento});
                SubCatList.Add(new SubCategory {Name="Guitarras",CategoryId=idCatCuerda});
                SubCatList.Add(new SubCategory {Name="Pianos",CategoryId=idCatPercusion});
                SubCatList.Add(new SubCategory {Name="Oboes",CategoryId=idCatViento});
                SubCatList.Add(new SubCategory {Name="Baterias",CategoryId=idCatPercusion});
                cnt.SubCategory.AddRange(SubCatList);
                cnt.SaveChanges(); 

                int idSubCatFlautas = getSubCatIdByName("Flautas");
                int idSubCatArmonicas = getSubCatIdByName("Armonicas");
                int idSubCatGuitarras = getSubCatIdByName("Guitarras");
                int idSubCatPianos = getSubCatIdByName("Pianos");
                int idSubCatOboes = getSubCatIdByName("Oboes");
                int idSubCatBaterias = getSubCatIdByName("Baterias");

                List<Instrument> insList = new List<Instrument>();
                insList.Add(new Instrument {Name="Flauta India",Brand="Native",Price=200,State="Disponible",SubCategoryId=idSubCatFlautas,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Armonica X",Brand="FXX",Price=50,State="Disponible",SubCategoryId=idSubCatArmonicas,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Guitarra Andaluza X",Brand="El Cordobes",Price=300,State="Disponible",SubCategoryId=idSubCatGuitarras,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Piano Y",Brand="Yamaha",Price=1000,State="Disponible",SubCategoryId=idSubCatPianos,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Electric 900",Brand="Hendrix",Price=660,State="Disponible",SubCategoryId=idSubCatGuitarras,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Oboe RK",Brand="Obs",Price=850,State="Disponible",SubCategoryId=idSubCatOboes,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                insList.Add(new Instrument {Name="Bateria 10T",Brand="FXX",Price=700,State="Disponible",SubCategoryId=idSubCatBaterias,Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book."});
                cnt.Instrument.AddRange(insList);
                cnt.SaveChanges(); 

                List<List> ListList = new List<List>();
                ListList.Add(new List {Name="Deseos",Deleted=false,UserId=1});
                ListList.Add(new List {Name="Deseos",Deleted=false,UserId=2});
                ListList.Add(new List {Name="Deseos",Deleted=false,UserId=3});
                cnt.List.AddRange(ListList);
                cnt.SaveChanges(); 

                List<Shop> ShopList = new List<Shop>();
                ShopList.Add(new Shop {City="Alicante"});
                ShopList.Add(new Shop {City="Valencia"});
                ShopList.Add(new Shop {City="Murcia"});
                cnt.Shop.AddRange(ShopList);
                cnt.SaveChanges(); 

                // N-N TABLES

                int idAttRed = getAttributeIdByValue("Rojo");
                int idAttGreen = getAttributeIdByValue("Verde");
                int idAttBlue = getAttributeIdByValue("Azul");
                int idAttMadera = getAttributeIdByValue("Madera");
                int idAttPlastico = getAttributeIdByValue("Plastico");
                int idAttMetal = getAttributeIdByValue("Metal");
                int idAttPuas = getAttributeIdByValue("Set Puas");
                int idAttPedal = getAttributeIdByValue("Pedal Distorsion");

                int idInsFlauta = getInstrumIdByName("Flauta India");
                int idInsArmonica = getInstrumIdByName("Armonica X");
                int idInsAndaluza = getInstrumIdByName("Guitarra Andaluza X");
                int idInsPiano = getInstrumIdByName("Piano Y");
                int idInsElectric = getInstrumIdByName("Electric 900");
                int idInsOboe = getInstrumIdByName("Oboe RK");
                int idInsBateria = getInstrumIdByName("Bateria 10T");

                int idShopAlicante = getShopIdByCity("Alicante");
                int idShopValencia = getShopIdByCity("Valencia");
                int idShopMurcia = getShopIdByCity("Murcia");

                List<Attribut_Ins> Attr_Inss = new List<Attribut_Ins>();
                    //cosas rojas
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttRed,InstrumentId=idInsFlauta});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttRed,InstrumentId=idInsArmonica});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttRed,InstrumentId=idInsPiano});
                    //cosas verdes
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttGreen,InstrumentId=idInsArmonica});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttGreen,InstrumentId=idInsElectric});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttGreen,InstrumentId=idInsBateria});
                    //cosas azules
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttBlue,InstrumentId=idInsArmonica});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttBlue,InstrumentId=idInsElectric});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttBlue,InstrumentId=idInsBateria});  
                    //cosas de madera
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttMadera,InstrumentId=idInsFlauta});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttMadera,InstrumentId=idInsArmonica});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttMadera,InstrumentId=idInsAndaluza});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttMadera,InstrumentId=idInsPiano});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttMadera,InstrumentId=idInsOboe});
                    //cosas de plastico
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttPlastico,InstrumentId=idInsArmonica});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttPlastico,InstrumentId=idInsElectric});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttPlastico,InstrumentId=idInsBateria});
                    //cosas de metal
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttMetal,InstrumentId=idInsArmonica});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttMetal,InstrumentId=idInsElectric});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttMetal,InstrumentId=idInsOboe});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttMetal,InstrumentId=idInsBateria});
                    //con set puas
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttPuas,InstrumentId=idInsAndaluza});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttPuas,InstrumentId=idInsElectric});
                    //con pedal d. 
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttPedal,InstrumentId=idInsElectric});
                Attr_Inss.Add(new Attribut_Ins {AttributId=idAttPedal,InstrumentId=idInsBateria});
                cnt.Attribut_Ins.AddRange(Attr_Inss);
                cnt.SaveChanges(); 

                List<List_Instrument> list_ListIns = new List<List_Instrument>();
                list_ListIns.Add(new List_Instrument {ListId=1,InstrumentId=idInsFlauta});
                list_ListIns.Add(new List_Instrument {ListId=1,InstrumentId=idInsArmonica});
                list_ListIns.Add(new List_Instrument {ListId=1,InstrumentId=idInsAndaluza});
                list_ListIns.Add(new List_Instrument {ListId=2,InstrumentId=idInsPiano});
                list_ListIns.Add(new List_Instrument {ListId=2,InstrumentId=idInsElectric});
                list_ListIns.Add(new List_Instrument {ListId=2,InstrumentId=idInsOboe});
                list_ListIns.Add(new List_Instrument {ListId=3,InstrumentId=idInsBateria});
                list_ListIns.Add(new List_Instrument {ListId=3,InstrumentId=idInsFlauta});
                list_ListIns.Add(new List_Instrument {ListId=3,InstrumentId=idInsElectric});
                cnt.List_Instrument.AddRange(list_ListIns);
                cnt.SaveChanges(); 

                List<Shop_Ins> list_ShopIns = new List<Shop_Ins>();
                list_ShopIns.Add(new Shop_Ins {ShopId=idShopAlicante,InstrumentId=idInsFlauta});
                list_ShopIns.Add(new Shop_Ins {ShopId=idShopAlicante,InstrumentId=idInsArmonica});
                list_ShopIns.Add(new Shop_Ins {ShopId=idShopAlicante,InstrumentId=idInsAndaluza});
                list_ShopIns.Add(new Shop_Ins {ShopId=idShopAlicante,InstrumentId=idInsPiano});
                list_ShopIns.Add(new Shop_Ins {ShopId=idShopAlicante,InstrumentId=idInsElectric});
                list_ShopIns.Add(new Shop_Ins {ShopId=idShopAlicante,InstrumentId=idInsOboe});
                list_ShopIns.Add(new Shop_Ins {ShopId=idShopAlicante,InstrumentId=idInsBateria});
                cnt.Shop_Ins.AddRange(list_ShopIns);
                cnt.SaveChanges(); 

            } //1.FLAUTA 2.ARMONICA 3.ANDALUZA 4.PIANO 5.ELECTRICA 6.OBOE 7.BATERIA
            catch (System.Exception)
            {
                throw;
            }

            //FUNCTIONS
            int getAttributeIdByValue(string value)
            {
                int idAttribute = 0;
                var idAttQuery = from att in cnt.Attribut
                where att.Value == value 
                select att.Id;

                idAttribute = idAttQuery.FirstOrDefault();
                return idAttribute;
            }
            int getInstrumIdByName(string name)
            {
                int idInstrument = 0;
                var idInsQuery = from ins in cnt.Instrument
                where ins.Name == name 
                select ins.Id;

                idInstrument = idInsQuery.FirstOrDefault();
                return idInstrument;
            }
            int getShopIdByCity(string city)
            {
                int idShop = 0;
                var idShopQuery = from sh in cnt.Shop
                where sh.City == city 
                select sh.Id;

                idShop = idShopQuery.FirstOrDefault();
                return idShop;
            }
            
            int getCatIdByName(string name)
            {
                int idCat = 0;
                var idCatQuery = from cat in cnt.Category
                where cat.Name == name 
                select cat.Id;

                idCat = idCatQuery.FirstOrDefault();
                return idCat;
            }
            int getSubCatIdByName(string name)
            {
                int idSubCat = 0;
                var idSubCatQuery = from subCat in cnt.SubCategory
                where subCat.Name == name 
                select subCat.Id;

                idSubCat = idSubCatQuery.FirstOrDefault();
                return idSubCat;
            }

        }
    }
}


