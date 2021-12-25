using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using proje3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.Entity;

namespace proje3.Controllers
{
    public class HomeController : Controller
    {
        private string _FileName;
        private ProjeContext _context = new ProjeContext();

        //GET
        [Authorize(Roles = MyConstants.RoleAdmin + "," + MyConstants.RoleUser)]
        public ActionResult DosyaYukle()
        {

            return View();
        }

        [HttpPost]
        [Authorize(Roles = MyConstants.RoleAdmin + "," + MyConstants.RoleUser)]
        public ActionResult DosyaYukle(HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                    return View();
                if (file.ContentLength > 0)
                {
                    _FileName = System.IO.Path.GetFileName(file.FileName);
                    string _path = System.IO.Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                    // burada Veri modelinin icini doldurmalıyız
                    verileriYukle();
                    Console.WriteLine("içerde ");
                }
                ViewBag.Message = "Dosya yüklendi!";


                return View();
            }
            catch
            {
                ViewBag.Message = "Dosya yükleme başarısız!";
                return View();
            }

        }

        public void verileriYukle()
        {
            //gecici degiskenler:
            Veri v = new Veri();
            string yazarAd = "";
            string ogrenciNo = "";
            string ogretimTuru = "";
            string dersAdi = "";
            string ozet = "";
            string teslimDonemi = "";
            string projeBasligi = "";
            string anahtarKelimeler = "";
            string danismanBilgleri = "";
            string danismanUnvan = "";
            string juriBilgiler = "";
            string juriUnvan = "";
            int yazarSayisi = 1;
            int indeks = 0;
            string sayfa;
            string dosyaAdi;
            string[] lines;
            string[] anahtarlarTmp;
            try
            {
                string yol = $"C:/Users/furka/Desktop/proje/proje/proje/UploadedFiles/{_FileName}";//yol değişmeli
                //string hadee = this.HttpContext.Server.MapPath(".");
                string hadee2 = Server.MapPath("~");
                //Console.WriteLine(hadee);
                //System.Diagnostics.Debug.WriteLine("bu mu1 = "+hadee);
                System.Diagnostics.Debug.WriteLine("bu mu2 = " + hadee2);//bu
                hadee2 += $"UploadedFiles\\{_FileName}";
                System.Diagnostics.Debug.WriteLine("bu mu2 = " + hadee2);
                dosyaAdi = _FileName;


                using (PdfReader reader = new PdfReader(hadee2))
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        if (i == 2)
                        {
                            sayfa = PdfTextExtractor.GetTextFromPage(reader, i);
                            lines = sayfa.Replace("\r", "").Split('\n');
                            dersAdi = lines[6];
                            for (int j = 8; j < 14; j++)//eklendi
                            {
                                projeBasligi += lines[j];
                                if (lines[j + 1] != " ")
                                    continue;
                                else
                                {
                                    yazarAd = lines[j + 2];
                                    if (lines[j + 3] != " ")
                                    {
                                        yazarAd += ", " + lines[j + 3];
                                        if (lines[j + 4] != " ")
                                        {
                                            yazarAd += ", " + lines[j + 4];
                                        }
                                    }
                                    break;
                                }
                            }
                            for (int j = 0; j < lines.Length; j++)
                            {
                                if (lines[j].Contains("Danışman"))
                                {
                                    danismanBilgleri = lines[j - 1];
                                    juriBilgiler = lines[j - 1];
                                }
                                else if (lines[j].Contains("Jüri Üyesi"))
                                {
                                    juriBilgiler += ", " + lines[j - 1];
                                }
                                else if (lines[j].Contains("Tarih"))
                                {
                                    teslimDonemi = lines[j].Substring(31, 4);
                                    if (lines[j].Contains(".06"))
                                    {
                                        teslimDonemi = (Int32.Parse(teslimDonemi) - 1) + "-" + teslimDonemi + " Bahar";
                                    }
                                    else if (lines[j].Contains(".01"))
                                    {
                                        teslimDonemi = (Int32.Parse(teslimDonemi) - 1) + "-" + teslimDonemi + " Güz";
                                    }
                                }
                            }
                            Console.WriteLine(sayfa);
                        }

                        else if (i == 4)//öğrenci numarası ve öğretim türlerini çıkart
                        {
                            sayfa = PdfTextExtractor.GetTextFromPage(reader, i);
                            lines = sayfa.Replace("\r", "").Split('\n');
                            for (int j = 0; j < lines.Length; j++)
                            {
                                if (lines[j].Contains("Öğrenci No"))
                                {
                                    string resultString = Regex.Match(lines[j], @"\d+").Value;
                                    ogrenciNo += Int32.Parse(resultString) + " , ";
                                    if (resultString.Substring(3, 3) == "201")
                                    {
                                        ogretimTuru = "1. Öğretim";
                                    }
                                    else
                                    {
                                        ogretimTuru = "2. Öğretim";
                                    }
                                }
                            }
                            ogrenciNo = ogrenciNo.Substring(0, ogrenciNo.Length - 3);
                            //Console.WriteLine(sayfa);

                        }
                        else if (i == 10)//özet ve anahtar kelimeleri çıkart
                        {
                            sayfa = PdfTextExtractor.GetTextFromPage(reader, i);
                            int anahtarKelimeOzetIndeks = sayfa.IndexOf("ÖZET");
                            int anahtarKelimeAnahtarKelimelerIndeks = sayfa.IndexOf("Anahtar kelimeler:");
                            int bitisIndeks = sayfa.IndexOf(".\n\n");
                            ozet = sayfa.Substring(anahtarKelimeOzetIndeks + 6, anahtarKelimeAnahtarKelimelerIndeks - 8 - anahtarKelimeOzetIndeks);
                            anahtarKelimeler = sayfa.Substring(anahtarKelimeAnahtarKelimelerIndeks + 19, sayfa.Length - (anahtarKelimeAnahtarKelimelerIndeks + 19));
                            anahtarlarTmp = anahtarKelimeler.Replace("\n", "").Replace(".", "").Split(',');
                            anahtarKelimeler = "";
                            anahtarKelimeler += anahtarlarTmp[0] + " , ";
                            for (int j = 1; j < anahtarlarTmp.Length; j++)//başlarındaki boşluklar kalkıyor(1. hariç onda zaten yok)
                            {
                                anahtarlarTmp[j] = anahtarlarTmp[j].Substring(1, anahtarlarTmp[j].Length - 1);
                                anahtarKelimeler += anahtarlarTmp[j] + " , ";
                            }
                            anahtarKelimeler = anahtarKelimeler.Substring(0, anahtarKelimeler.Length - 3);
                            anahtarKelimeler = anahtarKelimeler.ToLower();

                        }

                    }
                    kontrol();
                    VerileriResmenYukle();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            void kontrol()
            {
                System.Diagnostics.Debug.WriteLine("dersAdi = " + dersAdi);
                System.Diagnostics.Debug.WriteLine("proje başlığı = " + projeBasligi);
                System.Diagnostics.Debug.WriteLine("yazaradi = " + yazarAd);
                System.Diagnostics.Debug.WriteLine("ogrencino = " + ogrenciNo);
                System.Diagnostics.Debug.WriteLine("ogretimTuru = " + ogretimTuru);
                System.Diagnostics.Debug.WriteLine("danismanBilgleri = " + danismanBilgleri);
                System.Diagnostics.Debug.WriteLine("juriBilgiler = " + juriBilgiler);
                System.Diagnostics.Debug.WriteLine("teslimDonemi = " + teslimDonemi);
                System.Diagnostics.Debug.WriteLine("anahtarKelimeler = " + anahtarKelimeler);
                System.Diagnostics.Debug.WriteLine("ozet = \n" + ozet);


            }
            void VerileriResmenYukle()
            {
                v.dersAdi = dersAdi;
                v.projeBasligi = projeBasligi;
                v.yazarAd = yazarAd;
                v.ogrenciNo = ogrenciNo;
                v.ogretimTuru = ogretimTuru;
                v.danismanBilgileri = danismanBilgleri;
                v.juriBilgileri = juriBilgiler;
                v.teslimDonemi = teslimDonemi;
                v.anahtarKelimeler = anahtarKelimeler;
                v.ozet = ozet;
                v.dosyaAdi = dosyaAdi;
                v.UserName = User.Identity.GetUserName();
                _context.Veriler.Add(v);
                _context.SaveChanges();
            }
            /* 
             iText kullanabiliriz belki itextsharp da olabilir ama o depracated olmuş sıkıntı olabilir
             */
            /*
            Veri v = new Veri();
            v.yazarAd = "ekmek";
            projeContext.Veriler.Add(v);
            projeContext.SaveChanges();
            */
            /*
            StringBuilder processed = new StringBuilder();

            for (int i = 1; i <= pdfDocument.GetNumberOfPages(); ++i)
            {
                var page = pdfDocument.GetPage(i);
                string text = PdfTextExtractor.GetTextFromPage(page, strategy);
                processed.Append(text);
            }
            */

        }

