using InWorkWebApp.Custom.Classes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace InWorkWebApp.Models
{
    [Table("AspNetFAQ")]
    public class FrequentlyAskedQuestionModel : AuditableContent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Pregunta"), Required(ErrorMessage = "Se requiere una {0}")]
        public string Question { get; set; }

        [AllowHtml]
        [UIHint("tinymce_full")]
        [Display(Name = "Respuesta"), Required(ErrorMessage = "Se requiere una {0}")]
        public string Answer { get; set; }
    }
}