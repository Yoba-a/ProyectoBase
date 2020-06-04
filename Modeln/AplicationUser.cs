using Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

using System.Security.Claims;

using System.Threading.Tasks;

namespace Modeln
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(120)]
        public string Nombre { get; set; }

        [StringLength(120)]
        public string Apellido { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }
        public async static Task<ClaimsIdentity> CreateUserClaims(
    ClaimsIdentity identity,
    UserManager<ApplicationUser> manager,
    string userId
)
        {
            // Current User
            var currentUser = await manager.FindByIdAsync(userId);

            // Your User Data
            var jUser = JsonConvert.SerializeObject(new CurrentUser
            {
                UserId = currentUser.Id,
                Nombre = currentUser.Nombre,
                UserName = currentUser.UserName,
                Email = currentUser.Email,
            });

            identity.AddClaim(new Claim(ClaimTypes.UserData, jUser));

            return await Task.FromResult(identity);
        }

    }

}
