using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InWorkWebApp.Models
{
    [Table("AspNetDeletedInfo")]
    public class DeletedInfoModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Tipo de contenido"), Required(ErrorMessage = "Se requiere un {0}")]
        public string ContentType { get; set; }

        [Display(Name = "Modelo de datos")]
        public string DataModel { get; set; }

        [Display(Name = "Fecha de eliminación"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true), Required(ErrorMessage = "Se requiere una {0}")]
        public DateTime DeletedDate { get; set; }

        [Display(Name = "Eliminado por"), Required(ErrorMessage = "Se requiere el campo '{0}' ")]
        public string DeletedBy { get; set; }
    }
}