        //GET
        [Authorize(Roles = MyConstants.RoleUser + "," + MyConstants.RoleAdmin)]// burası kullanıcılar icin
        public ActionResult Sorgu(string OgrenciNo, string yazarAd,
            string ogretimTuru, string dersAdi, string ozet, string teslimDonemi,
            string projeBasligi, string anahtarKelimeler, string danismanBilgileri, string juriBilgileri)
        {


            string name = User.Identity.GetUserName();// şu anki kullanıcının idsi

            if (name == "appadmin")
            {
                return RedirectToAction("Sorgu", "Admin");
            }


            if (OgrenciNo == null && yazarAd == null && ogretimTuru == null
                && dersAdi == null && ozet == null && teslimDonemi == null && projeBasligi == null &&
                anahtarKelimeler == null && danismanBilgileri == null && juriBilgileri == null)
            {
                System.Diagnostics.Debug.WriteLine("if reis");
                var list = _context.Veriler.Where(i => i.UserName.Equals(name));
                return View(list.ToList());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("else else else movuk");
                return View(_context.Veriler.Where(i => (i.yazarAd.Contains(yazarAd) && i.ogrenciNo.Contains(OgrenciNo)
           && i.dersAdi.Contains(dersAdi) && i.teslimDonemi.Contains(teslimDonemi) && i.ogretimTuru.Contains(ogretimTuru) && i.projeBasligi.Contains(projeBasligi) && i.anahtarKelimeler.Contains(anahtarKelimeler)
           && i.ozet.Contains(ozet) && i.danismanBilgileri.Contains(danismanBilgileri) && i.juriBilgileri.Contains(juriBilgileri)) && i.UserName.Equals(name)).ToList());
            }

        }

