using System.ComponentModel.DataAnnotations;

namespace InWorkWebApp.Custom.Classes
{
    public class AuditableContent : Auditable
    {
        [Display(Name = "Fecha de publicación"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string PublishDate { get; set; }

        [Display(Name = "Fecha de expiración"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string ExpirationDate { get; set; }
    }
}