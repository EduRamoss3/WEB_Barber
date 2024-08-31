using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Barber.UI.Entities.DTO
{
    public sealed record ClientDTO
    {
        [Key]
        public int Id { get; init; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [StringLength(200, ErrorMessage = "Máximo de  200 caracteres")]
        public string Name { get; init; }

        [DisplayName("Agendado?")]
        [Required(ErrorMessage = "'Agendado' é obrigatório")]
        public bool Scheduled { get; init; }

        [Required(ErrorMessage ="O email é obrigatório!")]
        [EmailAddress(ErrorMessage ="Deve conter '@' e no máximo 50 caracteres")]
        public string Email { get; init; }

        [DataType(DataType.DateTime)]
        [DisplayName("Ultima vez aqui")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime LastTimeHere { get; init; }


    }
}
