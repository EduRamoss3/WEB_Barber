using System.ComponentModel.DataAnnotations;

namespace Barber.UI.Entities.DTO
{
    public sealed record BarberDTO 
    {
        [Key]
        public int Id { get; init; }
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [StringLength(200, ErrorMessage = "Max 200 caracteres")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Coloque uma disponibilidade!")]
        public bool Disponibility { get; init; }

        public List<SchedulesDTO> Schedules { get; init; } = new List<SchedulesDTO>();

    }
}