        // Sorgunun postu yok muydu ya
        [Authorize(Roles = MyConstants.RoleUser + "," + MyConstants.RoleAdmin)]
        public ActionResult DonemlereGore(string kullanici, string donem, string dersAdi)
        {
            string name = User.Identity.GetUserName();
            if (kullanici == null || donem == null || dersAdi == null)
            {
                var liste = _context.Veriler.Where(i => i.UserName == kullanici && i.teslimDonemi == donem && i.dersAdi == dersAdi).ToList();
                return View(liste);
            }

            if (name == "appadmin")
                return View(_context.Veriler.Where(i => i.UserName == kullanici && i.teslimDonemi == donem && i.dersAdi == dersAdi).ToList());
            else
                return View(_context.Veriler.Where(i => i.UserName == kullanici && i.teslimDonemi == donem && i.dersAdi == dersAdi && i.UserName.Equals(name)).ToList());
        }
        //[Authorize(Roles = MyConstants.RoleAdmin + "," + MyConstants.RoleUser)]
        //[HttpPost]
        //public ActionResult DonemlereGore()
        //{   

        //    return View();
        //}
        //[Authorize(Roles = MyConstants.RoleAdmin + "," + MyConstants.RoleUser)]
        //[HttpPost]
        //public ActionResult DonemlereGore()
        //{
        //    return View();
        //}



        [Authorize(Roles = MyConstants.RoleUser)]
        public ActionResult Delete(int? id)//int? int ama null olabilir demek normal intten farkı budur
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ara = _context.Veriler.Find(id);// ara applicationuser
            if (ara == null)
            {
                return HttpNotFound();
            }
            return View(ara);
        }

        [HttpPost]
        [Authorize(Roles = MyConstants.RoleUser)]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var ara = _context.Veriler.Find(id);
            if (ara != null)
            {
                System.Diagnostics.Debug.WriteLine("ara null değil ");
                _context.Veriler.Remove(ara);
                _context.SaveChanges();
                return RedirectToAction("Sorgu");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

    }
}