using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using proje3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace proje3.Controllers
{
    public class AdminController : Controller
    {
        private ProjeContext db = new ProjeContext();


        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        // GET
        [Authorize(Roles = MyConstants.RoleAdmin)]
        public ActionResult List()
        {
            var liste = db.Users.ToList();
            return View(liste);
        }
        [Authorize(Roles = MyConstants.RoleAdmin)]
        public ActionResult Edit(string id)
        {
            //string donustur;
            //if(id != null)
            //{
            //    donustur = id.ToString();
            //}
            //else
            //{
            //    return View();
            //}
            ApplicationUser a = new ApplicationUser();
            var ara = db.Users.Find(id);
            return View(ara);
        }
        [Authorize(Roles = MyConstants.RoleAdmin)]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,UserName,PasswordHash")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                var varMi = db.Users.Find(applicationUser.Id);
                if (varMi != null)
                {
                    varMi.UserName = applicationUser.UserName;
                    varMi.PasswordHash = applicationUser.PasswordHash;

                    db.SaveChanges();
                    return RedirectToAction("List");
                }

            }
            return View();
        }

        [Authorize(Roles = MyConstants.RoleAdmin)]
        public ActionResult Delete(string id)//int? int ama null olabilir demek normal intten farkı budur
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ara = db.Users.Find(id);// ara applicationuser
            if (ara == null)
            {
                return HttpNotFound();
            }
            return View(ara);
        }

        [HttpPost]
        [Authorize(Roles = MyConstants.RoleAdmin)]
        [ActionName("Delete")]
        public ActionResult DeletePost(string id)
        {
            var ara = db.Users.Find(id);
            if (ara != null)
            {
                System.Diagnostics.Debug.WriteLine("ara null değil ");
                db.Users.Remove(ara);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }



        //GET
        [Authorize(Roles = MyConstants.RoleAdmin)]// burası kullanıcılar icin
        public ActionResult Sorgu(string UserName,string OgrenciNo, string yazarAd,
            string ogretimTuru, string dersAdi, string ozet, string teslimDonemi,
            string projeBasligi, string anahtarKelimeler, string danismanBilgileri, string juriBilgileri)
        {

            if (UserName == null && OgrenciNo == null && yazarAd == null && ogretimTuru == null
                && dersAdi == null && ozet == null && teslimDonemi == null && projeBasligi == null &&
                anahtarKelimeler == null && danismanBilgileri == null && juriBilgileri == null)
            {

                return View(db.Veriler.ToList());
            }

            return View(db.Veriler.Where(i => (i.UserName.Contains(UserName) && i.yazarAd.Contains(yazarAd) && i.ogrenciNo.Contains(OgrenciNo)
        && i.dersAdi.Contains(dersAdi) && i.teslimDonemi.Contains(teslimDonemi) && i.ogretimTuru.Contains(ogretimTuru) && i.projeBasligi.Contains(projeBasligi) && i.anahtarKelimeler.Contains(anahtarKelimeler)
        && i.ozet.Contains(ozet) && i.danismanBilgileri.Contains(danismanBilgileri) && i.juriBilgileri.Contains(juriBilgileri))).ToList());


        }




        [Authorize(Roles = MyConstants.RoleAdmin)]
        public ActionResult DeleteVeri(int? id)//int? int ama null olabilir demek normal intten farkı budur
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ara = db.Veriler.Find(id);// ara applicationuser
            if (ara == null)
            {
                return HttpNotFound();
            }
            return View(ara);
        }

        [HttpPost]
        [Authorize(Roles = MyConstants.RoleAdmin)]
        [ActionName("DeleteVeri")]
        public ActionResult DeleteVeri(int id)
        {
            var ara = db.Veriler.Find(id);
            if (ara != null)
            {
                System.Diagnostics.Debug.WriteLine("ara null değil ");
                db.Veriler.Remove(ara);
                db.SaveChanges();
                return RedirectToAction("Sorgu");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


    }
}