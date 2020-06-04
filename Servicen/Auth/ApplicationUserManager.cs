using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Modeln;
using NLog;
using Persistence;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicen.Auth
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {

        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure la lógica de validación de nombres de usuario
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure la lógica de validación de contraseñas
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configurar valores predeterminados para bloqueo de usuario
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Registre proveedores de autenticación en dos fases. Esta aplicación usa los pasos Teléfono y Correo electrónico para recibir un código para comprobar el usuario
            // Puede escribir su propio proveedor y conectarlo aquí.
            manager.RegisterTwoFactorProvider("Código telefónico", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Su código de seguridad es {0}"
            });
            manager.RegisterTwoFactorProvider("Código de correo electrónico", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Código de seguridad",
                BodyFormat = "Su código de seguridad es {0}"
            });
           // manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }


        public async override Task<IdentityResult> AddToRoleAsync(string userId, string roleId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.ApplicationUserRole.Add(new ApplicationUserRole
                    {
                        UserId = userId,
                        RoleId = roleId
                    });

                    ctx.SaveChanges();
                }

                return await Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return await Task.FromResult(new IdentityResult(ex.Message));
            }
        }
        public async override Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    foreach (var roleId in roles)
                    {
                        ctx.ApplicationUserRole.Add(new ApplicationUserRole
                        {
                            UserId = userId,
                            RoleId = roleId,
                        });
                    }
                    ctx.SaveChanges();
                }
                return await Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return await Task.FromResult(new IdentityResult(ex.Message));
            }

        }

        public async override Task<bool> IsInRoleAsync(string userId , string roleId)
        {
            var result = false;
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    result = ctx.ApplicationUserRole.Any(x => x.UserId == userId && x.RoleId == roleId);

                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
            return await Task.FromResult(result);
        }
    }
}
