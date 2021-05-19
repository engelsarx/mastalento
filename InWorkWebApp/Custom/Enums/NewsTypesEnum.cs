using System.ComponentModel.DataAnnotations;

namespace InWorkWebApp.Custom.Enums
{
    public enum NewsTypesEnum
    {
        [Display(Name = "Noticia")]
        NEWS = 1,

        [Display(Name = "Consejo")]
        ADVICE = 2
    }
}