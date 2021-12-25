using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proje3.Models
{
    public class GirisModel:ApplicationUser
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}