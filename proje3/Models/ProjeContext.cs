using Microsoft.AspNet.Identity.EntityFramework;
using proje3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace proje3.Models
{
    public class ProjeContext: IdentityDbContext<ApplicationUser>
    {   
       
        public ProjeContext():base("DefaultConnection")//connection stringin adı
        {   
            Database.SetInitializer(new ProjeInitializer());
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Veri> Veriler { get; set; }
    }
}