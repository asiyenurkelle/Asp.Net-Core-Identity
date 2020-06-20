using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.Models
{
    public class UserSignUpViewModel
    {
        //confirm metodu içerisine kıyaslıcagı property'i alır parolaların eşleşme durumunu kontrol ettirip,uyarı verdirdik.
        [Display(Name="Kullanıcı Adı:")]
        [Required(ErrorMessage ="Kullanıcı Adı Boş Geçilemez.")]
        public string UserName { get; set; }
        [Display(Name = "Parola:")]
        [Required(ErrorMessage = "Parola Boş Geçilemez.")]
        public string Password { get; set; }
        [Display(Name = "Parola Tekrar:")]
        [Compare("Password",ErrorMessage ="Parolalar eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Ad:")]
        [Required(ErrorMessage = "Ad Boş Geçilemez.")]
        public string Name { get; set; }
        [Display(Name = "Soyad:")]
        [Required(ErrorMessage = "Soyad Boş Geçilemez.")]
        public string SurName { get; set; }
        [Display(Name = "E-mail:")]
        [Required(ErrorMessage = "E-mail Boş Geçilemez.")]
        public string Email { get; set; }
    }
}
