using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InWorkWebApp.Models
{
    [Table("AspNetApplicationMessages")]
    public class MessageModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Display(Name = "Nombre"), Required(ErrorMessage = "Se requiere un {0}"), StringLength(maximumLength: 50)]
        public string Name { get; set; }

        [Display(Name = "Correo electrónico"), EmailAddress, Required(ErrorMessage = "Se requiere un {0}"), StringLength(maximumLength: 150)]
        public string Email { get; set; }

        [Display(Name = "Teléfono celular"), StringLength(maximumLength: 10)]
        public string MobilePhone { get; set; }

        [Display(Name = "Asunto"), StringLength(maximumLength: 50)]
        public string Affair { get; set; }

        [Display(Name = "Mensaje"), Required(ErrorMessage = "Se requiere un {0}"), DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Display(Name = "Fecha de registro"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string RegisterDate { get; set; }
    }
}