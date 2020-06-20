using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.Models
{
    public class RoleAssignViewModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        //modelden bir liste alıcaz ve bütün rolleri bu listeye atıcaz.Bütün roller icinden ilgili rolün kullanıcıda var olup olmadıgına bakıcaz
        public bool Exists { get; set; }
    }
}
