using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InWorkWebApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Display(Name = "Correo electrónico"), Required(ErrorMessage = "Se requiere un {0}")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Código")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "¿Recordar este navegador?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Display(Name = "Correo electrónico"), Required(ErrorMessage = "Se requiere un {0}")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Display(Name = "Correo electrónico"), EmailAddress(ErrorMessage = "Se esperaba un {0} válido"), Required(ErrorMessage = "Se requiere un {0}")]
        public string Email { get; set; }

        [Display(Name = "Contraseña"), DataType(DataType.Password), Required(ErrorMessage = "Se requiere una {0}")]
        public string Password { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "Nombre(s)"), Required(ErrorMessage = "Se requiere un {0}")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido paterno"), Required(ErrorMessage = "Se requiere un {0}")]
        public string LastName { get; set; }

        [Display(Name = "Apellido materno"), Required(ErrorMessage = "Se requiere un {0}")]
        public string SecondLastName { get; set; }

        [Display(Name = "Teléfono celular"), Phone(ErrorMessage = "Se esperaba un {0} válido"), Required(ErrorMessage = "Se requiere un {0}")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Correo electrónico"), EmailAddress(ErrorMessage = "Se esperaba un {0} válido"), Required(ErrorMessage = "Se requiere un {0}")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "La {0} debe contener al menos {2} caracteres de longitud.", MinimumLength = 8)]
        [Display(Name = "Contraseña"), DataType(DataType.Password), Required(ErrorMessage = "Se requiere una {0}")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmación de contraseña"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Rol de usuario"), Required(ErrorMessage = "Se requiere un {0}")]
        public string UserRole { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Display(Name = "Correo electrónico"), EmailAddress(ErrorMessage = "Se esperaba un {0} válido"), Required(ErrorMessage = "Se requiere un {0}")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "La {0} debe contener al menos {2} caracteres de longitud.", MinimumLength = 8)]
        [Display(Name = "Contraseña"), DataType(DataType.Password), Required(ErrorMessage = "Se requiere una {0}")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmación de contraseña"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Código")]
        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Display(Name = "Correo electrónico"), EmailAddress(ErrorMessage = "Se esperaba un {0} válido"), Required(ErrorMessage = "Se requiere un {0}")]
        public string Email { get; set; }
    }
}
