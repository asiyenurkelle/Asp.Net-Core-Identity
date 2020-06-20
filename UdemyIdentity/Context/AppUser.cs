using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.Context
{
    //Otomatik olarak oluşturulan appuser tablomuzu özellestircez.
    public class AppUser: IdentityUser<int>
    {
        public string PictureUrl { get; set; }
        //3NF'ye aykırı ama ogrenmek icin cinsiyeti simdilik ekledik buraya.
        public string Gender { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }
    }
}
