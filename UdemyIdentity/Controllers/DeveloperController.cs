using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UdemyIdentity.Controllers
{
    [Authorize(Roles = "Admin,Developer")]
    //Bu sayfaya admin veya developerlar girebilir diğerleri giremez.

    public class DeveloperController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
