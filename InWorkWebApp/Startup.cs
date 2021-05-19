using InWorkWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(InWorkWebApp.Startup))]
namespace InWorkWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUsersAndRoles();
        }

        // Create default user roles and admin user for login
        private void CreateUsersAndRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!RoleManager.RoleExists("Administrador"))
            {
                var role = new IdentityRole("Administrador");
                RoleManager.Create(role);

                var user = new ApplicationUser
                {
                    UserName = ConfigurationManager.AppSettings["DefaultAdminUserName"],
                    Email = ConfigurationManager.AppSettings["DefaultAdminEmail"]
                };

                var password = ConfigurationManager.AppSettings["DefaultAdminPassword"];

                var chkUser = UserManager.Create(user, password);

                if (chkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "Administrador");
                }
            }

            if (!RoleManager.RoleExists("Editor"))
            {
                var role = new IdentityRole("Editor");
                RoleManager.Create(role);
            }

            if (!RoleManager.RoleExists("Empleado"))
            {
                var role = new IdentityRole("Empleado");
                RoleManager.Create(role);
            }

            if (!RoleManager.RoleExists("Usuario"))
            {
                var role = new IdentityRole("Usuario");
                RoleManager.Create(role);
            }
        }
    }
}
