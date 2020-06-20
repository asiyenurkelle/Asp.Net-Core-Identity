using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UdemyIdentity.Context;
using UdemyIdentity.Models;

namespace UdemyIdentity.Controllers
{
    public class HomeController : Controller
    {
        //user manager asp.net core tarafından db islemlerini kolaylastırmak icin hazırlanmıs bir sınıf.
        //İçerisine kontrol edicegi userı veririz
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            //Dependecy Injection (S.O.L.I.D) yöntemini kullandık.Bagımlılıgı azaltmak icin IAction resultun icinde newlemek yerine
            //constructorda örneğini alcagımız değişkeni cagırdık.
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View(new UserSignInViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //eger bu tokenı bütün heryerde kullanmak istersek controllerların üzerine [AutoValidateAntiforgeryToken] yazılır.
        //Bu attribute yazıldıgı action da client'da bir token degeri varsa ancak o client buraya bir istekte bulunabilir.
        public async Task<IActionResult> GirisYap(UserSignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                //giriş yapma işlemleri
                //3.parametremiz isPersistent yani kullancıyı birdaha hatırlayıp/hatırlamayacagı biz false verdik.
                //4.parametre lockOutOnFailure yani kullanıcının belirli sayıda sifreyi yanlıs girdiği durumda kullanıcıyı bloklayıp/bloklamayacagı 
                //biz true verdik ve db de AccessFailedCount alanı her yanlış girişte 1 artıcak.4.yanlışın sonunda Lockoutende kolonuna bir tarih atancak ve o tarihe kadar kullanıcı bloklandı.

                var identityResult= await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

                if (identityResult.IsLockedOut)
                {
                    var gelen = await _userManager.GetLockoutEndDateAsync
                        (await _userManager.FindByNameAsync(model.UserName));

                    var kisitlanansure=gelen.Value;
                    //Şuanki dakikadan ksııtlanan dakikayı cıkararak kalan dakikayı bulup,bilgi mesajı gönderdik.
                    var kalanDakika = kisitlanansure.Minute- DateTime.Now.Minute;

                    // eger bloklandıysa error mesajı verip,modeli index sayfasına yönlendiriyoruz.
                    ModelState.AddModelError("", $"3 kere yanlış girdiğiniz için hesabınız {kalanDakika} dk kilitlenmiştir.");
                    return View("Index", model);
                }

                //if (identityResult.IsNotAllowed)
                //{
                //    ModelState.AddModelError("", "E-mail adresinizi lütfen doğrulayınız.");
                //    return View("Index", model);
                //}

                if (identityResult.Succeeded)
                {
                    //ındex gidicek panelkontrollerına gidicek
                    return RedirectToAction("Index", "Panel");

                }
                var yanlisGirilmeSayisi = await _userManager.GetAccessFailedCountAsync
                    (await _userManager.FindByNameAsync(model.UserName));
                ModelState.AddModelError("",$"Kullanıcı adı veya Şifre hatalı {3-yanlisGirilmeSayisi} defa daha yanlış girerseniz hesabınız kilitlenecektir.");
            }
            return View("Index", model);
        }
        public IActionResult KayitOl()
        {

            return View(new UserSignUpViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> KayitOl(UserSignUpViewModel model)
        {
            //her zaman modelstate'in valid olup olmadıgını kontrol edicez.
            //asp.net core identity mimarisi komple asenkronik metotlarla calısır.
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    Email = model.Email,
                    Name = model.Name,
                    SurName = model.SurName,
                    UserName = model.UserName
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // işlem başarılıysa index sayfasına gönderdik yani girisyap ekranı.
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
