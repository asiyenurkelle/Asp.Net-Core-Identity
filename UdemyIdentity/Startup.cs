using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UdemyIdentity.Context;
using UdemyIdentity.CustomValidator;

namespace UdemyIdentity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //databaseimizi ekledik.
            services.AddDbContext<UdemyContext>();
            //�dentity nerede cal�s�cak(entityframework) ve hangi dbde cal�s�cak(udemycontext)
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                // burada identity taraf�ndan kay�t ol formuna atanan sifre olu�tururken sahip olmas� gereken �zellikleri de�i�tirdik
                opt.Password.RequireDigit = false;
                //�ifrenin sadece say� olmas� zorunlulugunu kald�rd�k.
                opt.Password.RequireLowercase = false;
                //b�y�k harf zorunlulugunu kald�rd�k.
                opt.Password.RequiredLength = 1;
                //sifrenin minimum uzunlugunun 1 olmas�n� saglad�k.
                opt.Password.RequireNonAlphanumeric = false;
                //sifrede ?! gibi karakterlerin olma zorunlulugunu kald�rd�k.
                opt.Password.RequireUppercase = false;

                //ka� dakika kullan�c�y� bloklayacag�m�z.
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                //kullan�c� ka� defa yanl�� girince onu bloklayacag�m�z.
                opt.Lockout.MaxFailedAccessAttempts = 3;
                //ilgili kullan�c� emailini onaylamad�ysa isnotallowed'a d���r.
                //opt.SignIn.RequireConfirmedEmail = true;

            }).AddErrorDescriber<CustomIdentityValidator> //t�rkcelestirilmi� hata mesajlar�m�z� g�stercegimiz s�n�f� ekledik.
            ().AddPasswordValidator<CustomPasswordValidator>
            ().AddEntityFrameworkStores<UdemyContext>();

            services.ConfigureApplicationCookie(opt =>
            {
                //Yetkimiz olmayan bir yere girmeye cal�st�g�m�zda home/�ndex url'ine y�nlendirme yapar.
                opt.LoginPath = new PathString("/Home/Index");
                opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
                //httponlye true dedi�imiz i�in bu cookie javascript taraf�ndan �ekilemez document.cookie dedi�inde cookienin verilerine ula�amaz.
                opt.Cookie.HttpOnly = true;
                //taray�c�da g�z�ken cookie nin ismini at�yoruz.
                opt.Cookie.Name = "UdemyCookie";
                //Samsite'a lax atarsak cookie'nin verilerine di�er sitelerde eri�ir.Scrict yaparsak sub domainlerimiz dahil bu cookieye eri�emez.
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.ExpireTimeSpan = TimeSpan.FromDays(20);
                //Cookie'nin ayakta kalma s�resini 20 g�n ayarlad�k default b�raksak 14 kal�rd�.
            });


            services.AddAuthorization(opt => { opt.AddPolicy("FemalePolicy", cnf =>
                    { cnf.RequireClaim("gender", "female"); });
            });


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
