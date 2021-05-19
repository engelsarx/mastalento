using System.ComponentModel.DataAnnotations;

namespace InWorkWebApp.Custom.Enums
{
    public enum SolutionTypesEnum
    {
        [Display(Name = "Producto")]
        PRODUCT = 1,

        [Display(Name = "Servicio")]
        SERVICE = 2
    }
}