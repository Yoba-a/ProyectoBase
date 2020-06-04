using Microsoft.AspNet.Identity.Owin;
using Modeln;
using Servicen.Auth;
using Servicen.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    
    public class UserController : Controller
    {
        private readonly UserService _userService = new UserService();


        private ApplicationRoleManager _roleManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
        }
        private ApplicationUserManager _userManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            } 
        }
        public ActionResult Index()
        {
            return View(
                _userService.GetAll()
                );
        }
        public ActionResult Get(string id)
        {
            ViewBag.Roles = _roleManager.Roles.Where(x=>x.Enabled).ToList();
            return View(
                _userService.Get(id)
                );
        }

        public async Task<ActionResult> AddRoleToUser(ApplicationUserRole role)
           
        {
            if(! _userManager.IsInRoleAsync(role.UserId, role.RoleId).Result)
            {
                var result = await _userManager.AddToRoleAsync(role.UserId, role.RoleId);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First());
                }
            }
            return RedirectToAction("Index");
            
        }

        public async Task CreateRoles()
        {
            var roles = new List<ApplicationRole>
            {
                new ApplicationRole{Name ="Admin" },
                new ApplicationRole{Name ="Moderator" },
                new ApplicationRole{Name ="User" },

            };

            foreach (var r in roles)
            {
                if (!await _roleManager.RoleExistsAsync(r.Name))
                {
                    await _roleManager.CreateAsync(r);
                }

            }
        }

        [AllowAnonymous]
        public string Anonymus()
        {
            return "anonymous";
        }
        [Authorize(Roles = "Moderator")]
        public string Moderator()
        {
            return "M oderator";

        }

        [Authorize(Roles = "Admin")]
        public string Admin()
        {
            return "Admin";

        }


    }

}