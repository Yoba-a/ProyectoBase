using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeln
{
    public class ApplicationRole : IdentityRole
    {
        public bool Enabled { get; set; }




    }


}
