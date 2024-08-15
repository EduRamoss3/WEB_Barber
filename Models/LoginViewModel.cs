using System.ComponentModel.DataAnnotations;

namespace Barber.UI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="O email é obrigatório!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Repita a senha!")]
        [Compare("Password",ErrorMessage ="As senhas não se coincidem!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
