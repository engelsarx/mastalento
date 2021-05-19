using InWorkWebApp.Custom.Classes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InWorkWebApp.Models
{
    [Table("AspNetCategories")]
    public class CategoryModel : Auditable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Nombre de la categoría"), Required(ErrorMessage = "Se requiere un {0}"), StringLength(maximumLength: 50)]
        public string Name { get; set; }
    }
}