using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InWorkWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nombre"), Required(ErrorMessage = "Se requiere un {0}")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido paterno"), Required(ErrorMessage = "Se requiere un {0}")]
        public string LastName { get; set; }

        [Display(Name = "Apellido materno"), Required(ErrorMessage = "Se requiere un {0}")]
        public string SecondLastName { get; set; }

        public virtual ApplicationDataModel ApplicationDataModel { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<CategoryModel> CategoryModels { get; set; }

        public DbSet<DeletedInfoModel> DeletedInfoModels { get; set; }

        public DbSet<DivisionModel> DivisionModels { get; set; }

        public DbSet<FrequentlyAskedQuestionModel> FrequentlyAskedQuestionModels { get; set; }

        public DbSet<NewsModel> NewsModels { get; set; }

        public DbSet<SolutionModel> SolutionModels { get; set; }

        public DbSet<AggregateValueModel> AggregateValueModels { get; set; }

        public DbSet<ApplicationDataModel> ApplicationDataModels { get; set; }

        public DbSet<MessageModel> MessageModels { get; set; }
    }
}