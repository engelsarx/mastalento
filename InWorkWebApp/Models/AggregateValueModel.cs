using InWorkWebApp.Custom.Classes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InWorkWebApp.Models
{
    [Table("AspNetAggregateValues")]
    public class AggregateValueModel : Auditable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Código ícono"), StringLength(maximumLength: 25)]
        public string IconCode { get; set; }

        [Display(Name = "Título"), Required(ErrorMessage = "Se requiere un {0}"), StringLength(maximumLength: 50)]
        public string Title { get; set; }

        [Display(Name = "Contenido"), Required(ErrorMessage = "Se requiere un {0}")]
        public string Content { get; set; }
    }
}