using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proje3.Models
{
    public class Veri
    {
        [Key]
        public int verilerID { get; set; }
        public string yazarAd { get; set; }// ad soyad olarark
        public string ogrenciNo { get; set; }
        public string ogretimTuru { get; set; }
        public string dersAdi { get; set; }
        public string ozet { get; set; }
        public string teslimDonemi { get; set; }
        public string projeBasligi { get; set; }
        public string anahtarKelimeler { get; set; }
        public string danismanBilgileri { get; set; }//ad soyad ünvan
        public string danismanUnvan { get; set; }
        public string juriBilgileri { get; set; }  //ad soyad unvan danışmanda juridir
        public string juriUnvan { get; set; }
        public string UserName { get; set; } // herkes kendi eklediğini gorecek
        public string dosyaAdi { get; set; }


    }
}