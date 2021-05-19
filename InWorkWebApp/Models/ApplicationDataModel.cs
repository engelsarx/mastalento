using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InWorkWebApp.Models
{
    [Table("AspNetApplicationData")]
    public class ApplicationDataModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Display(Name = "Logo")]
        public byte[] Logo { get; set; }

        [Display(Name = "Marca"), StringLength(maximumLength: 50)]
        public string Brand { get; set; }

        [Display(Name = "Eslogan"), StringLength(maximumLength: 150)]
        public string Slogan { get; set; }

        [Display(Name = "Texto destacado")]
        public string Intro { get; set; }

        [Display(Name = "Valores agregados")]
        public bool ShowAggregateValuesOnMainPage { get; set; }

        [Display(Name = "Grupos")]
        public bool ShowDivisionsOnMainPage { get; set; }

        [Display(Name = "Soluciones")]
        public bool ShowSolutionsOnMainPage { get; set; }

        [Display(Name = "Publicaciones")]
        public bool ShowNewsOnMainPage { get; set; }

        [Display(Name = "Preguntas frecuentes")]
        public bool ShowFAQOnMainPage { get; set; }

        //[Display(Name = "No. de valores agregados")]
        //public int MaxAggregateValuesNumber { get; set; }

        //[Display(Name = "No. de divisiones")]
        //public int MaxDivisionsNumber { get; set; }

        //[Display(Name = "No. de soluciones")]
        //public int MaxSolutionsNumber { get; set; }

        //[Display(Name = "No. de noticias")]
        //public int MaxNewsNumber { get; set; }

        //[Display(Name = "No. de preguntas frecuentes")]
        //public int MaxFAQNumber { get; set; }

        #region About

        [Display(Name = "Texto destacado para la página"), StringLength(maximumLength: 50)]
        public string AboutIntro { get; set; }

        [Display(Name = "Descripción de la página"), StringLength(maximumLength: 255)]
        public string AboutDescription { get; set; }

        [Display(Name = "Icono elemento destacado 1"), StringLength(maximumLength: 25)]
        public string SquareIconOne { get; set; }

        [Display(Name = "Texto elemento destacado 1"), StringLength(maximumLength: 25)]
        public string SquareTextOne { get; set; }

        [Display(Name = "Icono elemento destacado 2"), StringLength(maximumLength: 25)]
        public string SquareIconTwo { get; set; }

        [Display(Name = "Texto elemento destacado 2"), StringLength(maximumLength: 25)]
        public string SquareTextTwo { get; set; }

        [Display(Name = "Texto principal"), StringLength(maximumLength: 255)]
        public string AboutPrimaryBanner { get; set; }

        [Display(Name = "Texto secundario"), StringLength(maximumLength: 255)]
        public string AboutSecondaryBanner { get; set; }

        [Display(Name = "Texto alternativo"), StringLength(maximumLength: 255)]
        public string AboutAlternativeBanner { get; set; }

        [Display(Name = "Icono diferenciador 1"), StringLength(maximumLength: 25)]
        public string ItemIconOne { get; set; }

        [Display(Name = "Nombre del diferenciador 1"), StringLength(maximumLength: 25)]
        public string ItemNameOne { get; set; }

        [Display(Name = "Descripción diferenciador 1"), StringLength(maximumLength: 150)]
        public string ItemDescriptionOne { get; set; }

        [Display(Name = "Icono diferenciador 2"), StringLength(maximumLength: 25)]
        public string ItemIconTwo { get; set; }

        [Display(Name = "Nombre del diferenciador 2"), StringLength(maximumLength: 25)]
        public string ItemNameTwo { get; set; }

        [Display(Name = "Descripción diferenciador 2"), StringLength(maximumLength: 150)]
        public string ItemDescriptionTwo { get; set; }

        [Display(Name = "Icono diferenciador 3"), StringLength(maximumLength: 25)]
        public string ItemIconThree { get; set; }

        [Display(Name = "Nombre del diferenciador 3"), StringLength(maximumLength: 25)]
        public string ItemNameThree { get; set; }

        [Display(Name = "Descripción diferenciador 3"), StringLength(maximumLength: 150)]
        public string ItemDescriptionThree { get; set; }

        [Display(Name = "Icono diferenciador 4"), StringLength(maximumLength: 25)]
        public string ItemIconFour { get; set; }

        [Display(Name = "Nombre del diferenciador 4"), StringLength(maximumLength: 25)]
        public string ItemNameFour { get; set; }

        [Display(Name = "Descripción diferenciador 4"), StringLength(maximumLength: 150)]
        public string ItemDescriptionFour { get; set; }

        #endregion

        #region Divisions

        [Display(Name = "Texto destacado para la página"), StringLength(maximumLength: 50)]
        public string DivisionsIntro { get; set; }

        [Display(Name = "Descripción de la página"), StringLength(maximumLength: 255)]
        public string DivisionsDescription { get; set; }

        #endregion

        #region Solutions

        [Display(Name = "Texto destacado para la página"), StringLength(maximumLength: 50)]
        public string SolutionsIntro { get; set; }

        [Display(Name = "Descripción de la página"), StringLength(maximumLength: 255)]
        public string SolutionsDescription { get; set; }

        #endregion

        #region News

        [Display(Name = "Texto destacado para la página"), StringLength(maximumLength: 50)]
        public string NewsIntro { get; set; }

        [Display(Name = "Descripción de la página"), StringLength(maximumLength: 255)]
        public string NewsDescription { get; set; }

        #endregion

        #region Contact

        [Display(Name = "Texto destacado para la página"), StringLength(maximumLength: 50)]
        public string ContactIntro { get; set; }

        [Display(Name = "Descripción de la página"), StringLength(maximumLength: 255)]
        public string ContactDescription { get; set; }

        [Display(Name = "Correo electrónico de contacto"), EmailAddress(ErrorMessage = "Se esperaba un {0} válido")]
        public string ContactEmail { get; set; }

        [Display(Name = "Teléfono de contacto"), Phone(ErrorMessage = "Se esperaba un {0} válido")]
        public string ContactPhoneNumber { get; set; }

        #endregion

        #region FAQ

        [Display(Name = "Texto destacado para la página"), StringLength(maximumLength: 50)]
        public string FAQIntro { get; set; }

        [Display(Name = "Descripción de la página"), StringLength(maximumLength: 255)]
        public string FAQDescription { get; set; }

        #endregion
    }
}