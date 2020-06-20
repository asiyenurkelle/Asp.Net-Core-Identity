using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.CustomValidator
{
    //Burada ıdentity'nin göstermiş oldugu hata mesajlarını türkçeleştiricez.
    public class CustomIdentityValidator : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                //code'a istediğim ismi verebilirim acıklmayada türkçe hata mesajımızı yazdık.
                Code = "PasswordTooShort",
                //length parametresi bizim startup.cs'de verdiğimiz uzunluk değeri.
                Description = $"Parola minimum {length} karakter olmalıdır."
            };
            //burada yazdıgımız türkçeleştirilmiş hata mesajını gösterebilmemiz için konfigürasyon ayarlarına(startup.cs) bu sınıfı eklememiz gerek.
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "Parola bir alfanümerik karakter(! . ~ vs.) içermelidir"
            };
        }
        //daha önce alınmış kullanıcıadını almaması için gösterilen hata mesajını türkçeleştirmek.
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code="DuplicateUserName",
                Description=$"İlgili kullanıcı adı()({userName}) zaten sistemde kayıtlı."

            };
        }

    }
}
