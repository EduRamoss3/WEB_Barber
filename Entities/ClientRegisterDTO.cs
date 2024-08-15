using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Barber.UI.Entities
{
    public sealed record ClientRegisterDTO
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress]
        [StringLength(250, ErrorMessage = "Max 250 characters")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]

        public string Password { get; init; }

        [Required(ErrorMessage = "Password confirmation is required!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        [DisplayName("Confirmação de senha")]

        public string ConfirmPassword { get; init; }

        [Required(ErrorMessage = "Name is required!")]
        [StringLength(200, ErrorMessage = "Max 200 characters")]
        [DisplayName("Nome")]
        public string Name { get; init; }
    }
}
