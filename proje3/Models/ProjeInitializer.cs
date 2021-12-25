using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using proje3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace proje3.Models
{
    public class ProjeInitializer : DropCreateDatabaseIfModelChanges<ProjeContext>
    {
        protected override void Seed(ProjeContext projeContext)
        {
            List<Veri> veris = new List<Veri>();//boş olcak
            foreach (Veri veri in veris)
                projeContext.Veriler.Add(veri);


            base.Seed(projeContext);
        }
        /*
        private void PopulateUserandRoles()
        {
            var db = new ApplicationDbContext();
            //var db = new ProjeContext();
            //var projecontext = new ProjeContext();
            //var db = ProjeContext.Create();
            // popualtin roles
            if (!db.Roles.Any(x => x.Name == MyConstants.RoleAdmin))
            {
                db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(MyConstants.RoleAdmin));
                db.SaveChanges();
            }
            if (!db.Roles.Any(x => x.Name == MyConstants.RoleUser))
            {
                db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(MyConstants.RoleUser));
                db.SaveChanges();
            }

            if (!db.Users.Any(x => x.UserName == "appadmin"))
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new ApplicationUserManager(userStore);

                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                // kullaniciyi oluşturuyor
                var newUser = new ApplicationUser
                {
                    Id = "1",
                    Email = "metehanmart@gmail.com",
                    UserName = "appadmin"
                };

                userManager.Create(newUser, "applicationadmin");// 2. parametre şifre
                userManager.AddToRole(newUser.Id, MyConstants.RoleAdmin);// role ekledi
                db.SaveChanges();
            }

            if (!db.Users.Any(x => x.UserName == "appuser"))
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new ApplicationUserManager(userStore);

                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                // kullaniciyi oluşturuyor
                var newUser = new ApplicationUser
                {
                    Email = "metehasd@gmail.com",
                    UserName = "appuser"
                };

                userManager.Create(newUser, "applicationuser");// 2. parametre şifre
                userManager.AddToRole(newUser.Id, MyConstants.RoleUser);// role ekledi


                db.SaveChanges();
            }

        }
        */
    }
}