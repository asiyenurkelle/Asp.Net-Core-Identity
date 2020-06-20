using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using UdemyIdentity.Context;
using UdemyIdentity.Models;

namespace UdemyIdentity.Controllers
{
    //Sadece oturun açmış(giriş yapmış) kullanıcıların görüntüleyebilcegi işlemler.
    //Kullanıcıların oturum açmış olup/olmadıgını [Authorize] ile anlıyoruz.En üste yazarsak tüm controllerı kullanmak icin giris yapmıs olmamız gerekir.

    [Authorize]
    public class PanelController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public PanelController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task< IActionResult> Index()
        {
           var user= await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }
        public async Task<IActionResult> UpdateUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserUpdateViewModel model = new UserUpdateViewModel
            {
                Email = user.Email,
                Name = user.Name,
                SurName = user.SurName,
                PhoneNumber = user.PhoneNumber,
                pictureUrl = user.PictureUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task <IActionResult> UpdateUser(UserUpdateViewModel model)
        {
            //Kullanıcının doldurduğu bilgiler istediğimiz validasyona göre doldurulmul mu diye kontrol ediyoruz.
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                //kullanıcı güncelleme işleminde bir dosya secti mi diye bakıyoruz eger dosya seçmediyse boşuna upload işlemi yapmamak için.
                if(model.Picture!= null)
                {
                    //uygulamanın calıstıgı yere ulasmak icin directory metodunu kullanıyoruz.
                    var uygulamaninCalistigiYer=Directory.GetCurrentDirectory();
                    // yüklenecek dosyanın uzantısını buluyoruz.
                    var uzanti = System.IO.Path.GetExtension(model.Picture.FileName);
                    var resimAd = Guid.NewGuid() + uzanti;
                    var kaydedilecekYer = uygulamaninCalistigiYer + "/wwwroot/img/" + resimAd;
                    // gelen dosyayı nereye kaydedecegimiz.
                   
                    using var stream = new FileStream(kaydedilecekYer, FileMode.Create);
                    //stream maliyetli bir işlem oldugu icin basına using koyarak işi bitince garbage collactor'e gitmesini sağladık.



                    //bir işlem uzun sürüyorsa kullanıcıyı bekletmemek icin asenkronik metot kullanırız.Upload işlemi herkeste farklı olabilir ve bekleme işlemi uzun sürebilir.

                    await model.Picture.CopyToAsync(stream);
                    user.PictureUrl = resimAd;
                    //resimAd upload işlemini gerceklestirdik.Ve pictureurl'e yeni ismini atadık.
                }
                //kullanıcı güncelleme işlemleri
                //userın bilgilerini modelin içinden alıyoruz.
                user.Name = model.Name;
                user.SurName = model.SurName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    //kullanıcının güncelleme işlemi gerceklestiyse onu yönlendiricez.
                    return RedirectToAction("Index");
                }
                foreach(var item in result.Errors)
                {
                    // güncelleme işlemi başarısızsa modelstate'in içerisinden ilgili hataların açıklamasını bulup ekliyoruz.
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }

        //Herkesin erişmesini istediğimiz bir sayfa yazıcak olursak şu şekilde yaparız;
        [AllowAnonymous]
        public IActionResult HerkesErissin()
        {
            return View();
        }
        public async Task< IActionResult> LogOut()
        {
           await  _signInManager.SignOutAsync();
           return RedirectToAction("Index", "Home");
        }
    }
}
