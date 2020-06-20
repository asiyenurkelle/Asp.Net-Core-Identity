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
            //ýdentity nerede calýsýcak(entityframework) ve hangi dbde calýsýcak(udemycontext)
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                // burada identity tarafýndan kayýt ol formuna atanan sifre oluþtururken sahip olmasý gereken özellikleri deðiþtirdik
                opt.Password.RequireDigit = false;
                //þifrenin sadece sayý olmasý zorunlulugunu kaldýrdýk.
                opt.Password.RequireLowercase = false;
                //büyük harf zorunlulugunu kaldýrdýk.
                opt.Password.RequiredLength = 1;
                //sifrenin minimum uzunlugunun 1 olmasýný sagladýk.
                opt.Password.RequireNonAlphanumeric = false;
                //sifrede ?! gibi karakterlerin olma zorunlulugunu kaldýrdýk.
                opt.Password.RequireUppercase = false;

                //kaç dakika kullanýcýyý bloklayacagýmýz.
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                //kullanýcý kaç defa yanlýþ girince onu bloklayacagýmýz.
                opt.Lockout.MaxFailedAccessAttempts = 3;
                //ilgili kullanýcý emailini onaylamadýysa isnotallowed'a düþür.
                //opt.SignIn.RequireConfirmedEmail = true;

            }).AddErrorDescriber<CustomIdentityValidator> //türkcelestirilmiþ hata mesajlarýmýzý göstercegimiz sýnýfý ekledik.
            ().AddPasswordValidator<CustomPasswordValidator>
            ().AddEntityFrameworkStores<UdemyContext>();

            services.ConfigureApplicationCookie(opt =>
            {
                //Yetkimiz olmayan bir yere girmeye calýstýgýmýzda home/ýndex url'ine yönlendirme yapar.
                opt.LoginPath = new PathString("/Home/Index");
                opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
                //httponlye true dediðimiz için bu cookie javascript tarafýndan çekilemez document.cookie dediðinde cookienin verilerine ulaþamaz.
                opt.Cookie.HttpOnly = true;
                //tarayýcýda gözüken cookie nin ismini atýyoruz.
                opt.Cookie.Name = "UdemyCookie";
                //Samsite'a lax atarsak cookie'nin verilerine diðer sitelerde eriþir.Scrict yaparsak sub domainlerimiz dahil bu cookieye eriþemez.
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.ExpireTimeSpan = TimeSpan.FromDays(20);
                //Cookie'nin ayakta kalma süresini 20 gün ayarladýk default býraksak 14 kalýrdý.
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
