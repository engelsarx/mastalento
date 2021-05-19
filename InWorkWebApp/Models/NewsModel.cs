using InWorkWebApp.Custom.Classes;
using InWorkWebApp.Custom.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace InWorkWebApp.Models
{
    [Table("AspNetNews")]
    public class NewsModel : AuditableContent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Imagen")]
        public byte[] Image { get; set; }

        [Display(Name = "Título"), Required(ErrorMessage = "Se requiere un {0}")]
        public string Title { get; set; }

        [Display(Name = "Subtítulo")]
        public string Subtitle { get; set; }

        [Display(Name = "Resumen"), Required(ErrorMessage = "Se requiere un {0}"), DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [AllowHtml]
        [UIHint("tinymce_full")]
        [Display(Name = "Descripción"), Required(ErrorMessage = "Se requiere una {0}")]
        public string Description { get; set; }

        [Display(Name = "Enlace o referencia")]
        public string Link { get; set; }

        [Display(Name = "Tipo de publicación"), EnumDataType(typeof(NewsTypesEnum))]
        public NewsTypesEnum NewsType { get; set; }

        [Display(Name = "Categoría"), ForeignKey("Category")]
        public int CategoryId { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public virtual CategoryModel Category { get; set; }
    }
}