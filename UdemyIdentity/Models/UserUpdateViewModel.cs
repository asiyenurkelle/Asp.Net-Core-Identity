using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.Models
{
    public class UserUpdateViewModel
    {
        [Display(Name="E-mail:")]
        [Required(ErrorMessage = "E-mail alanı gereklidir")]
        //girilen mail adresinin e-mail adresi formatına uygun olmasını yazdık.
        [EmailAddress(ErrorMessage ="Lütfen geçerli bir e-mail adresi giriniz.")]
        public string Email { get; set; }

        [Display(Name = "Telefon:")]
        public string PhoneNumber { get; set; }

        public string pictureUrl { get; set; }
       
        public IFormFile Picture { get; set; }

        [Display(Name = "İsim:")]
        [Required(ErrorMessage="Name alanı gereklidir.")]
        public string Name { get; set; }

        [Display(Name = "Soyisim:")]
        [Required(ErrorMessage = "Surname alanı gereklidir.")]
        public string SurName { get; set; }


    }
}
