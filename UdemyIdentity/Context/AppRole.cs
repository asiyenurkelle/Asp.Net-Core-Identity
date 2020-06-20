using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.Context
{
    //otomatik olarak gelen db tablolarından approle'ü özellestirmek icin olusturdugumuz class.
    public class AppRole:IdentityRole<int>
    {

    }
}
