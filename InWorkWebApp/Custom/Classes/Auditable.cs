using System;
using System.ComponentModel.DataAnnotations;

namespace InWorkWebApp.Custom.Classes
{
    public class Auditable
    {
        [Display(Name = "Fecha de creación"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true), Required(ErrorMessage = "Se requiere una {0}")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Creado por"), Required(ErrorMessage = "Se requiere el campo '{0}'")]
        public string CreatedBy { get; set; }

        [Display(Name = "Fecha de modificación"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true), Required(ErrorMessage = "Se requiere una {0}")]
        public DateTime LastUpdateDate { get; set; }

        [Display(Name = "Modificado por"), Required(ErrorMessage = "Se requiere el campo '{0}' ")]
        public string ModifiedBy { get; set; }
    }
}