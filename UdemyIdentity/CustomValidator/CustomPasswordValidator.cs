using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentity.Context;

namespace UdemyIdentity.CustomValidator
{
    //Bu classın içerisinde kendimize özel validasyonlar yazıcaz.Hazır ıdentity'nin sağladıklarından farklı hatalar vermek için.
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        //parolanın kullanıcı adı içerememesi için hazır bir identity olmadıgından kendimiz oluşturuyoruz.
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            //Bir dizi oluşturduk ve eger kullancıadı parola içeriyorsa eleman sayısı bir olucak dizinin.Hata geldikce eleman sayısı artıcak.
            List<IdentityError> errors = new List<IdentityError>();
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {

                errors.Add(new IdentityError()
                {
                    Code = "PasswordContainsUserName",
                    Description = "Parola Kullanıcı adı içeremez!"
                });
            }

            if (errors.Count > 0)
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            else
            {
                return Task.FromResult(IdentityResult.Success);
            }
            //oluşturdugumuz validayonu startup.cs'de çağıracagız.
        }
    }
}
