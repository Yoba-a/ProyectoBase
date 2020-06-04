

using Microsoft.AspNet.Identity.EntityFramework;
using Modeln;

using System.Data.Entity;

namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRole { get; set; }

        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }

        public  DbSet<Cliente> Clientes { get; set; }
        public DbSet<Targeta> Targetas { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
