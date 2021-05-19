using InWorkWebApp.Custom.Classes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace InWorkWebApp.Models
{
    [Table("AspNetDivisions")]
    public class DivisionModel : AuditableContent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Imagen")]
        public byte[] Image { get; set; }

        [Display(Name = "Nombre"), Required(ErrorMessage = "Se requiere un {0}"), StringLength(maximumLength: 50)]
        public string Name { get; set; }

        [Display(Name = "Resumen"), Required(ErrorMessage = "Se requiere un {0}"), DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [AllowHtml]
        [UIHint("tinymce_full")]
        [Display(Name = "Contenido"), Required(ErrorMessage = "Se requiere un {0}")]
        public string Content { get; set; }
    }
}