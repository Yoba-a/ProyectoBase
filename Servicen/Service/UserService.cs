using Modeln;
using Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicen.Service
{
    public class UserService
    {
        public IEnumerable<UserGrid> GetAll()
        {
            var result = new List<UserGrid>();
            using (var ctx = new ApplicationDbContext())
            {
                result = (
                    from au in ctx.ApplicationUsers
                    from aur in ctx.ApplicationUserRole.Where(x => x.UserId == au.Id).DefaultIfEmpty()
                    from ar in ctx.ApplicationRole.Where(x => x.Id == aur.RoleId && x.Enabled).DefaultIfEmpty()
                    select new UserGrid
                    {
                        Id = au.Id,
                        Email = au.Email,
                        Nombre = au.Nombre,
                        Apellido = au.Apellido,
                        Role = ar.Name
                    }
                     ).ToList();

            }

                return result;
        }
        public ApplicationUser Get(string id )
        {
            var result = new ApplicationUser();
            using (var ctx = new ApplicationDbContext())
            {
                result = ctx.ApplicationUsers.Where(x =>x.Id == id).Single();

            }
            return result;

        }
        public void UpdateName(ApplicationUser model)
        {
            var result = new List<ApplicationUser>();
            using (var ctx = new ApplicationDbContext())
            {
                var originalEntity = ctx.ApplicationUsers.Where(x => x.Id == model.Id).Single();
                originalEntity.Nombre = model.Nombre;
                originalEntity.Apellido = model.Apellido;


                ctx.Entry(originalEntity).State = EntityState.Modified;
                ctx.SaveChanges();

            }

        }
    }
}
