using InWorkWebApp.Custom.Classes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace InWorkWebApp.Models
{
    [Table("AspNetUserAdditionalData")]
    public class AdditionalUserDataModel : Auditable
    {
        [Key, ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Imagen")]
        public byte[] Image { get; set; }

        #region Personal data

        [Display(Name = "Nombre"), Required(ErrorMessage = "Se requiere un {0}"), StringLength(maximumLength: 50)]
        public string Name { get; set; }

        [Display(Name = "Apellido paterno"), Required(ErrorMessage = "Se requiere un {0}"), StringLength(maximumLength: 50)]
        public string LastName { get; set; }

        [Display(Name = "Apellido materno"), StringLength(maximumLength: 50)]
        public string Surname { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Género (H o M)"), Required(ErrorMessage = "Se requiere un {0}"), StringLength(maximumLength: 1)]
        public string Gender { get; set; }

        [Phone]
        [Display(Name = "Teléfono de contacto"), StringLength(maximumLength: 10)]
        public string ContactPhone { get; set; }

        [EmailAddress]
        [Display(Name = "Correo electrónico de contacto"), StringLength(maximumLength: 150)]
        public string ContactEmail { get; set; }

        [Display(Name = "Información adicional")]
        public string AdditionalInformation { get; set; }

        #endregion

        #region Address data

        [Display(Name = "Calle"), StringLength(maximumLength: 100)]
        public string Street { get; set; }

        [Display(Name = "No. Exterior"), StringLength(maximumLength: 50)]
        public string ExternalNumber { get; set; }

        [Display(Name = "No. Interior"), StringLength(maximumLength: 50)]
        public string InternalNumber { get; set; }

        [Display(Name = "Colonia"), StringLength(maximumLength: 50)]
        public string Colony { get; set; }

        [Display(Name = "Alcaldía o municipio"), StringLength(maximumLength: 50)]
        public string Suburb { get; set; }

        [Display(Name = "Código postal"), StringLength(maximumLength: 5)]
        public string ZipCode { get; set; }

        [Display(Name = "Ciudad"), StringLength(maximumLength: 50)]
        public string City { get; set; }

        [Display(Name = "País"), StringLength(maximumLength: 50)]
        public string Country { get; set; }

        #endregion
    }
}