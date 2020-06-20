using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.Models
{
    public class UserSignInViewModel
    {
        //required (boş geçilememesi) gibi işlemler bizim validasyon kontrollerimiz.
        [Display(Name ="Kullanıcı Adı:")]
        //display ile labela kullanıcı adı yazdırdık.
        [Required(ErrorMessage ="Kullanıcı Adı Boş Geçilemez")]
        public string UserName { get; set; }

        [Display(Name = "Şifre:")]
        [Required(ErrorMessage = "Şifre Boş Geçilemez")]
        public string  Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
